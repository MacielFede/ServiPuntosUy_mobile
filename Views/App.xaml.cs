using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using Firebase.Messaging;
using Plugin.Firebase.CloudMessaging;
using ServiPuntosUy_mobile.Services.Interfaces;

namespace ServiPuntosUy_mobile.Views;

public partial class App : Application
{
	private readonly IAuthService _authService;
	private readonly IBranchService _branchService;
	private readonly ITenantService _tenantService;
	public App(IBranchService branchService, IAuthService authService, ITenantService tenantService)
	{
		InitializeComponent();
		_branchService = branchService;
		_authService = authService;
		_tenantService = tenantService;
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

	private static async void SubscribeToTopic()
	{
		try
		{
			await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
			await CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("ofertas");
			System.Diagnostics.Debug.WriteLine("Subscribed to topic: ofertas");
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to subscribe to topic: {ex.Message}");
		}
	}

	private static void SetupNotificationHandlers()
	{
		CrossFirebaseCloudMessaging.Current.NotificationReceived += (s, e) =>
		{
			MainThread.BeginInvokeOnMainThread(async () =>
					{
						Debug.WriteLine("ESTOY llego noti");
						// await Shell.Current.ShowPopupAsync(new Label
						// {
						// 	Text = e.Notification.Body
						// });

						//DisplayAlert("Oferta!", e.Notification.Body, "OK");
					});
		};
		CrossFirebaseCloudMessaging.Current.NotificationTapped += (s, e) =>
		{
			MainThread.BeginInvokeOnMainThread(async () =>
					{
						Debug.WriteLine("ESTOY toque noti");
						await Shell.Current.DisplayAlert("Oferta!", e.Notification.Body, "OK");
					});
		};
	}
}