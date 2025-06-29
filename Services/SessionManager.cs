using System.Diagnostics;
using ServiPuntosUy_mobile.Services.Interfaces;
using ServiPuntosUy_mobile.Views;

namespace ServiPuntosUy_mobile.Services;

public class SessionManager : ISessionManager
{
  private readonly IAuthService _authService;
  private readonly IBranchService _branchService;
  private readonly ITenantService _tenantService;

  public SessionManager(IApiService apiService, IAuthService authService, IBranchService branchService, ITenantService tenantService, IProductsService productsService)
  {
    _authService = authService;
    _branchService = branchService;
    _tenantService = tenantService;
    apiService.UserUnauthorized += async (s, e) => await HandleUnauthorizedAsync();
    productsService.UserMadePurchase += async (s, e) => await _authService.LoadUserData();
    _authService.SessionCreatedSuccessfully += async (s, e) => await HandleSessionCreatedAsync();
  }

  public async Task CheckUserSessionAsync()
  {
    var sessionData = await _authService.GetSessionData();
    if (sessionData != null)
    {
      _authService.TriggerSessionCreatedEvent();
      await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
  }

  public async Task HandleUnauthorizedAsync()
  {
    try
    {
      if (Shell.Current is not null)
      {
        var currentPage = Shell.Current.CurrentPage.GetType().Name;
        if (currentPage is not (nameof(LoginPage) or nameof(SignUpPage) or nameof(WelcomePage)))
        {
          await _authService.Logout();
          await Shell.Current.GoToAsync($"//{nameof(WelcomePage)}");
        }
      }
    }
    catch (Exception e)
    {
      Debug.WriteLine(e.Message);
      await _authService.Logout();
      await Shell.Current.GoToAsync($"//{nameof(WelcomePage)}");
    }
  }

  public async Task HandleSessionCreatedAsync()
  {
    await Task.WhenAll([
        _authService.LoadUserData(),
            _branchService.LoadBranchesAsync(),
            _branchService.LoadUserLocationAsync(),
            _tenantService.LoadTenantParameters(),
        ]);
  }
}
