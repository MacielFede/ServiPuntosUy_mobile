using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Views;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.ViewModel;

[QueryProperty("SiteUrl", "SiteUrl")]
public partial class SignupViewModel(IIdentityService identityService) : ObservableObject
{
  [ObservableProperty] private string? _siteUrl;

  [ObservableProperty] private string? _name;
  [ObservableProperty] private string? _email;
  [ObservableProperty] private string? _password;

  [RelayCommand]
  private async Task Signup()
  {
    var registerResult = await identityService.Signup(SiteUrl, Name, Email, Password);

    if (registerResult is { Error: false })
    {
      await identityService.SaveSession(registerResult.Data, SiteUrl);
      // await Shell.Current.GoToAsync($"///{nameof(EventsPage)}");
    }
    else
    {
      await Shell.Current.DisplayAlert("Error on signup", registerResult.Message, "OK");
    }
  }
}