using ServiPuntos.uy_mobile.Models.Enums;

namespace ServiPuntos.uy_mobile.Models;

public class Transaction
{
  public int Id { get; set; }
  public int BranchId { get; set; }
  public int UserId { get; set; }
  public DateTime CreatedAt { get; set; }
  public decimal Amount { get; set; }
  public int PointsEarned { get; set; }
  public TransactionType Type { get; set; }
  public int PointsSpent { get; set; }

  // Computed properties
  public bool IsCompra => Type == TransactionType.Compra;
  public bool IsCanje => Type == TransactionType.Canje;

  public string BranchAddress { get; set; } = "";
}

