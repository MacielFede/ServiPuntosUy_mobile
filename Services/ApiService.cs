using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.Services;

public partial class ApiService : IApiService
{
  private readonly HttpClient _httpClient;
  private readonly IConfiguration _configs;

  public ApiService(IConfiguration configuration)
  {
    _configs = configuration;
    _httpClient = new()
    {
      // BaseAddress = new Uri(_configs["Settings:ApiUrl"]!),
      BaseAddress = new Uri("http://localhost:8000/"),
    };
    _httpClient.DefaultRequestHeaders.Add("X-Tenant-Id", _configs["Settings:TenantId"]);
  }

  public async Task<T?> GET<T>(string requestUri, string? token, bool hasCurrentTenant)
  {
    foreach (var item in _configs.GetChildren())
    {
      Console.WriteLine($"Key: {item.Key}, Value: {item.Value}");
    }
    await Shell.Current.DisplayAlert("hola", _configs["API:URL"], "OK");
    return default;
    var response = await _httpClient.GetAsync(requestUri);
    var responseContent = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(responseContent);
  }

  public async Task<T?> POST<T>(string requestUri, string? token, dynamic requestData, bool hasCurrentTenant)
  {
    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
    if (token != null)
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var response = await _httpClient.PostAsync(requestUri, content);
    var responseContent = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(responseContent);
  }

  public async Task<T?> PATCH<T>(string requestUri, string? token, dynamic requestData, bool hasCurrentTenant)
  {
    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
    if (token != null)
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
    {
      Content = content
    };
    var response = await _httpClient.SendAsync(request);
    var responseContent = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(responseContent);
  }
}

