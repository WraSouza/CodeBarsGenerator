

using CodeBarsGenerator.Service;

namespace CodeBarsGenerator.Endpoints
{
    public static class GenGs1CodeBarsEndpoint
    {
        public static void GenGs1CodeBarsEndpoints(this IEndpointRouteBuilder endpoints)
        {
           endpoints.MapGet("/GenGs1CodeBars/{codigo}", (string codigo,IBarcodeService service,ILogger<Program> logger) =>
            {
                try
                {
                    var imagem = service.GerarCodigoBarrasGs1(codigo);

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
            .WithName("GetGs1Barcode")
            .WithMetadata(new Microsoft.AspNetCore.RateLimiting.EnableRateLimitingAttribute("PorIp"))
            .MapToApiVersion(1.0);
        }
    }
}