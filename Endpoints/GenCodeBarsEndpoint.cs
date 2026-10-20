

using CodeBarsGenerator.Service;

namespace CodeBarsGenerator.Endpoints
{
    public static class GenCodeBarsEndpoint
    {
        public static void GenCodeBarsEndpoints(this IEndpointRouteBuilder endpoints)
        {
           endpoints.MapGet("/GenCodeBars/{codigo}", (string codigo,IBarcodeService service,ILogger<Program> logger) =>
            {
                try
                {
                    var imagem = service.GerarCodigoBarras(codigo);

                    return Results.File(imagem, "image/png");
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception)
                {
                    logger.LogError("Erro ao gerar o código de barras para {Codigo}", codigo);
                    return Results.StatusCode(500);
                }
            })
            .WithName("GetBarcode")
            .WithMetadata(new Microsoft.AspNetCore.RateLimiting.EnableRateLimitingAttribute("PorIp"))
            .MapToApiVersion(1.0);
        }
    }
}