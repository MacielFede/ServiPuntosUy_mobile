using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Services.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using ServiPuntosUy_mobile.Models.Enums;
using System.Diagnostics;
using Auth0.OidcClient;
using System.Threading.Tasks;

namespace ServiPuntosUy_mobile.Services;

public class AuthService(IConfiguration configs, Auth0Client auth0Client) : ApiService(configs), IAuthService
{
  private readonly Auth0Client _auth0Client = auth0Client;
  public event EventHandler? SessionCreatedSuccessfully;
  public void TriggerSessionCreatedEvent()
  {
    SessionCreatedSuccessfully?.Invoke(this, EventArgs.Empty);
  }

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

  public async Task<ApiResponse<SessionData>> ValidateMagicLink(string token)
  {
    try
    {
      var payload = new { Token = token };
      return await POST<SessionData>("auth/validate-magic-link", payload);
    }
    catch (Exception ex)
    {
      return new ApiResponse<SessionData>(true, null, ex.Message);
    }
  }
  public async Task<ApiResponse<string>> CreateMagicLink(string email)
  {
    try
    {
      var payload = new { Email = email };
      return await POST<string>("auth/magic-link", payload);
    }
    catch (Exception ex)
    {
      return new ApiResponse<string>(true, null, ex.Message);
    }
  }
  public async Task<ApiResponse<SessionData>> LoginGoogle()
  {
    try
    {
      var loginResult = await _auth0Client.LoginAsync();
      if (!loginResult.IsError)
      {
        var payload = new GoogleUser()
        {
          IdToken = loginResult.AccessToken,
          Email = loginResult.User.Claims.First(c => c.Type == "email")?.Value!,
          Name = loginResult.User.Identity?.Name!,
          GoogleId = loginResult.User.Claims.First(c => c.Type == "sid").Value!
        };
        return await POST<SessionData>("auth/google-login", payload);
      }
      else
      {
        return new ApiResponse<SessionData>(true, null, loginResult.ErrorDescription);
      }
    }
    catch (Exception ex)
    {
      return new ApiResponse<SessionData>(true, null, ex.Message);
    }
  }

  public async Task<ApiResponse<SessionData>> Signup(string name, string email, string password)
  {
    var payload = new { Name = name, Email = email, Password = password };
    return await POST<SessionData>("auth/signup", payload);
  }

  public async Task Logout()
  {
    await _auth0Client.LogoutAsync();
    SecureStorage.Remove(SecureStorageType.Session.ToString());
    SecureStorage.Remove(SecureStorageType.User.ToString());
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

  public async Task<SessionData?> GetSessionData() => JsonConvert.DeserializeObject<SessionData>(await SecureStorage.GetAsync(SecureStorageType.Session.ToString()) ??
                                                   string.Empty);

  public async Task LoadUserData()
  {
    try
    {
      var userResponse = await GET<User>("auth/me");
      if (userResponse is { Error: false, Data: not null })
      {
        var userJson = JsonConvert.SerializeObject(userResponse.Data);
        await SecureStorage.SetAsync(SecureStorageType.User.ToString(), userJson);
      }
      else
      {
        Debug.WriteLine($"Error obteniendo informacion del usuario: {userResponse?.Message}");
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"Error obteniendo informacion del usuario: {ex}");
    }
  }


}
