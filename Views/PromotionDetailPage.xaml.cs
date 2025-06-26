using ServiPuntosUy_mobile.ViewModels;

namespace ServiPuntosUy_mobile.Views;

public partial class PromotionDetailPage : ContentPage
{
  public PromotionDetailPage(PromotionDetailViewModel promotionDetailViewModel)
  {
    InitializeComponent();
    BindingContext = promotionDetailViewModel;
  }

  protected override async void OnAppearing()
  {
    base.OnAppearing();
    var viewModel = BindingContext as PromotionDetailViewModel;
    if (viewModel is not null)
    {
      await Task.WhenAll([
        viewModel.LoadBranchesAsync(),
        viewModel.GetUserPoints(),
        viewModel.GetTenantPointsValue(),
      ]);
    }
  }

  protected override void OnDisappearing()
  {
    base.OnDisappearing();
    var viewModel = BindingContext as PromotionDetailViewModel;
    viewModel?.Reset();
  }
}
