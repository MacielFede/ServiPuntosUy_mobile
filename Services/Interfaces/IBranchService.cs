using System.Collections.ObjectModel;
using ServiPuntos.uy_mobile.Models;

namespace ServiPuntos.uy_mobile.Services.Interfaces;

public interface IBranchService
{
  ReadOnlyCollection<Branch>? AllLocations { get; }

  Branch? ClosestLocation { get; }

  event EventHandler LocationDataLoaded;

  Task LoadLocationsAsync();

  Branch? GetLocationById(int id);
}