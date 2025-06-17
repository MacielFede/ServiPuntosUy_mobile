using Microsoft.Extensions.Configuration;
using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Services.Interfaces;

namespace ServiPuntosUy_mobile.Services;

public class ProductsService(IConfiguration configs) : ApiService(configs), IProductsService
{
  public event EventHandler? UserMadePurchase;
  public void InvokeUserMadePurchaseEvent()
  {
    UserMadePurchase?.Invoke(this, EventArgs.Empty);
  }
  public async Task<ApiResponse<Promotion[]>> GetPromotionsAsync()
  {
    try
    {
      return await GET<Promotion[]>("promotion/tenant");
    }
    catch (Exception ex)
    {
      return new ApiResponse<Promotion[]>(true, null, ex.Message);
    }
  }
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

  public async Task<ApiResponse<Transaction>> PurchaseProduct(ProductForTransaction[] products, int branchId)
  {
    try
    {
      return await POST<Transaction>("transaction", new { BranchId = branchId, products });
    }
    catch (Exception ex)
    {
      return new ApiResponse<Transaction>(true, null, ex.Message);
    }
  }
  public async Task<ApiResponse<SessionData>> CreateProductRedemption(int productId, int branchId)
  {
    try
    {
      return await POST<SessionData>("redemption/generate-token", new { BranchId = branchId, ProductId = productId });
    }
    catch (Exception ex)
    {
      return new ApiResponse<SessionData>(true, null, ex.Message);
    }
  }

  public async Task<ApiResponse<Transaction[]>> GetTransactionHistory()
  {
    try
    {
      return await GET<Transaction[]>("transaction/history");
    }
    catch (Exception ex)
    {
      return new ApiResponse<Transaction[]>(true, null, ex.Message);
    }
  }

  public async Task<ApiResponse<TransactionItem[]>> GetTransactionDetails(int transactionId)
  {
    try
    {
      return await GET<TransactionItem[]>($"transaction/{transactionId}/items");
    }
    catch (Exception ex)
    {
      return new ApiResponse<TransactionItem[]>(true, null, ex.Message);
    }
  }
}
