namespace ServiPuntosUy_mobile.Models;

public class Product(int id,
int tenantId,
string name,
string description,
string imageUrl,
decimal price,
bool ageRestricted)
{

  public int Id { get; set; } = id;
  public int TenantId { get; set; } = tenantId;
  public string Name { get; set; } = name;
  public string Description { get; set; } = description;
  public string ImageUrl { get; set; } = imageUrl;
  public decimal Price { get; set; } = price;
  public bool AgeRestricted { get; set; } = ageRestricted;
}
