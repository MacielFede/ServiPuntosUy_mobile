using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Models.Enums;

namespace ServiPuntos.uy_mobile.ViewModels;

[QueryProperty(nameof(Product), "Product")]
public partial class ProductDetailViewModel : ObservableObject
{
  [ObservableProperty]
  private Product? product;
  [ObservableProperty]
  private int userPoints;

  public async Task GetUserPoints()
  {
    string? userData = await SecureStorage.GetAsync(SecureStorageType.User.ToString());
    if (string.IsNullOrWhiteSpace(userData))
    {
      UserPoints = 0;
      return;
    }
    UserPoints = JsonConvert.DeserializeObject<User>(userData)?.PointBalance ?? 0;
  }

  [RelayCommand]
  private async Task Buy()
  {
    if (Product is not null)
    {
      await Shell.Current.DisplayAlert("Carrito de compras", $"Gracias por comprar {Product.Name}!", "OK");
    }
  }

  [RelayCommand]
  private async Task UsePoints()
  {
    if (Product is not null)
    {
      await Shell.Current.DisplayAlert("Carrito de compras", $"Canjeaste {Product.Name} usando 200(HACERLO DINAMICO) puntos", "OK");
    }
  }
}
