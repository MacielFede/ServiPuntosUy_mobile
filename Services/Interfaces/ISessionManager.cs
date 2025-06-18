namespace ServiPuntosUy_mobile.Services.Interfaces;

public interface ISessionManager
{
  Task HandleUnauthorizedAsync();
  Task HandleSessionCreatedAsync();
  Task CheckUserSessionAsync();
}