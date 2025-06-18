using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Services.Interfaces;

namespace ServiPuntosUy_mobile.ViewModels;

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
      await _branchService.LoadUserLocationAsync();
      var address = _branchService?.ClosestBranch?.Address;
      FuelPrices = new ObservableCollection<FuelPrice>(await _fuelService.GetFuelPrices());
      if (address is null && _branchService is not null)
      {
        await _branchService.LoadBranchesAsync();
      }
      NearestBranchAddress = address ?? _branchService?.ClosestBranch?.Address ?? ""; ;
      Error = "";
    }
    catch (Exception ex)
    {
      Error = ex.Message;
    }
  }
}
