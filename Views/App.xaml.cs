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

namespace ServiPuntosUy_mobile.Views;

public partial class App : Microsoft.Maui.Controls.Application
{
	private readonly IAuthService _authService;
	private readonly ITenantService _tenantService;
	private readonly IConfiguration _configs;
	public App(IApiService apiService, IProductsService productsService, IBranchService branchService, IAuthService authService, ITenantService tenantService, IConfiguration configs)
	{
		InitializeComponent();
		_authService = authService;
		_tenantService = tenantService;
		_configs = configs;
		apiService.UserUnauthorized += async (s, e) =>
		{
			await _authService.Logout();
			if (Shell.Current is not null) await Shell.Current.GoToAsync($"//{nameof(WelcomePage)}");
		};
		_authService.SessionCreatedSuccessfully += async (s, e) =>
		{
			await Task.WhenAll([
				_authService.LoadUserData(),
				branchService.LoadBranchesAsync(),
				branchService.LoadUserLocationAsync(),
				_tenantService.LoadTenantParameters(),
			]);
		};
		productsService.UserMadePurchase += async (s, e) =>
		{
			await _authService.LoadUserData();
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

	protected override void OnAppLinkRequestReceived(Uri uri)
	{
		base.OnAppLinkRequestReceived(uri);
		if (uri.Scheme == "servipuntos" && uri.Host == "validate-magic-link")
		{
			var token = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("token");
			if (token is not null)
			{
				MainThread.BeginInvokeOnMainThread(async () =>
				{
					var routeParams = new Dictionary<string, object>
						{
								{ "Token", token }
						};
					await Shell.Current.GoToAsync(nameof(LoginPage), routeParams);
				});
			}
		}
	}
}
