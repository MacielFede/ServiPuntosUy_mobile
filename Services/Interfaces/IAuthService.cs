using ServiPuntosUy_mobile.Models;

namespace ServiPuntosUy_mobile.Services.Interfaces;

public interface IAuthService
{
  public event EventHandler? SessionCreatedSuccessfully;
  public Task<ApiResponse<SessionData>> Login(string email, string password);

  public Task<ApiResponse<SessionData>> LoginGoogle();
  public Task<ApiResponse<string>> CreateMagicLink(string email);
  public Task<ApiResponse<SessionData>> ValidateMagicLink(string token);

  public Task<ApiResponse<SessionData>> Signup(string name, string email, string password);

  public Task Logout();
  public Task SaveSession(SessionData sessionData);
  public Task<ApiResponse<User>> VerifyIdentity(Document document);
  public Task LoadUserData();
  public Task<SessionData?> GetSessionData();
  public void TriggerSessionCreatedEvent();
}