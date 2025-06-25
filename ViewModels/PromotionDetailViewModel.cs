using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Models.Enums;
using ServiPuntosUy_mobile.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using ServiPuntosUy_mobile.Views;

namespace ServiPuntosUy_mobile.ViewModels;

[QueryProperty(nameof(Promotion), "Promotion")]
public partial class PromotionDetailViewModel(IConfiguration configuration, IProductsService productsService, IBranchService branchService, ITenantService tenantService, IQrCodeService qrCodeService) : ObservableObject
{
  private readonly IConfiguration _configs = configuration;
  private readonly IBranchService _branchService = branchService;
  private readonly ITenantService _tenantService = tenantService;
  private readonly IProductsService _productService = productsService;
  private readonly IQrCodeService _qrCodeService = qrCodeService;
  public event Action? QrGenerated;
  [ObservableProperty]
  private ImageSource? qrImage;
  [ObservableProperty]
  private Promotion? promotion;

  [ObservableProperty]
  private Branch? selectedBranch;

  [ObservableProperty]
  private List<Branch>? branches;

  [ObservableProperty]
  private bool isStockAvailable;
  [ObservableProperty]
  private List<Product?> products = [];

  [ObservableProperty]
  private int quantity = 1;

  [ObservableProperty]
  private int userPoints;
  public decimal TotalPrice => Promotion is not null ? Promotion.Price * Quantity : 0;
  [ObservableProperty]
  private int tenantPointsValue;
  public int TotalPointsPrice => Promotion is not null && TenantPointsValue > 0 ? ((int)Promotion.Price) / TenantPointsValue * Quantity : 0;
  partial void OnQuantityChanged(int value)
  {
    OnPropertyChanged(nameof(TotalPrice));
    OnPropertyChanged(nameof(TotalPointsPrice));
  }

  partial void OnPromotionChanged(Promotion? value)
  {
    OnPropertyChanged(nameof(TotalPrice));
    OnPropertyChanged(nameof(TotalPointsPrice));
  }
  partial void OnTenantPointsValueChanged(int value)
  {
    OnPropertyChanged(nameof(TotalPointsPrice));
  }

  [RelayCommand]
  public async Task LoadProductsAsync()
  {
    var results = await Task.WhenAll(Promotion?.Products.Select(_productService.GetProductInfo).ToArray() ?? []);

    Products = results
        .Where(productResponse =>
        {
          if (productResponse is { Error: false, Data: not null })
          {
            return true;
          }
          Debug.WriteLine($"No se pudo obtener el producto, {productResponse.Message}");
          return false;
        }).Select(response => response.Data)
        .ToList() ?? [];
  }

  [RelayCommand]
  public async Task LoadBranchesAsync()
  {
    await _branchService.LoadBranchesAsync();
    var eligibleBranches = new List<Branch>();
    while (Promotion is null) { await Task.Delay(1000); }
    var filteredBranches = _branchService.AllBranches?.Where(branch => Promotion?.Branches.Contains(branch.Id) == true).ToList();
    if (filteredBranches != null && Promotion.Products != null)
    {
      var eligibleBranchesTemp = await Task.WhenAll(filteredBranches.Select(async branch =>
      {
        var stockResponses = await Task.WhenAll(Promotion.Products.Select(productId => _productService.GetProductStock(productId, branch.Id)));
        bool allProductsAvailable = stockResponses.All(r => !r.Error && r.Data != null && r.Data.Stock > 0);
        return allProductsAvailable ? branch : null;
      }));
      eligibleBranches = eligibleBranchesTemp?.Where(b => b is not null)?.Select(b => b!)?.ToList() ?? [];
    }

    Branches = eligibleBranches;
    IsStockAvailable = Branches.Count != 0;
    SelectedBranch = Branches?.FirstOrDefault();
    UpdateProductsStockAsync(null);
  }

  public async Task GetTenantPointsValue()
  {
    TenantPointsValue = await _tenantService.GetTenantPointValue();
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

  [RelayCommand]
  private async Task Buy()
  {
    if (Promotion is not null && SelectedBranch is not null)
    {
      try
      {
        var productsForTransaction = Promotion.Products.Select(prod => new ProductForTransaction
        {
          ProductId = prod,
          Quantity = Quantity
        }).ToArray();
        var response = await _productService.PurchaseProduct(productsForTransaction, SelectedBranch.Id);
        if (response is { Error: false, Data: not null })
        {
          await Shell.Current.DisplayAlert("Carrito de compras", $"Gracias por comprar {Promotion.Description}!\nPodes retirarlo en {SelectedBranch?.Address}.\nObtuviste {response.Data.PointsEarned} puntos", "OK");
          _productService.InvokeUserMadePurchaseEvent();
          await Shell.Current.Navigation.PopToRootAsync();
          await Shell.Current.GoToAsync($"//{nameof(TransactionsHistoryPage)}");
        }
        else
        {
          await Toast.Make(response.Message, ToastDuration.Short).Show();
        }
      }
      catch (Exception ex)
      {
        await Toast.Make(ex.Message, ToastDuration.Short).Show();
      }
    }
  }

  [RelayCommand]
  private async Task UsePoints()
  {
    if (Promotion is not null && SelectedBranch is not null)
    {
      // try
      // {
      //   var response = await _productService.CreateProductRedemption(Promotion.Id, SelectedBranch.Id);
      //   if (response is { Error: false, Data: not null })
      //   {
      //     QrImage = _qrCodeService.GenerateQrCode($"{_configs["API_URL"]}redemption/process/{response.Data.Token}");
      //     QrGenerated?.Invoke();
      //   }
      //   else
      //   {
      //     await Toast.Make(response.Message, ToastDuration.Short).Show();
      //   }
      // }
      // catch (Exception ex)
      // {
      //   await Toast.Make(ex.Message, ToastDuration.Short).Show();
      // }
      await Toast.Make("Trabajo en proceso", ToastDuration.Short).Show();
    }
  }
  public void Reset()
  {
    Quantity = 1;
  }

  public void SendPurchaseEvent()
  {
    _productService.InvokeUserMadePurchaseEvent();
  }

  partial void OnSelectedBranchChanged(Branch? value)
  {
    UpdateProductsStockAsync(value);
  }

  private async void UpdateProductsStockAsync(Branch? branch)
  {
    branch ??= SelectedBranch;
    try
    {
      var tasks = Promotion?.Products.Select(productId => _productService.GetProductStock(productId, branch!.Id));
      var responses = await Task.WhenAll(tasks ?? []);
      Products = responses
                    .Where(r => !r.Error && r.Data is not null && r.Data.Stock > 0)
                    .Select(r => r.Data)
                    .ToList();
    }
    catch (Exception) { }
  }
}
