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
		// TODO: Cargar sesion y mandarlo a la home si ya esta logeado
		// var sessionData =
		//     JsonConvert.DeserializeObject<SessionData>(await SecureStorage.GetAsync("SESSION") ??
		//                                                string.Empty);

		// if (sessionData != null && sessionData.Expiration > DateTime.Now.AddMinutes(15))
		// {
		//     await _selectSiteViewModel.GoToEventPage();
		// }
	}
}
