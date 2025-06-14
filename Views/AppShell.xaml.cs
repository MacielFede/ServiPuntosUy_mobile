using ServiPuntos.uy_mobile.Helpers;

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
		// Routing.RegisterRoute(nameof(TransactionsHistoryPage), typeof(TransactionsHistoryPage));

		SetDynamicUnselectedColor();
	}
	private static void SetDynamicUnselectedColor()
	{
		// Get the primary color
		if (Application.Current is not null && Application.Current.Resources.TryGetValue("PrimaryColor", out var primaryColorObj) &&
				primaryColorObj is Color primaryColor)
		{
			Application.Current.Resources["TabBarUnselectedColor"] = ColorHelper.GetBlackOrWhiteBasedOn(primaryColor);
		}
	}
}
