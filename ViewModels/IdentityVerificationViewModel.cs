using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.Models;
using Newtonsoft.Json;
using ServiPuntos.uy_mobile.Models.Enums;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;


namespace ServiPuntos.uy_mobile.ViewModels;

public partial class IdentityVerificationViewModel(IAuthService authService) : ObservableObject
{
  [ObservableProperty] private string? _documentNumber;
  [ObservableProperty] private string? _serialNumber;

  [RelayCommand]
  private async Task VerifyIdentity()
  {
    if (DocumentNumber == null)
    {
      await Toast.Make("Debes ingresar tu numero de cedula y de serie", ToastDuration.Short).Show();
      return;
    }
    try
    {
      var userDTOResponse = await authService.VerifyIdentity(new Document(DocumentNumber, SerialNumber));
      if (userDTOResponse is { Error: false, Data: not null })
      {
        var userJson = JsonConvert.SerializeObject(userDTOResponse.Data);
        await SecureStorage.SetAsync(SecureStorageType.User.ToString(), userJson);
        await Shell.Current.DisplayAlert("Identidad verificada con éxito.", "Podrás acceder a ofertas exclusivas!", "Volver");
        await Shell.Current.GoToAsync("..");
      }
      else
      {
        await Toast.Make(userDTOResponse.Message, ToastDuration.Short).Show();
      }
    }
    catch (Exception ex)
    {
      await Toast.Make(ex.Message, ToastDuration.Short).Show();
    }
  }

  public void Reset()
  {
    DocumentNumber = null;
    SerialNumber = null;
  }
}
