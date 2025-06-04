using ServiPuntos.uy_mobile.Models;

namespace ServiPuntos.uy_mobile.Services.Interfaces;

public interface IFuelService
{
  public Task<FuelPrice[]> GetFuelPrices();
}

