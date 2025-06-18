using ServiPuntosUy_mobile.ViewModels;

namespace ServiPuntosUy_mobile.Views;

public partial class SignUpPage : ContentPage
{
  public SignUpPage(SignUpViewModel signupViewModel)
  {
    InitializeComponent();
    BindingContext = signupViewModel;
  }
}