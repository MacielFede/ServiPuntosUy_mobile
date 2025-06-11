using Microsoft.Extensions.Logging;
using ServiPuntos.uy_mobile.Views;
using DotNet.Meteor.HotReload.Plugin;
using CommunityToolkit.Maui;
using ServiPuntos.uy_mobile.ViewModels;
using ServiPuntos.uy_mobile.Services;
using ServiPuntos.uy_mobile.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ServiPuntos.uy_mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		// TODO: GLOBAL CONFIGS
		var configDic = new Dictionary<string, string?>
				{
						{"API_URL" ,"http://10.0.2.2:5162/api/"},
						{"TENANT_ID" ,"1"},
						{"TENANT_NAME" ,"Ancap"},
				};
		var config = new ConfigurationBuilder().AddInMemoryCollection(configDic).Build();
		var builder = MauiApp.CreateBuilder();
		builder
				.UseMauiApp<App>()
#if DEBUG
				.EnableHotReload()
#endif
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				})
				.UseMauiCommunityToolkit()
				.UseMauiMaps()
				.Configuration.AddConfiguration(config);

		// Services
		builder.Services.AddSingleton<IAuthService, AuthService>();
		builder.Services.AddSingleton<IProductsService, ProductsService>();
		builder.Services.AddSingleton<IBranchService, BranchService>();
		builder.Services.AddSingleton<IFuelService, FuelService>();
		builder.Services.AddSingleton<IGeoService, GeoService>();
		builder.Services.AddSingleton<ITenantService, TenantService>();

		// Pages
		builder.Services.AddSingleton<WelcomePage>();
		builder.Services.AddSingleton<SignUpPage>();
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<HomePage>();
		builder.Services.AddSingleton<ProductDetailPage>();
		builder.Services.AddSingleton<IdentityVerificationPage>();
		builder.Services.AddSingleton<FuelPricesPage>();
		builder.Services.AddSingleton<BranchesPage>();
		builder.Services.AddSingleton<TransactionsHistoryPage>();

		// ViewModels
		builder.Services.AddSingleton<WelcomeViewModel>();
		builder.Services.AddSingleton<SignUpViewModel>();
		builder.Services.AddSingleton<HomeViewModel>();
		builder.Services.AddSingleton<ProductDetailViewModel>();
		builder.Services.AddSingleton<IdentityVerificationViewModel>();
		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<FuelPricesViewModel>();
		builder.Services.AddSingleton<BranchesViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
