namespace ServiPuntosUy_mobile.Models;

public class User(int id,
 string tenantId,
string email,
string name,
bool isVerified,
int pointBalance,
bool notificationsEnabled)
{
  public int Id { get; set; } = id;
  public string? TenantId { get; set; } = tenantId;
  public string Email { get; set; } = email;
  public string Name { get; set; } = name;
  public bool IsVerified { get; set; } = isVerified;
  public int PointBalance { get; set; } = pointBalance;
  public bool NotificationsEnabled { get; set; } = notificationsEnabled;
}