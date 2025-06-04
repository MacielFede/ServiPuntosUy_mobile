using ServiPuntos.uy_mobile.Enums;

namespace ServiPuntos.uy_mobile.Models;

public class FuelPrice(FuelType fuelType, decimal price)
{
  public decimal Price { get; set; } = price;
  public FuelType FuelType { get; set; } = fuelType;
}