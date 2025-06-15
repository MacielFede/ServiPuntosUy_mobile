using CommunityToolkit.Maui.Views;

namespace ServiPuntosUy_mobile.Popups;

public partial class NotificationPopup : Popup
{
  public NotificationPopup(string message, string imageUrl)
  {
    InitializeComponent();

    PopupText.Text = message;

    if (!string.IsNullOrEmpty(imageUrl))
      PopupImage.Source = ImageSource.FromUri(new Uri(imageUrl));
    else
      PopupImage.IsVisible = false;
  }

  private void Close_Clicked(object sender, EventArgs e)
  {
    Close(); // Closes the popup
  }
}
