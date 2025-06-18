
namespace ServiPuntosUy_mobile.Services.Interfaces
{
  public interface ITenantService
  {
    public Task LoadTenantUIAsync();
    public Task LoadTenantParameters();
    public Task<int> GetTenantPointValue();
  }
}
