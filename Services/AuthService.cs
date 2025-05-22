using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace ServiPuntos.uy_mobile.Services;

public class AuthService(IConfiguration configs) : ApiService(configs), IAuthService
{
  private const string Uri = "login";
  public async Task<ApiResponse<SessionData>> Login(string email, string password)
  {
    try
    {
      var payload = new { Email = email, Password = password };
      return await POST<SessionData>(Uri, null, payload, true);
    }
    catch (Exception ex)
    {
      return new ApiResponse<SessionData>(true, null, ex.Message);
    }
  }

  public async Task<ApiResponse<SessionData>?> LoginAuth0(int accessType)
  {
    // var loginResult = await auth0Client.LoginAsync();
    // var payload = new { Token = loginResult.TokenResponse.IdentityToken, IsAllowedRegister = true };

    var requestUri = $"{Uri}/OAuthLogin?siteAccess={accessType}";

    return await POST<SessionData>(requestUri, null, null, true);
  }

  public async Task<ApiResponse<SessionData>?> Signup(string name, string email, string password)
  {
    var requestUri = $"{Uri}/BasicSignup?siteAccess";
    var payload = new { Name = name, Email = email, Password = password };

    return await POST<SessionData>(requestUri, null, payload, true);
  }

  public async Task Logout()
  {
    // await auth0Client.LogoutAsync();
    SecureStorage.RemoveAll();
  }

  public async Task SaveSession(SessionData sessionData)
  {
    var sessionJson = JsonConvert.SerializeObject(sessionData);
    await SecureStorage.SetAsync("SESSION", sessionJson);
  }
}
