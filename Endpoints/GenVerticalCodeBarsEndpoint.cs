using CodeBarsGenerator.Service;

namespace CodeBarsGenerator.Endpoints
{
    public static class GenVerticalCodeBarsEndpoint
    {
        public static void GenVerticalCodeBarsEndpoints(this IEndpointRouteBuilder endpoints)
        {
           endpoints.MapGet("/GenVerticalCodeBars/{codigo}/{rotate?}", (string codigo,IBarcodeService service, ILogger<Program> logger,int? rotate = null) =>
            {
               try
                {
                    int angulo = rotate ?? 90;
                    var imagem = service.GerarCodigoBarrasVertical(codigo, angulo);               

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
            .WithName("GetVerticalBarcode")
            .WithMetadata(new Microsoft.AspNetCore.RateLimiting.EnableRateLimitingAttribute("PorIp"))
            .MapToApiVersion(1.0);
        }
    }
}