namespace ServiPuntosUy_mobile.Models;

public class Product()
{

  public int Id { get; set; }
  public int TenantId { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public string? ImageUrl { get; set; }
  public decimal Price { get; set; }
  public bool AgeRestricted { get; set; }
  public int Stock { get; set; } = 0;
  public bool HasPromotion { get; set; } = false;
  public int? PromotionId { get; set; }
  public string PromotionDescription { get; set; } = "";
  public decimal OriginalPrice { get; set; } = 0m;
  public decimal PromotionalPrice { get; set; } = 0m;
  public decimal Discount => OriginalPrice - PromotionalPrice;
  public decimal DiscountPercentage => OriginalPrice > 0 ? Math.Round((Discount / OriginalPrice) * 100, 2) : 0;
}