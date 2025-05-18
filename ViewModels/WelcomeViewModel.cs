using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiPuntos.uy_mobile.Views;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class WelcomeViewModel : ObservableObject
{

  [RelayCommand]
  private async Task GoToLoginPage()
  {
    await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
  }
}

