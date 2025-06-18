using System.Web;
using ServiPuntosUy_mobile.Views;
using ServiPuntosUy_mobile.Services.Interfaces;

namespace ServiPuntosUy_mobile.Services;

public class NavigationManager : INavigationManager
{
  public async Task HandleAppLinkAsync(Uri uri)
  {
    if (uri is null || !uri.IsAbsoluteUri)
      return;

    if (uri.Scheme == "servipuntos" && uri.Host == "validate-magic-link")
    {
      string? token = HttpUtility.ParseQueryString(uri.Query).Get("token");

      if (!string.IsNullOrWhiteSpace(token))
      {
        var routeParams = new Dictionary<string, object>
        {
          { "Token", token }
        };

        await Shell.Current.GoToAsync(nameof(LoginPage), routeParams);
      }
    }
  }
}
