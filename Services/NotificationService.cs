using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Configuration;
using Plugin.Firebase.CloudMessaging;
using ServiPuntosUy_mobile.Popups;
using ServiPuntosUy_mobile.Services.Interfaces;
#if ANDROID
using Android.App;
using Android.Content;
#elif IOS
using UIKit;
using UserNotifications;
#endif

namespace ServiPuntosUy_mobile.Services;

public class NotificationService(IConfiguration configs) : INotificationService
{
  private readonly IConfiguration _configs = configs;

  public async Task InitializeAsync()
  {
    await Permissions.RequestAsync<Permissions.PostNotifications>();
    await SubscribeToTopic();

    CrossFirebaseCloudMessaging.Current.NotificationReceived += (s, e) =>
    {
      MainThread.BeginInvokeOnMainThread(async () =>
      {
        var notificationText = e.Notification.Data?.Where(item => item.Key == "Description").FirstOrDefault().Value;
        if (notificationText is not null)
          await Shell.Current.ShowPopupAsync(new NotificationPopup(notificationText, this));
      });
    };

    CrossFirebaseCloudMessaging.Current.NotificationTapped += (s, e) =>
    {
      MainThread.BeginInvokeOnMainThread(async () =>
      {
        var notificationText = e.Notification.Data?.Where(item => item.Key == "Description").FirstOrDefault().Value;
        if (notificationText is not null)
          await Shell.Current.ShowPopupAsync(new NotificationPopup(notificationText, this));
      });
    };

    CrossFirebaseCloudMessaging.Current.TokenChanged += async (s, e) =>
    {
      await SubscribeToTopic();
    };
  }

  public void ClearNotifications()
  {
#if ANDROID
        var notificationManager = Android.App.Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
        notificationManager?.CancelAll();
#elif IOS
    UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
    UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
#endif
  }

  private async Task SubscribeToTopic()
  {
    try
    {
      await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
      await CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync($"ofertas_{_configs["TENANT_NAME"]!}");
      Debug.WriteLine($"Subscribed to topic: ofertas_{_configs["TENANT_NAME"]!}");
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"Failed to subscribe to topic: {ex.Message}");
    }
  }
}
