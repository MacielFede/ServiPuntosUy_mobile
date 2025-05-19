using Microsoft.Extensions.Logging;
using ServiPuntos.uy_mobile.Views;
using DotNet.Meteor.HotReload.Plugin;
using CommunityToolkit.Maui;
using ServiPuntos.uy_mobile.ViewModels;
using ServiPuntos.uy_mobile.Services;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
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
		// Pages
		builder.Services.AddSingleton<WelcomePage>();
		builder.Services.AddSingleton<SignUpPage>();
		builder.Services.AddSingleton<LoginPage>();
		// ViewModels
		builder.Services.AddSingleton<WelcomeViewModel>();
		builder.Services.AddSingleton<SignUpViewModel>();
		// builder.Services.AddSingleton<LoginViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
