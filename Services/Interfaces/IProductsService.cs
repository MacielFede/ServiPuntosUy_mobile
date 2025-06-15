using ServiPuntosUy_mobile.Models;

namespace ServiPuntosUy_mobile.Services.Interfaces;

public interface IProductsService
{
  public Task<ApiResponse<Product[]>> GetProductsAsync();
  public Task<ApiResponse<Product>> GetProductInfo(int id);
  public Task<ApiResponse<Transaction>> PurchaseProduct(ProductForTransaction[] products, int branchId);
  public Task<ApiResponse<SessionData>> CreateProductRedemption(int productId, int branchId);
  public Task<ApiResponse<Transaction[]>> GetTransactionHistory();
  public Task<ApiResponse<TransactionItem[]>> GetTransactionDetails(int transactionId);
}

