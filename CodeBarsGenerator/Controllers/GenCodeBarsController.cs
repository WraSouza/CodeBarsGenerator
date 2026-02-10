using Asp.Versioning;
using CodeBarsGenerator.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeBarsGenerator.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [ApiVersion(2)]   
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenCodeBarsController(IBarcodeService service) : ControllerBase
    {
        
        [ApiVersion(1)]
        [HttpGet("{codigo}")]
        public IActionResult GetBarcodeV1(string codigo)
        {
            try
            {
                var imagem = service.GerarCodigoBarras(codigo);

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
