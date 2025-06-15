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
#endif

namespace ServiPuntosUy_mobile.Views;

public partial class App : Microsoft.Maui.Controls.Application
{
	private readonly IAuthService _authService;
	private readonly IBranchService _branchService;
	private readonly ITenantService _tenantService;
	private readonly IConfiguration _configs;
	public App(IBranchService branchService, IAuthService authService, ITenantService tenantService, IConfiguration configs)
	{
		InitializeComponent();
		_branchService = branchService;
		_authService = authService;
		_tenantService = tenantService;
		_configs = configs;
		_authService.SessionCreatedSuccessfully += async (s, e) =>
		{
			await Task.WhenAll([
				_authService.LoadUserData(),
				_branchService.LoadBranchesAsync(),
				_branchService.LoadUserLocationAsync(),
				_tenantService.LoadTenantParameters(),
			]);
		};
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	protected override async void OnStart()
	{
		base.OnStart();
		await Task.WhenAll([
		_tenantService.LoadTenantUIAsync(),
		CheckUserSession(),
		]);
		await RequestNotificationPermission();
		ClearNotifications();
		SubscribeToTopic();
		SetupNotificationHandlers();
	}

	private async Task CheckUserSession()
	{
		var sessionData = await _authService.GetSessionData();
		if (sessionData != null)
		{
			_authService.TriggerSessionCreatedEvent();
			await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
		}
	}

	private async void SubscribeToTopic()
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

	private static void SetupNotificationHandlers()
	{
		CrossFirebaseCloudMessaging.Current.NotificationReceived += (s, e) =>
		{
			MainThread.BeginInvokeOnMainThread(async () =>
					{
						var popup = new NotificationPopup(e.Notification.Body, e.Notification.ImageUrl);
						ClearNotifications();
						await Shell.Current.ShowPopupAsync(popup);
					});
		};
		CrossFirebaseCloudMessaging.Current.NotificationTapped += (s, e) =>
		{
			MainThread.BeginInvokeOnMainThread(async () =>
					{
						var popup = new NotificationPopup(e.Notification.Body, e.Notification.ImageUrl);
						ClearNotifications();
						await Shell.Current.ShowPopupAsync(popup);
					});
		};
	}

	public static async Task<bool> RequestNotificationPermission()
	{
		var status = await Permissions.RequestAsync<Permissions.PostNotifications>();
		return status == PermissionStatus.Granted;
	}

	public static void ClearNotifications()
	{
#if ANDROID
		var notificationManager = Android.App.Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
		notificationManager?.CancelAll();
#elif IOS
UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
#endif
	}
}
