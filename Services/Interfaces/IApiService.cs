using System.Text.RegularExpressions;
using ServiPuntos.uy_mobile.Models;

namespace ServiPuntos.uy_mobile.Services.Interfaces
{
  public interface IApiService
  {
    Task<ApiResponse<T>> GET<T>(string requestUri);
    Task<ApiResponse<T>> POST<T>(string requestUri, dynamic requestData);
    Task<ApiResponse<T>> PATCH<T>(string requestUri, dynamic requestData);
  }
}
