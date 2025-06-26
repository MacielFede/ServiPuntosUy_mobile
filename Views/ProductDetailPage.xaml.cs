using ServiPuntosUy_mobile.ViewModels;

namespace ServiPuntosUy_mobile.Views;

public partial class ProductDetailPage : ContentPage
{
  public ProductDetailPage(ProductDetailViewModel productDetailViewModel)
  {
    InitializeComponent();
    BindingContext = productDetailViewModel;
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
    viewModel?.Reset();
  }
}
