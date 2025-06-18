using ServiPuntosUy_mobile.ViewModels;

namespace ServiPuntosUy_mobile.Views
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
