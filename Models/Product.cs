namespace ServiPuntos.uy_mobile.Models;

public class Product(int id, string name, string description)
{
  public int Id { get; set; } = id;
  public string Name { get; set; } = name;
  public string Description { get; set; } = description;
}

