using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.Services
{
  public class ProductsService : IProductsService
  {
    public Task<List<Product>> GetProductsAsync()
    {
      // Simulate fetching data from an API or database
      var products = new List<Product>
            {
                new(1,"Product 1","Description of Product 1", null),
                new(2,"Product 2","Description of Product 2", null),
                new(3,"Product 3","Description of Product 3", null)
            };
      return Task.FromResult(products);
    }
  }
}
