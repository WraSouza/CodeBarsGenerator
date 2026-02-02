using CodeBarsGenerator.Validator;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Windows.Compatibility;

namespace CodeBarsGenerator.Service
{
    public class BarCodeService : IBarcodeService
    {
        public byte[] GerarCodigoBarras(string codigo)
        {
            //throw new NotImplementedException();
            BarcodeValidator.Validar(codigo);

            // 1. Configura o escritor
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 150,
                    Margin = 10,
                    PureBarcode = true
                }
            };

            // 2. Gera o Bitmap (Sem precisar de Renderer manual, o Binding Windows já resolve)
            using Bitmap bitmap = writer.Write(codigo);

            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);

            return ms.ToArray();
        }
    }
}
