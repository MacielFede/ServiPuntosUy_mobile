using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.Views;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class HomeViewModel(IProductsService productsService, IAuthService authService) : ObservableObject
{
  private readonly IProductsService _productService = productsService;
  private readonly IAuthService _authService = authService;
  [ObservableProperty]
  private ObservableCollection<Product> products = [];
  [ObservableProperty]
  private int gasPrice;

  public async Task LoadProducts()
  {
    try
    {
      var productsList = await _productService.GetProductsAsync();
      if (productsList is { Error: false, Data: not null })
        Products = new ObservableCollection<Product>(productsList.Data);
      else
        await Shell.Current.DisplayAlert("Error obteniendo productos", productsList.Message, "OK");
    }
    catch (Exception ex)
    {
      await Shell.Current.DisplayAlert("Error obteniendo productos", ex.Message, "OK");
    }
  }

  public async Task GetGasPrice()
  {
    try
    {
      var gasPriceResponse = _productService.GetGasPrice();
      if (gasPriceResponse is { Error: false })
        GasPrice = gasPriceResponse.Data;
      else
        await Shell.Current.DisplayAlert("Error obteniendo productos", gasPriceResponse.Message, "OK");
    }
    catch (Exception ex)
    {
      await Shell.Current.DisplayAlert("Error obteniendo precio del combustible", ex.Message, "OK");
    }
  }

  [RelayCommand]
  public async Task Logout()
  {
    await _authService.Logout();
    await Shell.Current.GoToAsync($"//{nameof(WelcomePage)}");
  }
}
