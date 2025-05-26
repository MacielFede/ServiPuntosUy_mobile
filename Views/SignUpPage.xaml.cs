using ServiPuntos.uy_mobile.ViewModels;

namespace ServiPuntos.uy_mobile.Views;

public partial class SignUpPage : ContentPage
{
  public SignUpPage(SignUpViewModel signupViewModel)
  {
    InitializeComponent();
    BindingContext = signupViewModel;
  }
}