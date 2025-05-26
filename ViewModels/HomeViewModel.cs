using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class HomeViewModel(IProductsService productsService, IAuthService authService) : ObservableObject
{
  private readonly IProductsService _productService = productsService;
  private readonly IAuthService _authService = authService;
  [ObservableProperty]
  private ObservableCollection<Product> products = [];

  public async Task LoadProducts()
  {
    var productsList = await _productService.GetProductsAsync();
    Products = new ObservableCollection<Product>(productsList);
  }

  [RelayCommand]
  public async Task Logout()
  {
    await _authService.Logout();
    await Shell.Current.GoToAsync("//WelcomePage");
  }
}
