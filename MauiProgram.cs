using Microsoft.Extensions.Logging;
using ServiPuntosUy_mobile.Views;
using DotNet.Meteor.HotReload.Plugin;
using CommunityToolkit.Maui;
using ServiPuntosUy_mobile.ViewModels;
using ServiPuntosUy_mobile.Services;
using ServiPuntosUy_mobile.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Auth0.OidcClient;
using System.Reflection;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.CloudMessaging;

#if IOS
using Plugin.Firebase.Core.Platforms.iOS;
#elif ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#endif

namespace ServiPuntosUy_mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ServiPuntosUy_mobile.appsettings.json") ?? throw new Exception("The specified appsettings.json file can't be loaded");
		var config = new ConfigurationBuilder()
			.AddJsonStream(stream)
			.Build();
		var builder = MauiApp.CreateBuilder();
#if DEBUG
		builder.Logging.AddDebug();
		builder.EnableHotReload()
#endif
				.UseMauiApp<App>()
				.RegisterFirebaseServices()
				.UseSkiaSharp()
				.ConfigureFonts(fonts =>
		{
			fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
		})
		.UseMauiCommunityToolkit()
		.UseMauiMaps()
		.Configuration.AddConfiguration(config);

		// Services
		builder.Services.AddSingleton(new Auth0Client(new()
		{
			Domain = config["AUTH_DOMAIN"],
			ClientId = config["AUTH_CLIENTID"],
			RedirectUri = "servipuntos://callback/",
			PostLogoutRedirectUri = "servipuntos://callback/",
			Scope = "openid profile email"
		}));
		builder.Services.AddSingleton<ISessionManager, SessionManager>();
		builder.Services.AddSingleton<INotificationService, NotificationService>();
		builder.Services.AddSingleton<INavigationManager, NavigationManager>();
		builder.Services.AddSingleton<IApiService, ApiService>();
		builder.Services.AddSingleton<IAuthService, AuthService>();
		builder.Services.AddSingleton<IProductsService, ProductsService>();
		builder.Services.AddSingleton<IBranchService, BranchService>();
		builder.Services.AddSingleton<IFuelService, FuelService>();
		builder.Services.AddSingleton<IGeoService, GeoService>();
		builder.Services.AddSingleton<ITenantService, TenantService>();
		builder.Services.AddTransient<IQrCodeService, QrCodeService>();

		// Pages
		builder.Services.AddSingleton<WelcomePage>();
		builder.Services.AddTransient<SignUpPage>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddSingleton<HomePage>();
		builder.Services.AddSingleton<ProductDetailPage>();
		builder.Services.AddSingleton<IdentityVerificationPage>();
		builder.Services.AddSingleton<FuelPricesPage>();
		builder.Services.AddSingleton<BranchesPage>();
		builder.Services.AddSingleton<TransactionsHistoryPage>();
		builder.Services.AddSingleton<PromotionDetailPage>();

		// ViewModels
		builder.Services.AddSingleton<WelcomeViewModel>();
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<SignUpViewModel>();
		builder.Services.AddSingleton<HomeViewModel>();
		builder.Services.AddSingleton<ProductDetailViewModel>();
		builder.Services.AddTransient<IdentityVerificationViewModel>();
		builder.Services.AddSingleton<FuelPricesViewModel>();
		builder.Services.AddSingleton<BranchesViewModel>();
		builder.Services.AddSingleton<TransactionsHistoryViewModel>();
		builder.Services.AddSingleton<PromotionDetailViewModel>();
		return builder.Build();
	}

	private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
	{
		builder.ConfigureLifecycleEvents(events =>
		{
#if IOS
			events.AddiOS(iOS => iOS.WillFinishLaunching((_, __) =>
			{
				CrossFirebase.Initialize();
				FirebaseCloudMessagingImplementation.Initialize();
				return false;
			}));
#elif ANDROID
			events.AddAndroid(android => android.OnCreate((activity, _) =>
					CrossFirebase.Initialize(activity)));
#endif
		});
		return builder;
	}
}
