using System.Diagnostics;
using ServiPuntosUy_mobile.Services.Interfaces;
using ServiPuntosUy_mobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ServiPuntosUy_mobile.ViewModels;

public class BranchesViewModel(IBranchService branchService) : ObservableObject
{
  private readonly IBranchService _branchService = branchService;

  public async Task<Location> GetUserLocation()
  {
    try
    {
      await _branchService.LoadUserLocationAsync();
      return _branchService.UserLocation;
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"Error obteniendo la ubicación del usuario: {ex.Message}");
      return new(-34.9011, -56.1645);
    }
  }

  public Branch[] GetBranches() => _branchService.AllBranches != null ? [.. _branchService.AllBranches] : [];
}