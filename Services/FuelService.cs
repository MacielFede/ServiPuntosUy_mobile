using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ServiPuntosUy_mobile.Enums;
using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Services.Interfaces;

namespace ServiPuntosUy_mobile.Services;

public class FuelService(IConfiguration configs, IBranchService branchService) : ApiService(configs), IFuelService
{
  private readonly IBranchService _branchService = branchService;
  public async Task<FuelPrice[]> GetFuelPrices(int? _branchId)
  {
    int? branchId = _branchId ?? _branchService?.ClosestBranch?.Id;
    if (branchId is null && _branchService is not null)
    {
      await _branchService.LoadBranchesAsync();
    }
    ApiResponse<FuelPrice> fetchedFuelprice;
    List<FuelPrice> activePrices = [];
    foreach (var type in Enum.GetValues<FuelType>())
    {
      try
      {
        fetchedFuelprice = await GET<FuelPrice>($"fuel/{branchId ?? _branchService?.ClosestBranch?.Id}/price/{type}");
        if (fetchedFuelprice is { Error: false, Data: not null }) activePrices.Add(fetchedFuelprice.Data);
        else throw new Exception(fetchedFuelprice.Message);
      }
      catch (Exception ex)
      {
        Debug.WriteLine($"No pude obtener el precio del tipo de combustible {type}: {ex.Message}");
      }
    }
    return activePrices.Count == 0 ? throw new Exception("No pudimos obtener los combustible para la estación seleccionada, prueba con otra porfavor.") : [.. activePrices];
  }
}
