using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.Views;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class LoginViewModel(IAuthService authService, IConfiguration _configs) : ObservableObject
{
  [ObservableProperty] private string? _email;
  [ObservableProperty] private string? _TenantName;
  [ObservableProperty] private string? _password;

  [RelayCommand]
  private async Task Login()
  {
    try
    {
      if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
      {
        await Shell.Current.DisplayAlert("Error en el inicio de sesión", "Debes ingresar email y contraseña", "OK");
        return;
      }
      var loginResult = await authService.Login(Email, Password);
      if (loginResult is { Error: false, Data: not null })
      {
        await authService.SaveSession(loginResult.Data);
        authService.TriggerSessionCreatedEvent();
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
      }
      else
      {
        await Shell.Current.DisplayAlert("Error en el inicio de sesión", loginResult?.Message, "OK");
      }
    }
    catch (Exception e)
    {
      await Shell.Current.DisplayAlert("Error en el inicio de sesión", e.Message, "OK");
    }
  }
  [RelayCommand]
  private async Task SingleSignOn()
  {
    await Shell.Current.DisplayAlert("Error en contraseña unica", "Tamos trabajando en esto", "OK");
  }

  public void Reset()
  {
    Email = null;
    Password = null;
  }

  public void GetTenantName()
  {
    TenantName = _configs["TENANT_NAME"] ?? "interno";
  }

  // [RelayCommand]
  // private async Task LoginAuth0()
  // {
  //   var siteResult = await authService.ValidateSite();

  //   if (siteResult == null || siteResult.Error)
  //   {
  //     await Application.Current.MainPage.DisplayAlert("Site error", siteResult.Message, "OK");
  //     return;
  //   }

  //   var accessType = siteResult.Data.AccessType;

  //   var loginResult = await authService.LoginAuth0(, accessType);

  //   if (loginResult is { Error: false })
  //   {
  //     await authService.SaveSession(loginResult.Data, );
  //     await Shell.Current.GoToAsync($"///{nameof(EventsPage)}");
  //   }
  //   else
  //   {
  //     await Application.Current.MainPage.DisplayAlert("Login error", loginResult.Message, "OK");
  //   }
  // }

}
