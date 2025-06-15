using ServiPuntos.uy_mobile.ViewModels;

namespace ServiPuntos.uy_mobile.Views;

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
    }
  }

  private void OnQrGenerated()
  {
    MainThread.BeginInvokeOnMainThread(() =>
    {
      QrOverlay.IsVisible = true;
    });
  }

  private async void CloseQrOverlay_Clicked(object sender, EventArgs e)
  {
    var viewModel = BindingContext as ProductDetailViewModel;
    if (viewModel is not null) await viewModel.GetUserPoints();
    QrOverlay.IsVisible = false;
  }
}
