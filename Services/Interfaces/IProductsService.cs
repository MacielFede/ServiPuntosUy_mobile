using ServiPuntos.uy_mobile.Models;

namespace ServiPuntos.uy_mobile.Services.Interfaces;

public interface IProductsService
{
  public Task<ApiResponse<Product[]>> GetProductsAsync();
  public Task<ApiResponse<Product>> GetProductInfo(int id);
}

