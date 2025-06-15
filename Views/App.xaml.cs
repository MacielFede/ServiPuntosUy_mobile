using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.Views;

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
}