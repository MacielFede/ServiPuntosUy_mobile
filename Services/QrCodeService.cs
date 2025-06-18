using Microsoft.Maui.Controls;
using QRCoder;
using SkiaSharp;
using ServiPuntosUy_mobile.Services.Interfaces;
using System.IO;

namespace ServiPuntosUy_mobile.Services;

public class QrCodeService : IQrCodeService
{
  public ImageSource GenerateQrCode(string text)
  {
    using var qrGenerator = new QRCodeGenerator();
    using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

    int pixelsPerModule = 10;
    int size = qrCodeData.ModuleMatrix.Count * pixelsPerModule;

    using var surface = SKSurface.Create(new SKImageInfo(size, size));
    var canvas = surface.Canvas;
    canvas.Clear(SKColors.White);

    using var paint = new SKPaint { Color = SKColors.Black, IsAntialias = false };

    for (int y = 0; y < qrCodeData.ModuleMatrix.Count; y++)
    {
      for (int x = 0; x < qrCodeData.ModuleMatrix[y].Count; x++)
      {
        if (qrCodeData.ModuleMatrix[y][x])
        {
          canvas.DrawRect(x * pixelsPerModule, y * pixelsPerModule, pixelsPerModule, pixelsPerModule, paint);
        }
      }
    }

    using var image = surface.Snapshot();
    using var data = image.Encode(SKEncodedImageFormat.Png, 100);
    using var stream = new MemoryStream();
    data.SaveTo(stream);
    stream.Position = 0;

    return ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));
  }
}
