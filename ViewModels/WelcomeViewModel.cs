using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.Views;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class WelcomeViewModel(IConfiguration _configs, IAuthService authService) : ObservableObject
{
  private readonly IAuthService _authService = authService;
  [ObservableProperty] private bool _signingWithGoogle = false;
  [ObservableProperty] private string? _tenantName;
  [RelayCommand]
  private async Task GoToLoginPage()
  {
    await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
  }
  [RelayCommand]
  private async Task GoToSignUpPage()
  {
    await Shell.Current.GoToAsync($"{nameof(SignUpPage)}");
  }
  [RelayCommand]
  private async Task LoginWithGoogle()
  {
    try
    {
      SigningWithGoogle = true;
      var loginResult = await _authService.LoginGoogle();
      Debug.WriteLine($"ESTOY {loginResult.Message}");
      Debug.WriteLine($"ESTOY {loginResult.Data}");
      if (loginResult is { Error: false, Data: not null })
      {
        await _authService.SaveSession(loginResult.Data);
        _authService.TriggerSessionCreatedEvent();
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
      }
      else
      {
        await Toast.Make(loginResult.Message, ToastDuration.Short).Show();
      }
      SigningWithGoogle = false;
    }
    catch (Exception e)
    {
      SigningWithGoogle = false;
      await Toast.Make(e.Message, ToastDuration.Short).Show();
    }
  }
  public void GetTenantName()
  {
    TenantName = _configs["TENANT_NAME"] ?? "interno";
  }
}

