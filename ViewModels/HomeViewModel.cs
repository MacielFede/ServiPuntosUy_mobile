using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Models.Enums;
using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.Views;

namespace ServiPuntos.uy_mobile.ViewModels;

public partial class HomeViewModel(IProductsService productsService, IAuthService authService) : ObservableObject
{
  private readonly IProductsService _productService = productsService;
  private readonly IAuthService _authService = authService;
  public ObservableCollection<Product> FlashOffers { get; } = [];

  public bool HasFlashOffers => FlashOffers.Any();
  [ObservableProperty]
  private int userPoints;

  [ObservableProperty]
  private ObservableCollection<Product> products = [];
  [RelayCommand]
  public async Task Logout()
  {
    await _authService.Logout();
    await Shell.Current.GoToAsync($"//{nameof(WelcomePage)}");
  }

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
}
