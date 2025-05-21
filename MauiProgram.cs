using Microsoft.Extensions.Logging;
using ServiPuntos.uy_mobile.Views;
using DotNet.Meteor.HotReload.Plugin;
using CommunityToolkit.Maui;
using ServiPuntos.uy_mobile.ViewModels;
using ServiPuntos.uy_mobile.Services;
using ServiPuntos.uy_mobile.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using DotNetEnv.Configuration;

namespace ServiPuntos.uy_mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var config = new ConfigurationBuilder().AddDotNetEnv().Build();
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
			}).UseMauiCommunityToolkit();
		// Services
		builder.Services.AddSingleton<IAuthService, AuthService>();
		builder.Services.AddSingleton<IProductsService, ProductsService>();
		builder.Services.AddSingleton<IConfiguration>(config);
		// Pages
		builder.Services.AddSingleton<WelcomePage>();
		builder.Services.AddSingleton<SignUpPage>();
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<HomePage>();
		builder.Services.AddTransient<ProductDetailPage>();
		// ViewModels
		builder.Services.AddSingleton<WelcomeViewModel>();
		builder.Services.AddSingleton<SignUpViewModel>();
		builder.Services.AddSingleton<HomeViewModel>();
		builder.Services.AddTransient<ProductDetailViewModel>();
		builder.Services.AddSingleton<LoginViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
