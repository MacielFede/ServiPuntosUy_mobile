using ServiPuntosUy_mobile.ViewModels;

namespace ServiPuntosUy_mobile.Views;

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
	}
}
