// using CommunityToolkit.Mvvm.ComponentModel;
// using CommunityToolkit.Mvvm.Input;
// using CommunityToolkit.Mvvm.Messaging;
// using TuPencaUy.Models;
// using TuPencaUy.Services.Interfaces;
// using TuPencaUy.Views;

// namespace TuPencaUy.ViewModel;

// [QueryProperty("SiteUrl", "SiteUrl")]
// public partial class LoginViewModel(IIdentityService identityService) : ObservableObject
// {
//     [ObservableProperty] private string? _siteUrl;

//     [ObservableProperty] private string? _email;
//     [ObservableProperty] private string? _password;

//     [RelayCommand]
//     private async Task Login()
//     {
//         var siteResult = await identityService.ValidateSite(SiteUrl);

//         if (siteResult == null || siteResult.Error)
//         {
//             await Application.Current.MainPage.DisplayAlert("Site error", siteResult.Message, "OK");
//             return;
//         }

//         var accessType = siteResult.Data.AccessType;

//         var loginResult = await identityService.Login(SiteUrl, Email, Password, accessType);

//         if (loginResult is { Error: false })
//         {
//             await identityService.SaveSession(loginResult.Data, SiteUrl);
//             await Shell.Current.GoToAsync($"///{nameof(EventsPage)}");
//         }
//         else
//         {
//             await Application.Current.MainPage.DisplayAlert("Login error", loginResult.Message, "OK");
//         }
//     }

//     [RelayCommand]
//     private async Task LoginAuth0()
//     {
//         var siteResult = await identityService.ValidateSite(SiteUrl);

//         if (siteResult == null || siteResult.Error)
//         {
//             await Application.Current.MainPage.DisplayAlert("Site error", siteResult.Message, "OK");
//             return;
//         }

//         var accessType = siteResult.Data.AccessType;

//         var loginResult = await identityService.LoginAuth0(SiteUrl, accessType);

//         if (loginResult is { Error: false })
//         {
//             await identityService.SaveSession(loginResult.Data, SiteUrl);
//             await Shell.Current.GoToAsync($"///{nameof(EventsPage)}");
//         }
//         else
//         {
//             await Application.Current.MainPage.DisplayAlert("Login error", loginResult.Message, "OK");
//         }
//     }

//     [RelayCommand]
//     private async Task Register()
//     {
//         await Shell.Current.GoToAsync($"{nameof(SignupPage)}?SiteUrl={SiteUrl}");
//     }
// }