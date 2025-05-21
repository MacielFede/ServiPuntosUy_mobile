using System.Text.RegularExpressions;

namespace ServiPuntos.uy_mobile.Services.Interfaces
{
  public interface IApiService
  {
    Task<T?> GET<T>(string requestUri, string? token, bool hasCurrentTenant);
    Task<T?> POST<T>(string requestUri, string? token, dynamic requestData, bool hasCurrentTenant);
    Task<T?> PATCH<T>(string requestUri, string? token, dynamic requestData, bool hasCurrentTenant);
  }
}
