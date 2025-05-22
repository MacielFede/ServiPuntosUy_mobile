using ServiPuntos.uy_mobile.ViewModels;
using ServiPuntos.uy_mobile.Models;

namespace ServiPuntos.uy_mobile.Views;

public partial class HomePage : ContentPage
{
  public HomePage(HomeViewModel homeViewModel)
  {
    InitializeComponent();
    BindingContext = homeViewModel;
  }

  protected override void OnAppearing()
  {
    base.OnAppearing();
    if (BindingContext is HomeViewModel homeViewModel)
    {
      _ = homeViewModel.LoadProducts();
    }
  }

  private async void OnProductSelected(object sender, SelectionChangedEventArgs selection)
  {
    if (selection.CurrentSelection.FirstOrDefault() is Product selectedProduct)
    {
      var navParams = new Dictionary<string, object>
    {
        { "Product", selectedProduct }
    };
      await Shell.Current.GoToAsync(nameof(ProductDetailPage), navParams);
    }
  }
}
