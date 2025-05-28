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
    if (Name is null || Email is null || Password is null)
    {
      await Shell.Current.DisplayAlert("Error en el registro", "Debes ingresar todos los datos", "OK");
      return;
    }
    try
    {
      var registerResult = await authService.Signup(Name, Email, Password);

      if (registerResult is { Error: false, Data: not null })
      {
        await authService.SaveSession(registerResult.Data);
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
      }
      else
      {
        await Shell.Current.DisplayAlert("Error en el registro", registerResult?.Message, "OK");
      }
    }
    catch (Exception ex)
    {
      await Shell.Current.DisplayAlert("Error en el registro", ex.Message, "OK");
    }
  }
}