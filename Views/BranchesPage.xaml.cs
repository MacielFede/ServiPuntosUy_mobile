using Map = Microsoft.Maui.Controls.Maps.Map;
using ServiPuntosUy_mobile.ViewModels;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;

namespace ServiPuntosUy_mobile.Views;

public partial class BranchesPage : ContentPage
{
  private readonly BranchesViewModel _branchesViewModel;
  public BranchesPage(BranchesViewModel branchesViewModel)
  {
    InitializeComponent();
    BindingContext = branchesViewModel;
    _branchesViewModel = branchesViewModel;
  }

  protected override async void OnAppearing()
  {
    base.OnAppearing();
    await BranchesPage_Loaded();
  }

  private async Task BranchesPage_Loaded()
  {
    var location = await _branchesViewModel.GetUserLocation();
    BranchesMap.MoveToRegion(new MapSpan(location, 0.1, 0.1));
    BranchesMap.Pins.Clear();
    foreach (var branch in _branchesViewModel.GetBranches() ?? [])
    {
      Debug.WriteLine($"ESTOY {branch}");
      Pin pin = new()
      {
        Location = new Location(branch.Latitud, branch.Longitud),
        Label = branch.Address,
        Address = $"Clickea aqui para mas info...\n" +
                    $"Teléfono: {branch.Phone ?? "No disponible"}\n" +
                    $"Hora de apertura: {branch.OpenTime:HH\\:mm}\n" +
                    $"Hora de cierre: {branch.ClosingTime:HH\\:mm}",
        Type = PinType.Place
      };
      pin.InfoWindowClicked += OnInfoWindowClicked;
      BranchesMap.Pins.Add(pin);
    }
  }

  private void OnInfoWindowClicked(object? sender, PinClickedEventArgs e)
  {
    e.HideInfoWindow = true;
    Debug.WriteLine(sender);
    Debug.WriteLine(sender?.GetType());
    Debug.WriteLine(sender?.ToString());
    var pin = sender as Pin;
    if (pin != null)
    {
      DisplayAlert($"Sucursal en\n{pin.Label}", CleanLabel(pin.Address), "OK");
    }
  }

  static string CleanLabel(string rawAddressText) => rawAddressText.Replace("Clickea aqui para mas info...\n", "").Trim();
}
