using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Models.Enums;
using ServiPuntosUy_mobile.Services.Interfaces;
using ServiPuntosUy_mobile.Views;

namespace ServiPuntosUy_mobile.ViewModels;

public partial class HomeViewModel(IProductsService productsService, IAuthService authService) : ObservableObject
{
  private readonly IProductsService _productService = productsService;
  private readonly IAuthService _authService = authService;
  [ObservableProperty]
  private ObservableCollection<Promotion> flashOffers = [];
  partial void OnFlashOffersChanged(ObservableCollection<Promotion> value)
  {
    OnPropertyChanged(nameof(HasFlashOffers));
  }

  public bool HasFlashOffers => User?.IsVerified is true && FlashOffers.Any();
  [ObservableProperty]
  private User? user;
  partial void OnUserChanged(User? value)
  {
    if (value is not null && value.IsVerified is true && FlashOffers.Count == 0 || Products.Count == 0)
    {
      _ = LoadPromotions();
      _ = LoadProducts();
    }
  }

  [ObservableProperty]
  private ObservableCollection<Product> products = [];

  [RelayCommand]
  public async Task Logout()
  {
    await _authService.Logout();
    await Shell.Current.GoToAsync($"//{nameof(WelcomePage)}");
  }

  public async Task LoadPromotions()
  {
    if (User?.IsVerified is null or false) return;
    try
    {
      var response = await _productService.GetPromotionsAsync();
      if (response is { Error: false, Data: not null })
        FlashOffers = response.Data.ToObservableCollection();
      else
        await Toast.Make($"{response.Message}", ToastDuration.Short).Show();
    }
    catch (Exception ex)
    {
      await Toast.Make($"{ex.Message}", ToastDuration.Short).Show();
    }
  }
  public async Task LoadProducts()
  {
    try
    {
      var response = await _productService.GetProductsAsync();
      if (response is { Error: false, Data: not null })
        Products = response.Data.Where(prod => !prod.AgeRestricted || User?.IsVerified is true).ToObservableCollection();
      else
        await Toast.Make($"{response.Message}", ToastDuration.Short).Show();
    }
    catch (Exception ex)
    {
      await Toast.Make($"{ex.Message}", ToastDuration.Short).Show();
    }
  }

  public async Task GetUser()
  {
    await _authService.LoadUserData();
    string? userData = await SecureStorage.GetAsync(SecureStorageType.User.ToString());
    if (string.IsNullOrWhiteSpace(userData))
    {
      await Logout();
      return;
    }
    var savedUser = JsonConvert.DeserializeObject<User>(userData);
    if (savedUser is null)
    {
      await Logout();
      return;
    }
    User = savedUser;
  }
}
