using Asp.Versioning;
using CodeBarsGenerator.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CodeBarsGenerator.Controllers
{
    [ApiController]
    [ApiVersion(1)]       
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenCodeBarsTestController(IBarcodeService service,ILogger<GenCodeBarsTestController> logger) : ControllerBase
    {        
        
        [HttpGet("{codigo}")]
        [MapToApiVersion(1)]
        [EnableRateLimiting("PorIp")]
        public IActionResult GetBarcode(string codigo)
        {
            try
            {
                var imagem = service.GerarCodigoBarras(codigo);               

                return File(imagem, "image/png");
            }
            catch (ArgumentException ex)
            {
                 logger.LogInformation("Código inválido: {Codigo}, erro: {Erro}", codigo, ex.Message);

                return BadRequest(ex.Message);
               
            }
            catch (Exception)
            {
                logger.LogError("Erro ao gerar o código de barras para {Codigo}", codigo);

                return StatusCode(500, "Erro ao gerar o código de barras.");
                
            }

        }
        
    }
    
}
