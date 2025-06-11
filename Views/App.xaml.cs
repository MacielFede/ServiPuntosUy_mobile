using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.ViewModels;

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
			await _branchService.LoadBranchesAsync();
			await _authService.LoadUserData();
			await _branchService.LoadUserLocationAsync();
		};
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	protected override void OnStart()
	{
		base.OnStart();
		_ = _tenantService.LoadTenantUIAsync();
		_ = CheckUserSession();
	}

	private async Task CheckUserSession()
	{
		var sessionData = await _authService.GetSessionData();
		if (sessionData != null /* && sessionData.Expiration > DateTime.Now.AddMinutes(15) */)
		{
			_authService.TriggerSessionCreatedEvent();
			await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
		}
	}
}