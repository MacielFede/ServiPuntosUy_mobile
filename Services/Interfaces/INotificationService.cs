namespace ServiPuntosUy_mobile.Services.Interfaces;

public interface INotificationService
{
  Task InitializeAsync();
  void ClearNotifications();
}