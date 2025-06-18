using CommunityToolkit.Maui.Views;
using ServiPuntosUy_mobile.Services.Interfaces;

namespace ServiPuntosUy_mobile.Popups
{
  public partial class NotificationPopup : Popup
  {
    public NotificationPopup(string message, INotificationService notificationService)
    {
      InitializeComponent();

      NotificationText.Text = message;
      _ = Task.Run(async () =>
      {
        await Task.Delay(1000);
        notificationService.ClearNotifications();
      });
    }

    private void Close_Clicked(object sender, EventArgs e)
    {
      Close();
    }
  }
}
