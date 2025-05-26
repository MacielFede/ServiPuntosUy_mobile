using ServiPuntos.uy_mobile.ViewModels;

namespace ServiPuntos.uy_mobile.Views;

public partial class WelcomePage : ContentPage
{
	private readonly WelcomeViewModel _welcomeViewModel;
	public WelcomePage(WelcomeViewModel welcomeViewModel)
	{
		InitializeComponent();
		_welcomeViewModel = welcomeViewModel;
		BindingContext = welcomeViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_ = _welcomeViewModel.CheckUserSession();
	}
}
