using ServiPuntosUy_mobile.Models;

namespace ServiPuntosUy_mobile.Services.Interfaces;

public interface IProductsService
{
  public event EventHandler? UserMadePurchase;
  public void InvokeUserMadePurchaseEvent();
  public Task<ApiResponse<Product[]>> GetProductsAsync();
  public Task<ApiResponse<Promotion[]>> GetPromotionsAsync();
  public Task<ApiResponse<Product>> GetProductInfo(int id);
  public Task<ApiResponse<Product>> GetProductStock(int id, int branchId);
  public Task<ApiResponse<Transaction>> PurchaseProduct(ProductForTransaction[] products, int branchId);
  public Task<ApiResponse<SessionData>> CreateProductRedemption(ProductForTransaction[] products, int branchId);
  public Task<ApiResponse<Transaction[]>> GetTransactionHistory();
  public Task<ApiResponse<TransactionItem[]>> GetTransactionDetails(int transactionId);
}

