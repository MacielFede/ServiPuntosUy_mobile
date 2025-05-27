using ServiPuntos.uy_mobile.ViewModels;

namespace ServiPuntos.uy_mobile.Views;

public partial class IdentityVerificationPage : ContentPage
{
  public IdentityVerificationPage(IdentityVerificationViewModel _identityVerificationViewModel)
  {
    InitializeComponent();
    BindingContext = _identityVerificationViewModel;
  }
  protected override void OnDisappearing()
  {
    base.OnDisappearing();
    (BindingContext as IdentityVerificationViewModel)?.Reset();
  }
}