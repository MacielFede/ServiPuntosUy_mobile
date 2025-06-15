using CommunityToolkit.Maui.Views;

namespace ServiPuntosUy_mobile.Popups
{
  public partial class NotificationPopup : Popup
  {
    public NotificationPopup(string message, string imageUrl)
    {
      InitializeComponent();

      NotificationText.Text = message;

      if (!string.IsNullOrEmpty(imageUrl))
      {
        NotificationImage.Source = ImageSource.FromUri(new Uri(imageUrl));
      }
      else
      {
        NotificationImage.IsVisible = false;
      }
    }

    private void Close_Clicked(object sender, EventArgs e)
    {
      Close();
    }
  }
}
