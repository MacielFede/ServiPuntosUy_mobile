using ServiPuntos.uy_mobile.ViewModel;

namespace ServiPuntos.uy_mobile.Views;

public partial class SignupPage : ContentPage
{
  public SignupPage(SignupViewModel signupViewModel)
  {
    InitializeComponent();
    BindingContext = signupViewModel;
  }
}