using ServiPuntosUy_mobile.ViewModels;

namespace ServiPuntosUy_mobile.Views;

public partial class PromotionDetailPage : ContentPage
{
  public PromotionDetailPage(PromotionDetailViewModel promotionDetailViewModel)
  {
    InitializeComponent();
    BindingContext = promotionDetailViewModel;

    promotionDetailViewModel.QrGenerated += OnQrGenerated;
  }

  protected override async void OnAppearing()
  {
    base.OnAppearing();
    var viewModel = BindingContext as PromotionDetailViewModel;
    if (viewModel is not null)
    {
      await Task.WhenAll([
        viewModel.GetUserPoints(),
        viewModel.LoadBranchesAsync(),
        viewModel.GetTenantPointsValue(),
        viewModel.LoadProductsAsync()
      ]);
    }
  }

  protected override void OnDisappearing()
  {
    base.OnDisappearing();
    var viewModel = BindingContext as PromotionDetailViewModel;
    if (viewModel is not null)
    {
      viewModel.QrGenerated -= OnQrGenerated;
      viewModel.Reset();
    }
  }

  private void OnQrGenerated()
  {
    MainThread.BeginInvokeOnMainThread(() =>
    {
      QrOverlay.IsVisible = true;
    });
  }

  private void CloseQrOverlay_Clicked(object sender, EventArgs e)
  {
    var viewModel = BindingContext as PromotionDetailViewModel;
    viewModel?.SendPurchaseEvent();
    QrOverlay.IsVisible = false;
  }
}
