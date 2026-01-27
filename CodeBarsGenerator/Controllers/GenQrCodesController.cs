using CodeBarsGenerator.Helper;
using CodeBarsGenerator.Service;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Windows.Compatibility;

namespace CodeBarsGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenQrCodesController(IQrcodeService service) : ControllerBase
    {       

        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetQrcode(string codigo)
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
                return StatusCode(500, "Erro ao gerar o código de barras.");
            }


        }
    }
}
