namespace ServiPuntos.uy_mobile.Views;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
	}
}
