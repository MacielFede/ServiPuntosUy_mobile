using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using ServiPuntos.uy_mobile.Views;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class WelcomeViewModel(IConfiguration _configs) : ObservableObject
{
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
  public void GetTenantName()
  {
    TenantName = _configs["TENANT_NAME"] ?? "interno";
  }
}

