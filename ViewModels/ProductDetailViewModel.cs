using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Models.Enums;
using ServiPuntosUy_mobile.Services.Interfaces;
using ServiPuntosUy_mobile.ViewModels.Interfaces;
using Microsoft.Extensions.Configuration;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using ServiPuntosUy_mobile.Popups;
using ServiPuntosUy_mobile.Views;

namespace ServiPuntosUy_mobile.ViewModels;

[QueryProperty(nameof(Product), "Product")]
public partial class ProductDetailViewModel(IConfiguration configuration, IBranchService branchService, IProductsService productsService, ITenantService tenantService, IQrCodeService qrCodeService) : ObservableObject, IPurchaseViewModel
{
  private readonly IConfiguration _configs = configuration;
  private readonly IProductsService _productsService = productsService;
  private readonly IBranchService _branchService = branchService;
  private readonly ITenantService _tenantService = tenantService;
  private readonly IQrCodeService _qrCodeService = qrCodeService;
  [ObservableProperty]
  private Product? product;

  [ObservableProperty]
  private Branch? selectedBranch;

  [ObservableProperty]
  private List<Branch>? branches;

  [ObservableProperty]
  private bool isStockAvailable;

  [ObservableProperty]
  private int quantity = 1;

  [ObservableProperty]
  private int userPoints;

  public decimal TotalPrice => Product is not null ? Product.Price * Quantity : 0;

  [ObservableProperty]
  private int tenantPointsValue;

  public int TotalPointsPrice => Product is not null && TenantPointsValue > 0 ? ((int)Product.Price) / TenantPointsValue * Quantity : 0;

  partial void OnQuantityChanged(int value)
  {
    OnPropertyChanged(nameof(TotalPrice));
    OnPropertyChanged(nameof(TotalPointsPrice));
  }

  partial void OnProductChanged(Product? value)
  {
    OnPropertyChanged(nameof(TotalPrice));
    OnPropertyChanged(nameof(TotalPointsPrice));
  }

  partial void OnTenantPointsValueChanged(int value)
  {
    OnPropertyChanged(nameof(TotalPointsPrice));
  }

  partial void OnSelectedBranchChanged(Branch? value)
  {
    if (value?.Id != SelectedBranch?.Id)
      UpdateProductStockAsync(value);
  }

  private async void UpdateProductStockAsync(Branch? branch)
  {
    if (Product is not null)
    {
      branch ??= SelectedBranch;
      try
      {
        var response = await _productsService.GetProductStock(Product.Id, branch!.Id);
        if (!response.Error && response.Data is not null)
        {
          Product = response.Data;
        }
      }
      catch (Exception) { }
    }
  }

  [RelayCommand]
  public async Task LoadBranchesAsync()
  {
    await _branchService.LoadBranchesAsync();
    var eligibleBranches = new List<Branch>();
    while (Product is null) { await Task.Delay(1000); }
    var filteredBranches = _branchService.AllBranches?.ToList();
    if (filteredBranches != null)
    {
      var eligibleBranchesTemp = await Task.WhenAll(filteredBranches.Select(async branch =>
      {
        var response = await _productsService.GetProductStock(Product.Id, branch.Id);
        return (!response.Error && response.Data != null && response.Data.Stock > 0) ? branch : null;
      }));
      eligibleBranches = eligibleBranchesTemp.Where(b => b is not null).Select(b => b!).ToList();
    }
    Branches = eligibleBranches;
    IsStockAvailable = Branches.Count != 0;
    SelectedBranch = Branches?.FirstOrDefault();
    UpdateProductStockAsync(null);
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
    if (Product is not null && SelectedBranch is not null)
    {
      try
      {
        var productForTransaction = new ProductForTransaction
        {
          ProductId = Product.Id,
          Quantity = Quantity
        };
        var response = await _productsService.PurchaseProduct(new[] { productForTransaction }, SelectedBranch.Id);
        if (response is { Error: false, Data: not null })
        {
          await Shell.Current.DisplayAlert("Carrito de compras",
              $"Gracias por comprar {Product.Name}!\nPodes retirarlo en {SelectedBranch?.Address}.\nObtuviste {response.Data.PointsEarned} puntos", "OK");
          _productsService.InvokeUserMadePurchaseEvent();
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
    if (Product is not null && SelectedBranch is not null)
    {
      try
      {
        var response = await _productsService.CreateProductRedemption(new[] { new ProductForTransaction { ProductId = Product.Id, Quantity = Quantity } }, SelectedBranch.Id);
        if (response is { Error: false, Data: not null })
        {
          await Shell.Current.ShowPopupAsync(new QrPopup(_qrCodeService.GenerateQrCode($"{_configs["API_URL"]}redemption/process/{response.Data.Token}"), this));
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

  public void Reset()
  {
    Quantity = 1;
  }

  public void SendPurchaseEvent()
  {
    _productsService.InvokeUserMadePurchaseEvent();
  }
}
