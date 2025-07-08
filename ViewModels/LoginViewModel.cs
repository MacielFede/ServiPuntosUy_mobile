using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using ServiPuntosUy_mobile.Services.Interfaces;
using ServiPuntosUy_mobile.Views;

namespace ServiPuntosUy_mobile.ViewModels;

[QueryProperty(nameof(Token), "Token")]
public partial class LoginViewModel(IAuthService authService, IConfiguration _configs) : ObservableObject
{
  [ObservableProperty] private string? _email;
  [ObservableProperty] private string? _TenantName;
  [ObservableProperty] private string? _password;
  [ObservableProperty] private string? token;
  [ObservableProperty] private bool usingMagicLink = false;

  partial void OnTokenChanged(string? value)
  {
    _ = OnTokenChangedAsync(value);
  }

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


  private async Task OnTokenChangedAsync(string? value)
  {
    if (!string.IsNullOrEmpty(value))
    {
      var response = await authService.ValidateMagicLink(value);
      if (response is { Error: false, Data: not null })
      {
        await authService.SaveSession(response.Data);
        authService.TriggerSessionCreatedEvent();
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        UsingMagicLink = false;
      }
      else
      {
        await Toast.Make("Ocurrio un error validando tu inicio de sesion", ToastDuration.Long).Show();
        UsingMagicLink = false;
      }
    }
    else
    {
      await Toast.Make("Ocurrio un error validando tu inicio de sesion", ToastDuration.Long).Show();
      UsingMagicLink = false;
    }
  }

  [RelayCommand]
  private async Task SingleSignOn()
  {
    if (string.IsNullOrWhiteSpace(Email))
    {
      await Toast.Make("Debes ingresar tu correo electronico para crear el magic link", ToastDuration.Short).Show();
      return;
    }
    UsingMagicLink = true;
    var loginResult = await authService.CreateMagicLink(Email);
    if (loginResult is { Error: false, Data: not null })
    {
      await Shell.Current.DisplayAlert("Inicio de sesion", "Recibiras un email con un link para continuar con el inicio de seison", "Ok");
    }
    else
    {
      await Toast.Make(loginResult.Message, ToastDuration.Short).Show();
    }
    UsingMagicLink = false;
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
