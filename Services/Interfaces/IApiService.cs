using System.Text.RegularExpressions;
using ServiPuntosUy_mobile.Models;

namespace ServiPuntosUy_mobile.Services.Interfaces
{
  public interface IApiService
  {
    public event EventHandler? UserUnauthorized;
    Task<ApiResponse<T>> GET<T>(string requestUri);
    Task<ApiResponse<T>> POST<T>(string requestUri, dynamic requestData);
  }
}
