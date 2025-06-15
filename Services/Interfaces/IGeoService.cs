
namespace ServiPuntosUy_mobile.Services.Interfaces
{
  public interface IGeoService
  {
    Task<bool> RequestLocationPermissionAsync();
    Task<(double Latitude, double Longitude)> GetCurrentLocationAsync();
  }
}
