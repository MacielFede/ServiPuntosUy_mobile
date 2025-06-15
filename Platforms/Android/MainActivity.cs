using System.Diagnostics;
using _Microsoft.Android.Resource.Designer;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.Firebase.CloudMessaging;

namespace ServiPuntosUy_mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
  protected override void OnCreate(Bundle? saveInstanceState)
  {
    base.OnCreate(saveInstanceState);
    if (Intent is not null)
    {
      FirebaseCloudMessagingImplementation.OnNewIntent(Intent);
    }
    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
    {
      var channelId = $"{PackageName}.default";
      var notificationManager = (NotificationManager)GetSystemService(NotificationService)!;
      var channel = new NotificationChannel(channelId, "Default", NotificationImportance.High);
      notificationManager.CreateNotificationChannel(channel);
      FirebaseCloudMessagingImplementation.ChannelId = channelId;
      FirebaseCloudMessagingImplementation.SmallIconRef = ResourceConstant.Drawable.notification_icon_background;
    }
  }

  protected override void OnNewIntent(Intent? intent)
  {
    base.OnNewIntent(intent);
    if (intent != null)
    {
      FirebaseCloudMessagingImplementation.OnNewIntent(intent);
    }
  }
}
