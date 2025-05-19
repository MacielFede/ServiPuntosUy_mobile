using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.ViewModels;

[QueryProperty("TenantUrl", "TenantUrl")]
public partial class SignUpViewModel(IAuthService authService) : ObservableObject
{
  [ObservableProperty] private string? _tenantUrl;

  [ObservableProperty] private string? _name;
  [ObservableProperty] private string? _email;
  [ObservableProperty] private string? _password;

  [RelayCommand]
  private async Task Signup()
  {
    var registerResult = await authService.Signup(TenantUrl, Name, Email, Password);

    if (registerResult is { Error: false })
    {
      await authService.SaveSession(registerResult.Data);
      // await Shell.Current.GoToAsync($"///{nameof(EventsPage)}");
    }
    else
    {
      await Shell.Current.DisplayAlert("Error on signup", registerResult.Message, "OK");
    }
  }
}