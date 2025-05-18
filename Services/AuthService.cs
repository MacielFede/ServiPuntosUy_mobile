using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.Services;

public class IdentityService() : BaseService, IIdentityService
{
  private const string Uri = "identity";
  // public async Task<ApiResponse<SessionData>?> Login(string siteUrl, string email, string password, int accessType)
  // {
  //   var requestUri = $"{Uri}/BasicLogin?siteAccess={accessType}";
  //   var payload = new { Email = email, Password = password };

  //   return await GeneratePostRequest<ApiResponse<SessionData>>(siteUrl, requestUri, null, payload, true);
  // }

  // public async Task<ApiResponse<SessionData>?> LoginAuth0(string siteUrl, int accessType)
  // {
  //   var loginResult = await auth0Client.LoginAsync();
  //   var payload = new { Token = loginResult.TokenResponse.IdentityToken, IsAllowedRegister = true };

  //   var requestUri = $"{Uri}/OAuthLogin?siteAccess={accessType}";

  //   return await GeneratePostRequest<ApiResponse<SessionData>>(siteUrl, requestUri, null, payload, true);
  // }

  public async Task<ApiResponse<SessionData>?> Signup(string siteUrl, string name, string email, string password,
      int accessType)
  {
    var requestUri = $"{Uri}/BasicSignup?siteAccess={accessType}";
    var payload = new { Name = name, Email = email, Password = password };

    return await GeneratePostRequest<ApiResponse<SessionData>>(siteUrl, requestUri, null, payload, true);
  }

  public async Task Logout()
  {
    // await auth0Client.LogoutAsync();
    SecureStorage.RemoveAll();
  }

  public async Task SaveSession(SessionData sessionData, string siteUrl)
  {
    var sessionJson = JsonConvert.SerializeObject(sessionData);
    await SecureStorage.SetAsync("SESSION", sessionJson);
  }
}