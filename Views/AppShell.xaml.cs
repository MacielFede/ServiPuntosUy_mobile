namespace ServiPuntos.uy_mobile.Views;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
		Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
		Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
		Routing.RegisterRoute(nameof(IdentityVerificationPage), typeof(IdentityVerificationPage));
	}
}
