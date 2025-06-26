using CommunityToolkit.Maui.Views;
using ServiPuntosUy_mobile.ViewModels.Interfaces;

namespace ServiPuntosUy_mobile.Popups
{
  public partial class QrPopup : Popup
  {
    private readonly IPurchaseViewModel viewModel;
    public QrPopup(ImageSource image, IPurchaseViewModel vm)
    {
      InitializeComponent();
      QrImage.Source = image;
      viewModel = vm;
    }

    private void Close_Clicked(object sender, EventArgs e)
    {
      viewModel.SendPurchaseEvent();
      Close();
    }
  }
}
