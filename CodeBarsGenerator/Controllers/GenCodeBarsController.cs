using Asp.Versioning;
using CodeBarsGenerator.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeBarsGenerator.Controllers
{
    // Uma base para tudo que for V1
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
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

    // Uma base para tudo que for V2
    [ApiVersion(2.0)]    
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenCodeBarsV2Controller(IBarcodeService service) : ControllerBase
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
