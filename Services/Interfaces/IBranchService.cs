using System.Collections.ObjectModel;
using ServiPuntos.uy_mobile.Models;

namespace ServiPuntos.uy_mobile.Services.Interfaces;

public interface IBranchService
{
  ReadOnlyCollection<Branch>? AllBranches { get; }
  Location UserLocation { get; set; }

  Branch? ClosestBranch { get; }

  Task LoadBranchesAsync();

  Branch? GetBranchById(int id);
  Task LoadUserLocationAsync();
}