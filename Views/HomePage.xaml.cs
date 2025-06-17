using ServiPuntosUy_mobile.ViewModels;
using ServiPuntosUy_mobile.Models;
using System.Diagnostics;
using ServiPuntosUy_mobile.Converters;

namespace ServiPuntosUy_mobile.Views;

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
      await Task.WhenAll([homeViewModel.LoadProducts(), homeViewModel.LoadPromotions()]);
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
  private async void OnPromotionSelected(object sender, SelectionChangedEventArgs selection)
  {
    if (selection.CurrentSelection.FirstOrDefault() is Promotion selectedPromotion)
    {
      var navParams = new Dictionary<string, object>
    {
        { "Promotion", selectedPromotion }
    };
      await Shell.Current.GoToAsync(nameof(PromotionDetailPage), navParams);
    }
  }
  private async void OnAvatarClicked(object sender, EventArgs eventArgs)
  {
    await Shell.Current.GoToAsync(nameof(IdentityVerificationPage));
  }

  private void StartPolling()
  {
    _pollingCancellationTokenSource = new CancellationTokenSource();
    bool isFirstPoll = true;

    Task.Run(async () =>
    {
      while (!_pollingCancellationTokenSource.IsCancellationRequested)
      {
        try
        {
          if (BindingContext is HomeViewModel homeViewModel)
          {
            // Always run on first poll
            if (isFirstPoll || homeViewModel.UserPoints == 0)
            {
              await CurrencyFormatConverter.InitializeCurrencySymbolAsync();
              await homeViewModel.GetUserPoints();
              isFirstPoll = false;
            }
            else
            {
              StopPolling();
              break;
            }
          }

          await Task.Delay(TimeSpan.FromSeconds(0.5), _pollingCancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
          // Normal when polling is stopped
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
