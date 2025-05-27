using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using ServiPuntos.uy_mobile.Models.Enums;
using System.Threading.Tasks;

namespace ServiPuntos.uy_mobile.Services;

public class AuthService(IConfiguration configs) : ApiService(configs), IAuthService
{
  public async Task<ApiResponse<SessionData>> Login(string email, string password)
  {
    try
    {
      var payload = new { Email = email, Password = password };
      return await POST<SessionData>("auth/login", payload);
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

    var requestUri = "auth/OAuthLogin?siteAccess={accessType}";

    return await POST<SessionData>(requestUri, null);
  }

  public async Task<ApiResponse<SessionData>?> Signup(string name, string email, string password)
  {
    var requestUri = "auth/BasicSignup?siteAccess";
    var payload = new { Name = name, Email = email, Password = password };

    return await POST<SessionData>(requestUri, payload);
  }

  public async Task Logout()
  {
    // await auth0Client.LogoutAsync();
    SecureStorage.RemoveAll();
  }

  public async Task SaveSession(SessionData sessionData)
  {
    var sessionJson = JsonConvert.SerializeObject(sessionData);
    await SecureStorage.SetAsync(SecureStorageType.Session.ToString(), sessionJson);
  }

  public async Task<ApiResponse<User>> VerifyIdentity(Document document)
  {
    try
    {
      return await POST<User>("VEAI", document);
    }
    catch (Exception ex)
    {
      return new ApiResponse<User>(true, null, ex.Message);
    }
  }
}
