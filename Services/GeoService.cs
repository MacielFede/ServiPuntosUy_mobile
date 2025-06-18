using ServiPuntosUy_mobile.Services.Interfaces;

namespace ServiPuntosUy_mobile.Services;

public class GeoService : IGeoService
{
  public async Task<bool> RequestLocationPermissionAsync()
  {
    var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

    if (status != PermissionStatus.Granted)
    {
      status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
    }

    return status == PermissionStatus.Granted;
  }

  public async Task<(double Latitude, double Longitude)> GetCurrentLocationAsync()
  {
    if (!await RequestLocationPermissionAsync())
      throw new UnauthorizedAccessException("Location permission denied.");

    var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
    var location = await Geolocation.GetLocationAsync(request) ?? throw new Exception("Unable to get location.");
    return (location.Latitude, location.Longitude);
  }
}

