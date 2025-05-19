using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ServiPuntos.uy_mobile.Services.Interfaces;

namespace ServiPuntos.uy_mobile.Services
{
  public partial class ApiService : IApiService
  {
    private readonly HttpClient _httpClient;

    public ApiService()
    {
      _httpClient = new()
      {
        BaseAddress = new Uri("https://servipuntos.com")
      };
    }

    [GeneratedRegex(@"https://(.*?).servipuntos.com/")]
    private static partial Regex MyRegex();

    protected static string GetDomain(string url)
    {
      Console.WriteLine(url);
      Console.WriteLine(MyRegex().Match(url).Groups[1].Value);
      return MyRegex().Match(url).Groups[1].Value;
    }

    public async Task<T?> GET<T>(string tenantUrl, string requestUri, string? token, bool hasCurrentTenant)
    {
      var response = await _httpClient.GetAsync(requestUri);
      var responseContent = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(responseContent);
    }

    public async Task<T?> POST<T>(string tenantUrl, string requestUri, string? token, dynamic requestData, bool hasCurrentTenant)
    {
      var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
      if (token != null)
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
      var response = await _httpClient.PostAsync(requestUri, content);
      var responseContent = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(responseContent);
    }

    public async Task<T?> PATCH<T>(string tenantUrl, string requestUri, string? token, dynamic requestData, bool hasCurrentTenant)
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
}
