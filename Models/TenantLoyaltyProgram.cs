namespace ServiPuntos.uy_mobile.Models;

public class TenantLoyaltyProgram
{
  public int Id { get; set; }

  public int TenantId { get; set; } = 0;

  public string PointsName { get; set; } = string.Empty;

  public int PointsValue { get; set; } = 1;

  public decimal AccumulationRule { get; set; } = 1m;

  public int ExpiricyPolicyDays { get; set; } = 180;
}

