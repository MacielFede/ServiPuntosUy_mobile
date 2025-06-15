using Foundation;
using UIKit;
using Firebase.Core;

namespace ServiPuntosUy_mobile;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	public override bool FinishedLaunching(UIApplication app, NSDictionary options)
	{
		App.Configure();
		return base.FinishedLaunching(app, options);
	}
}
