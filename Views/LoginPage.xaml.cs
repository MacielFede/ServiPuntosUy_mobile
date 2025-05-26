using ServiPuntos.uy_mobile.ViewModels;

namespace ServiPuntos.uy_mobile.Views;

public partial class LoginPage : ContentPage
{
  public LoginPage(LoginViewModel loginViewModel)
  {
    InitializeComponent();
    BindingContext = loginViewModel;
  }
  protected override void OnDisappearing()
  {
    base.OnDisappearing();
    (BindingContext as LoginViewModel)?.Reset();
  }
}