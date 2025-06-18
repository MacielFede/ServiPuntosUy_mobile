using ServiPuntosUy_mobile.Models;

namespace ServiPuntosUy_mobile.Services.Interfaces;

public interface IFuelService
{
  public Task<FuelPrice[]> GetFuelPrices();
}

