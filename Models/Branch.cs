namespace ServiPuntos.uy_mobile.Models;

public class Branch
{
  public int Id { get; set; }
  public int TenantId { get; set; }
  public Tenant? Tenant { get; set; }
  public string? Address { get; set; }
  public double Latitud { get; set; }
  public double Longitud { get; set; }
  public string? Phone { get; set; }
  public TimeOnly OpenTime { get; set; }
  public TimeOnly ClosingTime { get; set; }
}
