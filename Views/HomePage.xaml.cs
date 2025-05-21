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


  private async void OnAvatarClicked(object sender, EventArgs e)
  {
    // Show sidebar with login and signup buttons
    await Shell.Current.GoToAsync(nameof(LoginPage));
  }

  private async void OnProductSelected(object sender, SelectedItemChangedEventArgs selection)
  {
    if (selection.SelectedItem is Product selectedProduct)
    {
      var navParams = new Dictionary<string, object>
    {
        { "Product", selectedProduct }
    };
      await Shell.Current.GoToAsync(nameof(ProductDetailPage), navParams);
    }
  }
}
