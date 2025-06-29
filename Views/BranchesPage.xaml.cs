using Map = Microsoft.Maui.Controls.Maps.Map;
using ServiPuntosUy_mobile.ViewModels;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;
using System.Threading.Tasks;

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
    await RefreshMapMarkers();
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

  private async Task RefreshMapMarkers()
  {
    BranchesMap.Pins.Clear();
    foreach (var branch in await _branchesViewModel.GetBranches() ?? [])
    {
      Pin pin = new()
      {
        Location = new Location(branch.Latitud, branch.Longitud),
        Label = branch.Address,
        Address = $"Clickea aqui para mas info...\nTeléfono: {branch.Phone ?? "No disponible"}\nHora de apertura: {branch.OpenTime:HH\\:mm}\nHora de cierre: {branch.ClosingTime:HH\\:mm}",
        Type = PinType.Place
      };
      pin.InfoWindowClicked += OnInfoWindowClicked;
      BranchesMap.Pins.Add(pin);
    }
  }

  private void OnOpenFilterActionClicked(object sender, EventArgs e)
  {
    var button = sender as Button;
    if (button?.Text == "Limpiar filtro")
      _branchesViewModel.FilterOpenTime = null;
    else
      _branchesViewModel.FilterOpenTime = TimeOnly.FromTimeSpan(OpenTimePicker.Time);
    _ = RefreshMapMarkers();
  }

  private void OnCloseFilterActionClicked(object sender, EventArgs e)
  {
    var button = sender as Button;
    if (button?.Text == "Limpiar filtro")
      _branchesViewModel.FilterClosingTime = null;
    else
      _branchesViewModel.FilterClosingTime = TimeOnly.FromTimeSpan(CloseTimePicker.Time);
    _ = RefreshMapMarkers();
  }
}
