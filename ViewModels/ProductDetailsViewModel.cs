using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiPuntos.uy_mobile.Models;

namespace ServiPuntos.uy_mobile.ViewModels;

[QueryProperty(nameof(Product), "Product")]
public partial class ProductDetailViewModel : ObservableObject
{
  [ObservableProperty]
  private Product? product;

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