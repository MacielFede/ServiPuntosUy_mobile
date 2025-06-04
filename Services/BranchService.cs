using AndroidX.ConstraintLayout.Core.Widgets.Analyzer;
using Microsoft.Extensions.Configuration;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ServiPuntos.uy_mobile.Services;

public class BranchService(IConfiguration configs) : ApiService(configs), IBranchService
{
  private Location? UserLocation { get; set; }
  private List<Branch>? _allLocations;
  private Branch? _closestLocation;
  public event EventHandler? LocationDataLoaded;

  public ReadOnlyCollection<Branch>? AllLocations
  {
    get
    {
      return _allLocations?.AsReadOnly();
    }
  }

  public Branch? ClosestLocation
  {
    get
    {
      if (UserLocation is not null)
      {
        if (_allLocations is null) return null;
        Branch? closestBranch = null;
        double distanceToClosestBranch = double.PositiveInfinity;
        foreach (Branch location in _allLocations)
        {
          if (closestBranch is null)
          {
            closestBranch = location;
            distanceToClosestBranch = Location.CalculateDistance(location.Latitud, location.Longitud, UserLocation, DistanceUnits.Kilometers);
          }
          else
          {
            var distanceToBranch = Location.CalculateDistance(location.Latitud, location.Longitud, UserLocation, DistanceUnits.Kilometers);
            if (distanceToBranch < distanceToClosestBranch)
            {
              closestBranch = location;
              distanceToClosestBranch = distanceToBranch;
            }
          }
        }
        _closestLocation = closestBranch;
        return closestBranch;
      }
      _closestLocation = _allLocations?.First();
      return _allLocations?.First();
    }
  }

  public async Task LoadLocationsAsync()
  {
    if (_allLocations?.Count > 0)
    {
      return;
    }
    try
    {
      ApiResponse<List<Branch>> branches = await GET<List<Branch>>("branch");
      if (branches is { Error: false, Data: not null })
      {
        _allLocations = branches.Data;
        Debug.WriteLine($"ESTOY lleno {branches}");
        LocationDataLoaded?.Invoke(this, EventArgs.Empty);
      }
      else
      {
        _allLocations = [];
        Debug.WriteLine($"ESTOY vacio {branches.Message}");
      }
    }
    catch (Exception ex)
    {
      _allLocations = [];
      Debug.WriteLine($"ESTOY Error: {ex.Message}");
    }
  }

  public Branch? GetLocationById(int id) => _allLocations?.FirstOrDefault(s => s.Id == id);
}