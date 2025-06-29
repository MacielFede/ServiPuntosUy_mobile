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
  private bool loadingBranches = true;
  [ObservableProperty]
  private string error = "";
  [ObservableProperty]
  private List<Branch>? branches;
  [ObservableProperty]
  private Branch? selectedBranch;
  partial void OnSelectedBranchChanged(Branch? value)
  {
    FuelPrices = [];
    Error = "";
    LoadingBranches = true;
    _ = LoadFuelPrices(value?.Id);
  }
  public async Task LoadFuelPrices(int? newBranchId)
  {
    try
    {
      await _branchService.LoadUserLocationAsync();
      FuelPrices = new ObservableCollection<FuelPrice>(await _fuelService.GetFuelPrices(newBranchId ?? SelectedBranch?.Id));
      if (Branches is null || _branchService.ClosestBranch is null)
      {
        await _branchService.LoadBranchesAsync();
        Branches = _branchService.AllBranches?.ToList();
        SelectedBranch = _branchService?.ClosestBranch;
      }
      Error = "";
    }
    catch (Exception ex)
    {
      Branches = _branchService.AllBranches?.ToList();
      SelectedBranch ??= _branchService?.ClosestBranch;
      Error = ex.Message;
    }
    LoadingBranches = false;
  }
}
