namespace ServiPuntos.uy_mobile.Models;

public class Product(int id, string name, string description, string? imageUrl)
{
  public int Id { get; set; } = id;
  public string Name { get; set; } = name;
  public string Description { get; set; } = description;
  public string? ImageUrl { get; set; } = imageUrl;
}
