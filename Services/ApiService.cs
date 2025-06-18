using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiPuntosUy_mobile.Services.Interfaces;
using ServiPuntosUy_mobile.Models;
using ServiPuntosUy_mobile.Models.Enums;

namespace ServiPuntosUy_mobile.Services;

public partial class ApiService : IApiService
{
  public event EventHandler? UserUnauthorized;
  private readonly HttpClient _httpClient;
  private readonly IConfiguration _configs;

  public ApiService(IConfiguration configuration)
  {
    _configs = configuration;
    _httpClient = new()
    {
      BaseAddress = new Uri(_configs["API_URL"]!),
    };
    _httpClient.DefaultRequestHeaders.Add("X-Tenant-Name", _configs["TENANT_NAME"]);
  }

  public async Task<ApiResponse<T>> GET<T>(string requestUri)
  {
    try
    {
      var storedToken = await GetTokenAsync();
      if (storedToken != null)
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storedToken);
      var response = await _httpClient.GetAsync(requestUri);
      if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
      {
        UserUnauthorized?.Invoke(this, EventArgs.Empty);
        throw new UnauthorizedAccessException("Unauthorized");
      }
      var responseContent = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent) ?? new ApiResponse<T>(true, default, "Error interno, intenta nuevamente más tarde.");
    }
    catch (Exception ex)
    {
      return new ApiResponse<T>(true, default, ex.Message);
    }
  }

  public async Task<ApiResponse<T>> POST<T>(string requestUri, dynamic requestData)
  {
    try
    {
      var storedToken = await GetTokenAsync();
      if (storedToken is not null)
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storedToken);
      var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(requestUri, content);
      if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
      {
        UserUnauthorized?.Invoke(this, EventArgs.Empty);
        throw new UnauthorizedAccessException("Unauthorized");
      }
      var responseContent = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent) ?? new ApiResponse<T>(true, default, "Error interno, intenta nuevamente más tarde.");
    }
    catch (Exception ex)
    {
      return new ApiResponse<T>(true, default, ex.Message);
    }
  }

  public async Task<ApiResponse<T>> PATCH<T>(string requestUri, dynamic requestData)
  {
    try
    {
      var storedToken = await GetTokenAsync();
      if (storedToken != null)
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storedToken);
      var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

      var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
      {
        Content = content
      };
      var response = await _httpClient.SendAsync(request);
      if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
      {
        UserUnauthorized?.Invoke(this, EventArgs.Empty);
        throw new UnauthorizedAccessException("Unauthorized");
      }
      var responseContent = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent) ?? new ApiResponse<T>(true, default, "Error interno, intenta nuevamente más tarde.");
    }
    catch (Exception ex)
    {
      return new ApiResponse<T>(true, default, ex.Message);
    }
  }

  private static async Task<string?> GetTokenAsync()
  {
    string? sessionJson = await SecureStorage.GetAsync(SecureStorageType.Session.ToString());
    if (string.IsNullOrWhiteSpace(sessionJson))
      return null;
    var sessionData = JsonConvert.DeserializeObject<SessionData>(sessionJson);
    return sessionData?.Token;
  }
}
