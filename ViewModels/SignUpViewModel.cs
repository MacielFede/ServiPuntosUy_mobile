using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.Views;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class SignUpViewModel(IAuthService authService) : ObservableObject
{
  [ObservableProperty] private string? _name;
  [ObservableProperty] private string? _email;
  [ObservableProperty] private string? _password;

  [RelayCommand]
  private async Task Signup()
  {
    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    return;
    var registerResult = await authService.Signup(Name, Email, Password);

    if (registerResult is { Error: false })
    {
      await authService.SaveSession(registerResult.Data);
    }
    else
    {
      await Shell.Current.DisplayAlert("Error on signup", registerResult.Message, "OK");
    }
  }
}