using ServiPuntos.uy_mobile.ViewModels;
using ServiPuntos.uy_mobile.Models;
using System.Diagnostics;
using ServiPuntos.uy_mobile.Converters;

namespace ServiPuntos.uy_mobile.Views;

public partial class HomePage : ContentPage
{
  private CancellationTokenSource? _pollingCancellationTokenSource;
  public HomePage(HomeViewModel homeViewModel)
  {
    InitializeComponent();
    BindingContext = homeViewModel;
  }
  protected override async void OnAppearing()
  {
    base.OnAppearing();
    if (BindingContext is HomeViewModel homeViewModel)
    {
      await homeViewModel.LoadProducts();
      StartPolling();
    }
  }

  protected override void OnDisappearing()
  {
    base.OnDisappearing();
    StopPolling();
  }

  private async void OnProductSelected(object sender, SelectionChangedEventArgs selection)
  {
    if (selection.CurrentSelection.FirstOrDefault() is Product selectedProduct)
    {
      var navParams = new Dictionary<string, object>
    {
        { "Product", selectedProduct }
    };
      await Shell.Current.GoToAsync(nameof(ProductDetailPage), navParams);
    }
  }
  private async void OnAvatarClicked(object sender, EventArgs eventArgs)
  {
    await Shell.Current.GoToAsync(nameof(IdentityVerificationPage));
  }

  private void StartPolling()
  {
    _pollingCancellationTokenSource = new CancellationTokenSource();

    Task.Run(async () =>
    {
      while (!_pollingCancellationTokenSource.IsCancellationRequested)
      {
        try
        {
          if (BindingContext is HomeViewModel homeViewModel && homeViewModel.UserPoints == 0)
          {
            await CurrencyFormatConverter.InitializeCurrencySymbolAsync();
            await homeViewModel.GetUserPoints();
          }
          else
          {
            StopPolling();
          }
          // Wait 5 seconds before the next poll
          await Task.Delay(TimeSpan.FromSeconds(0.5), _pollingCancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
          // Normal case when polling is stopped
        }
        catch (Exception ex)
        {
          Debug.WriteLine($"Error en polling: {ex.Message}");
        }
      }
    }, _pollingCancellationTokenSource.Token);
  }

  private void StopPolling()
  {
    _pollingCancellationTokenSource?.Cancel();
    _pollingCancellationTokenSource?.Dispose();
    _pollingCancellationTokenSource = null;
  }
}
