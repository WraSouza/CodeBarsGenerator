

using CodeBarsGenerator.Service;

namespace CodeBarsGenerator.Endpoints
{
    public static class GenPureCodeBarsEndpoint
    {
        public static void GenPureCodeBarsEndpoints(this IEndpointRouteBuilder endpoints)
        {
           endpoints.MapGet("/GenPureCodeBars/{codigo}", (string codigo,IBarcodeService service,ILogger<Program> logger) =>
            {
                try
                {
                    var imagem = service.GerarCodigoBarrasPuro(codigo);

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
            .WithName("GetPureBarcode")
            .WithMetadata(new Microsoft.AspNetCore.RateLimiting.EnableRateLimitingAttribute("PorIp"))
            .MapToApiVersion(1.0);
        }
    }
}