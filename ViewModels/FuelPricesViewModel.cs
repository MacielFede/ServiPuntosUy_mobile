using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class FuelPricesViewModel(IFuelService fuelService, IBranchService branchService) : ObservableObject
{
  private readonly IFuelService _fuelService = fuelService;
  private readonly IBranchService _branchService = branchService;
  [ObservableProperty]
  private ObservableCollection<FuelPrice> fuelPrices = [];
  [ObservableProperty]
  private string error = "";
  [ObservableProperty]
  private string nearestBranchAddress = "";

  public async Task LoadFuelPrices()
  {
    try
    {
      FuelPrices = new ObservableCollection<FuelPrice>(await _fuelService.GetFuelPrices());
      var address = _branchService?.ClosestBranch?.Address;
      if (address is null && _branchService is not null)
      {
        await _branchService.LoadBranchesAsync();
        throw new Exception("Estamos trabajando para obtener el precio de los combustibles mas cercanos a ti.");
      }

      Error = "";
    }
    catch (Exception ex)
    {
      Error = ex.Message;
    }
  }
}
