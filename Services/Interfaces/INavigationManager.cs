namespace ServiPuntosUy_mobile.Services.Interfaces;

public interface INavigationManager
{
  Task HandleAppLinkAsync(Uri uri);
}