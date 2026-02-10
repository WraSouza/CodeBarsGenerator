using Asp.Versioning;
using CodeBarsGenerator.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeBarsGenerator.Controllers
{
    [ApiController]
    [ApiVersion(1)]    
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenQrCodesController(IQrcodeService service) : ControllerBase
    {
        [MapToApiVersion(1)]
        [HttpGet("{codigo}")]
        public IActionResult GetQrcode(string codigo)
        {
            try
            {
                var imagem = service.GerarQrCode(codigo);

                return File(imagem, "image/bmp");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao gerar o QR Code.");
            }

        }
    }
}
