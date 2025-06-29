using CommunityToolkit.Maui.Alerts;
using Microsoft.Extensions.Configuration;
using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ServiPuntosUy_mobile.Services;

public class BranchService(IConfiguration configs, IGeoService geoService) : ApiService(configs), IBranchService
{
  private readonly IGeoService _geoService = geoService;
  public Location UserLocation { get; set; } = new(-34.9011, -56.1645);
  private List<Branch>? _allBranches;

  public ReadOnlyCollection<Branch>? AllBranches
  {
    get
    {
      return _allBranches?.AsReadOnly();
    }
  }

  public Branch? ClosestBranch
  {
    get
    {
      if (UserLocation is not null)
      {
        if (_allBranches is null) return null;
        Branch? closestBranch = null;
        closestBranch = _allBranches
          .OrderBy(branch => Location.CalculateDistance(branch.Latitud, branch.Longitud, UserLocation, DistanceUnits.Kilometers))
          .FirstOrDefault();
        return closestBranch;
      }
      return _allBranches != null ? _allBranches?.First() : null;
    }
  }

  public async Task LoadBranchesAsync()
  {
    try
    {
      ApiResponse<List<Branch>> branches = await GET<List<Branch>>("branch");
      if (branches is { Error: false, Data: not null })
      {
        _allBranches = branches.Data;
      }
      else
      {
        Debug.WriteLine($"Error obteniendo estaciones: {branches.Message}");
        _allBranches = [];
      }
    }
    catch (Exception ex)
    {
      _allBranches = [];
      Debug.WriteLine($"Error obteniendo estaciones: {ex.Message}");
    }
  }

  public Branch? GetBranchById(int id) => _allBranches?.FirstOrDefault(s => s.Id == id);

  public async Task LoadUserLocationAsync()
  {
    try
    {
      var (lat, lng) = await _geoService.GetCurrentLocationAsync();
      UserLocation = new Location(lat, lng);
    }
    catch (Exception ex)
    {
      Toast.Make($"Error obteniendo tu ubicacion, vamos a usar una por defecto: {ex.Message}");
    }
  }
}