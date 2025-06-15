using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using ServiPuntosUy_mobile.Services.Interfaces;
using ServiPuntosUy_mobile.Views;

namespace ServiPuntosUy_mobile.ViewModels;

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
        await Toast.Make("Debes ingresar email y contraseña", ToastDuration.Short).Show();
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
        await Toast.Make(loginResult.Message, ToastDuration.Short).Show();
      }
    }
    catch (Exception e)
    {
      await Toast.Make(e.Message, ToastDuration.Short).Show();
    }
  }
  [RelayCommand]
  private static async Task SingleSignOn()
  {
    await Toast.Make("Tamos trabajando en esto", ToastDuration.Short).Show();
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
}
