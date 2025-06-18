using ServiPuntosUy_mobile.Services.Interfaces;

namespace ServiPuntosUy_mobile.Views;

public partial class App : Application
{
	private readonly ISessionManager _sessionCoordinator;
	private readonly INotificationService _notificationService;
	private readonly INavigationManager _navigationManager;
	private readonly ITenantService _tenantService;

	public App(ISessionManager sessionCoordinator,
						 INotificationService notificationService,
						 INavigationManager navigationManager, ITenantService tenantService)
	{
		InitializeComponent();

		_sessionCoordinator = sessionCoordinator;
		_notificationService = notificationService;
		_navigationManager = navigationManager;
		_tenantService = tenantService;
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
			_sessionCoordinator.CheckUserSessionAsync(),
			_notificationService.InitializeAsync(),
		]);
	}

	protected override void OnAppLinkRequestReceived(Uri uri)
	{
		base.OnAppLinkRequestReceived(uri);
		MainThread.BeginInvokeOnMainThread(() => _navigationManager.HandleAppLinkAsync(uri));
	}
}
