using Android.App;
using Android.Content;
using Android.Content.PM;
using ServiPuntosUy_mobile.Views;

namespace ServiPuntosUy_mobile;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter([Intent.ActionView],
              Categories = new[] {
                Intent.CategoryDefault,
                Intent.CategoryBrowsable
              },
              DataScheme = "servipuntos",
              DataHost = "callback")]
public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
{
}