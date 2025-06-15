using System.Collections.ObjectModel;
using ServiPuntosUy_mobile.Models;

namespace ServiPuntosUy_mobile.Services.Interfaces;

public interface IBranchService
{
  ReadOnlyCollection<Branch>? AllBranches { get; }
  Location UserLocation { get; set; }

  Branch? ClosestBranch { get; }

  Task LoadBranchesAsync();

  Branch? GetBranchById(int id);
  Task LoadUserLocationAsync();
}