using CodeBarsGenerator.Validator;
using SkiaSharp;
using ZXing;
using ZXing.SkiaSharp;

namespace CodeBarsGenerator.Service
{
    public class QrCodeService : IQrcodeService
    {
        public byte[] GerarQrCode(string codigo)
        {
            BarcodeValidator.Validar(codigo);

            // 1. Utilizamos o BarcodeWriter do namespace ZXing.SkiaSharp
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300,
                    Margin = 1,
                    PureBarcode = true                    
                }
            };

            // 2. Gera o SKBitmap (nativo do SkiaSharp, roda em Linux)
            using var bitmap = writer.Write(codigo);

            // 3. Converte o bitmap para uma imagem (PNG é mais recomendado que BMP para web/linux)
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            // 4. Retorna os bytes diretamente
            return data.ToArray();
        }
    }
}