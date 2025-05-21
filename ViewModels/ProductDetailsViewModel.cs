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
  private static void Buy()
  {
    // Add your buy logic here
  }
}