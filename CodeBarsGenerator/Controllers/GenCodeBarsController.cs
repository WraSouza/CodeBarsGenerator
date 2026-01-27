using CodeBarsGenerator.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeBarsGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenCodeBarsController(IBarcodeService service) : ControllerBase
    {

        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetBarcode(string codigo)
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
