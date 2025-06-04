using ServiPuntos.uy_mobile.ViewModels;

namespace ServiPuntos.uy_mobile.Views
{
  public partial class FuelPricesPage : ContentPage
  {
    public FuelPricesPage(FuelPricesViewModel fuelPricesViewModel)
    {
      InitializeComponent();
      BindingContext = fuelPricesViewModel;
    }

    protected override void OnAppearing()
    {
      base.OnAppearing();
      (BindingContext as FuelPricesViewModel)?.LoadFuelPrices();
    }
  }
}
