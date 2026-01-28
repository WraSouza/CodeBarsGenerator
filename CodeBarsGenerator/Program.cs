using CodeBarsGenerator.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

builder.Host.UseWindowsService();

var sdn = builder.Configuration.GetConnectionString("SentryDsn");

builder.WebHost.UseSentry(o =>
{
    o.Dsn = sdn;
    // When configuring for the first time, to see what the SDK is doing:
    o.Debug = true;
    o.TracesSampleRate = 0.1;
});

builder.Services.AddScoped<IBarcodeService, BarCodeService>();
builder.Services.AddScoped<IQrcodeService, QrCodeService>();

var app = builder.Build();

app.UseSentryTracing();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
