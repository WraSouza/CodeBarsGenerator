using Asp.Versioning;
using CodeBarsGenerator.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CodeBarsGenerator.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenPureCodeBarsController(IBarcodeService service) : ControllerBase
    {
       
        [HttpGet("{codigo}")]
        [MapToApiVersion(1)]
        [EnableRateLimiting("PorIp")]
        public IActionResult GetBarcodeV1(string codigo)
        {
            try
            {
                var imagem = service.GerarCodigoBarrasPuro(codigo);

                return File(imagem, "image/bmp");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao gerar o código de barras.");
            }

        }
    }
}
