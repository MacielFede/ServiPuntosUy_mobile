using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Models.Enums;
using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.Views;
using QRCoder;
using SkiaSharp;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;

namespace ServiPuntos.uy_mobile.ViewModels;

[QueryProperty(nameof(Product), "Product")]
public partial class ProductDetailViewModel(IConfiguration configuration, IBranchService branchService, IProductsService productsService, ITenantService tenantService) : ObservableObject
{
  private readonly IConfiguration _configs = configuration;
  private readonly IProductsService _productsService = productsService;
  private readonly IBranchService _branchService = branchService;
  private readonly ITenantService _tenantService = tenantService;
  public event Action? QrGenerated;
  [ObservableProperty]
  private ImageSource? qrImage;
  [ObservableProperty]
  private Product? product;

  [ObservableProperty]
  private Branch? selectedBranch;

  [ObservableProperty]
  private List<Branch>? branches;


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

  [RelayCommand]
  public async Task LoadBranchesAsync()
  {
    await _branchService.LoadBranchesAsync();
    Branches = _branchService.AllBranches?.ToList();
    SelectedBranch = _branchService.ClosestBranch;
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
        var response = await _productsService.PurchaseProduct([productForTransaction], SelectedBranch.Id);
        if (response is { Error: false, Data: not null })
        {
          await Shell.Current.DisplayAlert("Carrito de compras", $"Gracias por comprar {Product.Name}!\nPodes retirarlo en {SelectedBranch?.Address}.\nObtuviste {response.Data.PointsEarned} puntos", "OK");
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
        var response = await _productsService.CreateProductRedemption(Product.Id, SelectedBranch.Id);
        if (response is { Error: false, Data: not null })
        {
          QrImage = QrCodeService.GenerateQrCode($"{_configs["API_URL"]}redemption/process/{response.Data.Token}");
          QrGenerated?.Invoke();
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
}

public static class QrCodeService
{
  public static ImageSource GenerateQrCode(string text)
  {
    using var qrGenerator = new QRCodeGenerator();
    using var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

    int pixelsPerModule = 10;
    int size = qrCodeData.ModuleMatrix.Count * pixelsPerModule;

    using var surface = SKSurface.Create(new SKImageInfo(size, size));
    var canvas = surface.Canvas;
    canvas.Clear(SKColors.White);

    using var paint = new SKPaint { Color = SKColors.Black, IsAntialias = false };

    for (int y = 0; y < qrCodeData.ModuleMatrix.Count; y++)
    {
      for (int x = 0; x < qrCodeData.ModuleMatrix[y].Count; x++)
      {
        if (qrCodeData.ModuleMatrix[y][x])
        {
          canvas.DrawRect(x * pixelsPerModule, y * pixelsPerModule, pixelsPerModule, pixelsPerModule, paint);
        }
      }
    }

    using var image = surface.Snapshot();
    using var data = image.Encode(SKEncodedImageFormat.Png, 100);
    using var stream = new MemoryStream();
    data.SaveTo(stream);
    stream.Position = 0;

    return ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));
  }
}
