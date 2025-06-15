namespace ServiPuntosUy_mobile.Models;

public class TransactionItem
{
  public int ProductId { get; set; }

  public int Quantity { get; set; }

  public decimal UnitPrice { get; set; }

  public required string ProductName { get; set; }

  public required string ProductImageUrl { get; set; }
}

