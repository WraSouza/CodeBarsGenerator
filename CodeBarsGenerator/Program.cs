using Asp.Versioning;
using CodeBarsGenerator.Service;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Host.UseWindowsService();

var sdn = builder.Configuration.GetConnectionString("SentryDsn");

// Registra o documento para a V1
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "API - V1";
        return Task.CompletedTask;
    });
});

//// Registra o documento para a V2
builder.Services.AddOpenApi("v2", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "API - V2";
        return Task.CompletedTask;
    });
});

builder.WebHost.UseSentry(o =>
{
    o.Dsn = sdn;
    // When configuring for the first time, to see what the SDK is doing:
    o.Debug = true;
    o.TracesSampleRate = 0.1;
});


//// --- INICIO DA CONFIGURAÇÃO DE VERSIONAMENTO ---
var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// É aqui que a mágica acontece para o Swagger e o Explorer
apiVersioningBuilder.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
//// --- FIM DA CONFIGURAÇÃO ---

builder.Services.AddScoped<IBarcodeService, BarCodeService>();
builder.Services.AddScoped<IQrcodeService, QrCodeService>();

var app = builder.Build();

app.UseSentryTracing();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapOpenApi();
    app.MapScalarApiReference();

    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
