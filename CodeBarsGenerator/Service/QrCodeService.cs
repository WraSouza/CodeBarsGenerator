using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Windows.Compatibility;

namespace CodeBarsGenerator.Service
{
    public class QrCodeService : IQrcodeService
    {
        public byte[] GerarQrCode(string codigo)
        {
            var writer = new BarcodeWriter
            {
                // A única mudança real é aqui: de CODE_128 para QR_CODE
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.QrCode.QrCodeEncodingOptions
                {
                    Width = 300,  // QR Codes costumam ser quadrados
                    Height = 300,
                    Margin = 1,   // Margem branca ao redor
                    CharacterSet = "UTF-8" // Garante que acentos funcionem no QR Code
                }
            };

            using Bitmap bitmap = writer.Write(codigo);
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);

            return ms.ToArray();
        }
    }
}
