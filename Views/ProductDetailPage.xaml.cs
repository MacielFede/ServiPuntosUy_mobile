using ServiPuntosUy_mobile.ViewModels;

namespace ServiPuntosUy_mobile.Views;

public partial class ProductDetailPage : ContentPage
{
  public ProductDetailPage(ProductDetailViewModel productDetailViewModel)
  {
    InitializeComponent();
    BindingContext = productDetailViewModel;

    productDetailViewModel.QrGenerated += OnQrGenerated;
  }

  protected override async void OnAppearing()
  {
    base.OnAppearing();
    var viewModel = BindingContext as ProductDetailViewModel;
    if (viewModel is not null)
    {
      await Task.WhenAll([
      viewModel.GetUserPoints(),
      viewModel.LoadBranchesAsync(),
      viewModel.GetTenantPointsValue()
      ]);
    }
  }

  protected override void OnDisappearing()
  {
    base.OnDisappearing();
    var viewModel = BindingContext as ProductDetailViewModel;
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
    var viewModel = BindingContext as ProductDetailViewModel;
    viewModel?.SendPurchaseEvent();
    QrOverlay.IsVisible = false;
  }
}
