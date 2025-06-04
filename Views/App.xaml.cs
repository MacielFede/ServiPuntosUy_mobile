using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.ViewModels;

namespace ServiPuntos.uy_mobile.Views;

public partial class App : Application
{
	private readonly IBranchService _branchService;
	public App(IBranchService branchService)
	{
		InitializeComponent();
		_branchService = branchService;
		LoginViewModel.LoggedInSuccessfully += async (s, e) =>
		{
			await branchService.LoadLocationsAsync();
		};
		SignUpViewModel.SignUpSuccessfully += async (s, e) =>
		{
			await branchService.LoadLocationsAsync();
		};
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	protected override void OnStart()
	{
		base.OnStart();
		_branchService.LoadLocationsAsync();
	}
}