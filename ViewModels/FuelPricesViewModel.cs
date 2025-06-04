using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class FuelPricesViewModel(IFuelService fuelService) : ObservableObject
{
  private readonly IFuelService _fuelService = fuelService;
  [ObservableProperty]
  private ObservableCollection<FuelPrice> fuelPrices = [];
  [ObservableProperty]
  private string error = "";

  public async Task LoadFuelPrices()
  {
    try
    {
      FuelPrices = new ObservableCollection<FuelPrice>(await _fuelService.GetFuelPrices());
      Error = "";
    }
    catch (Exception ex)
    {
      Error = ex.Message;
    }
  }
}
