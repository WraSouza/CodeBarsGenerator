using Asp.Versioning;
using CodeBarsGenerator.Service;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Slack;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var slackWebhook = builder.Configuration["Slack:WebhookUrl"];

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()

    // Console recebe Information, Warning, Error...
    .WriteTo.Console()

    // Slack recebe somente Warning ou superior
    .WriteTo.Slack(
        slackWebhook!,
        restrictedToMinimumLevel: LogEventLevel.Warning)

    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("PorIp", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
        var logger = context.HttpContext.RequestServices
            .GetRequiredService<ILogger<Program>>();

        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var path = context.HttpContext.Request.Path;

        logger.LogWarning(
            "Rate limit excedido. IP: {IP}, Endpoint: {Endpoint}",
            ip,
            path);

        context.HttpContext.Response.StatusCode =
            StatusCodes.Status429TooManyRequests;

        await context.HttpContext.Response.WriteAsync(
            "Limite de requisições excedido. Tente novamente mais tarde.",
            cancellationToken);
    };
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Host.UseWindowsService();

builder.Services.AddHealthChecks();

var sdn = builder.Configuration.GetConnectionString("SentryDsn");

builder.Logging.AddConsole();

var useSentry = !string.IsNullOrWhiteSpace(sdn);

if (useSentry)
{
    builder.WebHost.UseSentry(o =>
    {
        o.Dsn = sdn;
        // When configuring for the first time, to see what the SDK is doing:
        o.Debug = true;
        o.TracesSampleRate = 0.1;
    });
}


builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
})
.AddMvc() // This is needed for controllers
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddScoped<IBarcodeService, BarCodeService>();
builder.Services.AddScoped<IQrcodeService, QrCodeService>();

var app = builder.Build();

if (useSentry)
{
    app.UseSentryTracing();
}

app.MapOpenApi();
app.MapScalarApiReference();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.MapHealthChecks("/health");

app.MapGet("/", () => Results.Redirect("/scalar/"));

app.UseAuthorization();

app.UseRateLimiter();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status404NotFound)
    {
        var logger = context.RequestServices
            .GetRequiredService<ILogger<Program>>();

        logger.LogWarning(
            "Endpoint não encontrado. Method: {Method}, Path: {Path}, QueryString: {QueryString}",
            context.Request.Method,
            context.Request.Path,
            context.Request.QueryString);
    }
});

app.MapControllers();

app.Run();
