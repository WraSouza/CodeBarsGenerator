using CodeBarsGenerator.Helper;
using CodeBarsGenerator.Validator;
using SkiaSharp;
using ZXing;
using ZXing.SkiaSharp;

namespace CodeBarsGenerator.Service
{
    public class BarCodeService : IBarcodeService
    {
        // Resolve a fonte uma única vez (é um pouco custoso, então não faz sentido
        // repetir a busca a cada chamada). Cai para SKTypeface.Default como fallback
        // caso a DejaVu Sans não esteja instalada no ambiente (ex: rodando fora do container).
        private static readonly SKTypeface _typeface =
            SKFontManager.Default.MatchFamily("DejaVu Sans")
            ?? SKTypeface.Default;

        private static SKFont CriarFonte(float size = 25.0f) =>
            new SKFont(_typeface) { Size = size, Embolden = true };

        public byte[] GerarCodigoBarras(string codigo)
        {
            BarcodeValidator.Validar(codigo);

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 500,
                    Height = 90,
                    Margin = 0,
                    PureBarcode = true
                }
            };

            using var barcodeBitmap = writer.Write(codigo);

            int extraHeight = 35;
            using var surface = SKSurface.Create(new SKImageInfo(barcodeBitmap.Width, barcodeBitmap.Height + extraHeight));
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.White);
            canvas.DrawBitmap(barcodeBitmap, 0, 0, SKSamplingOptions.Default, null);

            using var font = CriarFonte();

            var paint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true
            };

            canvas.DrawText(codigo, barcodeBitmap.Width / 2, barcodeBitmap.Height + 30, SKTextAlign.Center, font, paint);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

        public byte[] GerarCodigoBarrasPuro(string codigo)
        {
            BarcodeValidator.Validar(codigo);

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 350,
                    Height = 150,
                    Margin = 0,
                    PureBarcode = true
                }
            };

            using var bitmap = writer.Write(codigo);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            return data.ToArray();
        }


        public byte[] GerarCodigoBarrasVertical(string codigo, int anguloRotacao = 90)
        {
            BarcodeValidator.Validar(codigo);

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 500,
                    Height = 90,
                    Margin = 0,
                    PureBarcode = true
                }
            };

            using var barcodeBitmap = writer.Write(codigo);

            var nearestSampling = new SKSamplingOptions(SKFilterMode.Nearest, SKMipmapMode.None);

            if (anguloRotacao == 0)
            {
                using var img0 = SKImage.FromBitmap(barcodeBitmap);
                using var d0 = img0.Encode(SKEncodedImageFormat.Png, 100);
                return d0.ToArray();
            }

            bool swapDimensions = anguloRotacao == 90 || anguloRotacao == 270;
            int rotatedWidth = swapDimensions ? barcodeBitmap.Height : barcodeBitmap.Width;
            int rotatedHeight = swapDimensions ? barcodeBitmap.Width : barcodeBitmap.Height;

            using var rotatedSurface = SKSurface.Create(new SKImageInfo(rotatedWidth, rotatedHeight));
            var rotatedCanvas = rotatedSurface.Canvas;

            switch (anguloRotacao)
            {
                case 90:
                    rotatedCanvas.Translate(0, rotatedHeight);
                    rotatedCanvas.RotateDegrees(270);
                    break;
                case 180:
                    rotatedCanvas.Translate(rotatedWidth, rotatedHeight);
                    rotatedCanvas.RotateDegrees(180);
                    break;
                case 270:
                    rotatedCanvas.Translate(rotatedWidth, 0);
                    rotatedCanvas.RotateDegrees(90);
                    break;
            }

            using var barcodeImage = SKImage.FromBitmap(barcodeBitmap);
            rotatedCanvas.DrawImage(barcodeImage, 0, 0, nearestSampling, null);

            using var image = rotatedSurface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }

        public byte[] GerarCodigoBarrasGs1(string codigoComParenteses)
        {
            BarcodeValidator.Validar(codigoComParenteses);

            var codigoGs1 = Gs1Converter.ConverterParaGs1(codigoComParenteses);

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 700,
                    Height = 120,
                    Margin = 0,
                    PureBarcode = true
                }
            };

            using var barcodeBitmap = writer.Write(codigoGs1);

            var nearestSampling = new SKSamplingOptions(SKFilterMode.Nearest, SKMipmapMode.None);

            using var font = CriarFonte();
            font.Size = 20f;
            using var paint = new SKPaint { Color = SKColors.Black, IsAntialias = true };

            // Mede a largura real que o texto vai ocupar
            float textWidth = font.MeasureText(codigoComParenteses, paint);

            int extraHeight = 35;
            int barcodeCenterOffset = 0;

            // Usa a maior largura entre o barcode e o texto, centralizando o barcode se o texto for maior
            int width = Math.Max(barcodeBitmap.Width, (int)Math.Ceiling(textWidth));
            int height = barcodeBitmap.Height + extraHeight;

            if (width > barcodeBitmap.Width)
                barcodeCenterOffset = (width - barcodeBitmap.Width) / 2;

            using var surface = SKSurface.Create(new SKImageInfo(width, height));
            var canvas = surface.Canvas;

            canvas.Clear(SKColors.White);
            canvas.DrawBitmap(barcodeBitmap, barcodeCenterOffset, 0, nearestSampling, null);

            canvas.DrawText(codigoComParenteses, width / 2f, barcodeBitmap.Height + 30, SKTextAlign.Center, font, paint);

            using var image = surface.Snapshot();
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            return data.ToArray();
        }        
    }
}