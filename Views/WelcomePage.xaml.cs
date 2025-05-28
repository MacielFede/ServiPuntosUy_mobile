using ServiPuntos.uy_mobile.ViewModels;

namespace ServiPuntos.uy_mobile.Views;

public partial class WelcomePage : ContentPage
{
	public WelcomePage(WelcomeViewModel welcomeViewModel)
	{
		InitializeComponent();
		BindingContext = welcomeViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as WelcomeViewModel)?.GetTenantName();
		_ = (BindingContext as WelcomeViewModel)?.CheckUserSession();
	}
}
