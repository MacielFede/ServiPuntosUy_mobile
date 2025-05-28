using Microsoft.Extensions.Configuration;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.Services;

public class ProductsService(IConfiguration configs) : ApiService(configs), IProductsService
{
  public async Task<ApiResponse<Product[]>> GetProductsAsync()
  {
    try
    {
      return await GET<Product[]>("product");
    }
    catch (Exception ex)
    {
      return new ApiResponse<Product[]>(true, null, ex.Message);
    }
  }

  public async Task<ApiResponse<Product>> GetProductInfo(int id)
  {
    try
    {
      var response = await GET<Product>($"product/{id}");
      return response;
    }
    catch (Exception ex)
    {
      return new ApiResponse<Product>(true, null, ex.Message);
    }
  }
}
