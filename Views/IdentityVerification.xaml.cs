using ServiPuntosUy_mobile.ViewModels;

namespace ServiPuntosUy_mobile.Views;

public partial class IdentityVerificationPage : ContentPage
{
  public IdentityVerificationPage(IdentityVerificationViewModel _identityVerificationViewModel)
  {
    InitializeComponent();
    BindingContext = _identityVerificationViewModel;
  }
}