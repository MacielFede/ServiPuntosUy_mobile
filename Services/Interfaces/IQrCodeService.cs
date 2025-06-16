using Microsoft.Maui.Controls;

namespace ServiPuntosUy_mobile.Services.Interfaces;

public interface IQrCodeService
{
  ImageSource GenerateQrCode(string text);
}
