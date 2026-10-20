

using CodeBarsGenerator.Service;

namespace CodeBarsGenerator.Endpoints
{
    public static class GenQrCodeEndpoint
    {
        public static void GenQrCodeEndpoints(this IEndpointRouteBuilder endpoints)
        {
           endpoints.MapGet("/GenQrCode/{codigo}", (string codigo,IQrcodeService service,ILogger<Program> logger) =>
            {
                try
                {
                    var texto = Uri.UnescapeDataString(codigo);

                    var imagem = service.GerarQrCode(texto);

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
            .WithName("GetQrCode")
            .WithMetadata(new Microsoft.AspNetCore.RateLimiting.EnableRateLimitingAttribute("PorIp"))
            .MapToApiVersion(1.0);
        }
    }
}