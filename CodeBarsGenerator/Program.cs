var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

builder.Host.UseWindowsService();

builder.WebHost.UseSentry(o =>
{
    o.Dsn = "https://df00c9d1e2faa19d66effc9935e6ef58@o4508410133479424.ingest.us.sentry.io/4510782764613632";
    // When configuring for the first time, to see what the SDK is doing:
    o.Debug = true;
    o.TracesSampleRate = 0.1;
});

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
