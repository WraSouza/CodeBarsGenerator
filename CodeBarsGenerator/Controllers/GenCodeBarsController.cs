using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Windows.Compatibility;

namespace CodeBarsGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenCodeBarsController : ControllerBase
    {

        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetBarcode(string codigo)
        {
            // 1. Configura o escritor
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 150,
                    Margin = 10
                }
            };

            // 2. Gera o Bitmap (Sem precisar de Renderer manual, o Binding Windows já resolve)
            using Bitmap bitmap = writer.Write(codigo);

            // 3. Converte para Stream BMP
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);

            return File(ms.ToArray(), "image/bmp");

        }
    }
}
