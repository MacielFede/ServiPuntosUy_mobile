namespace ServiPuntosUy_mobile.Models;

public class Promotion
{
  public int PromotionId { get; set; }
  public string Description { get; set; } = "";
  public int Price { get; set; }
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public List<int> Branches { get; set; } = [];
  public List<int> Products { get; set; } = [];
}