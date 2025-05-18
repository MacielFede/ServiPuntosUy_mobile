using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ServiPuntos.uy_mobile.Helpers;

public abstract partial class ApiConnection
{
  private readonly HttpClient _httpClient;

  protected ApiConnection()
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

  public async Task<T?> GET<T>(string siteUrl, string requestUri, string? token, bool hasCurrentTenant)
  {
    _httpClient.DefaultRequestHeaders.Remove("currentTenant");
    if (hasCurrentTenant)
      _httpClient.DefaultRequestHeaders.Add("currentTenant", GetDomain(siteUrl));
    var response = await _httpClient.GetAsync(requestUri);
    var responseContent = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(responseContent);
  }

  public async Task<T?> POST<T>(string siteUrl, string requestUri, string? token, dynamic requestData, bool hasCurrentTenant)
  {
    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

    _httpClient.DefaultRequestHeaders.Remove("currentTenant");
    if (hasCurrentTenant)
      _httpClient.DefaultRequestHeaders.Add("currentTenant", GetDomain(siteUrl));
    if (token != null)
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    var response = await _httpClient.PostAsync(requestUri, content);
    var responseContent = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(responseContent);
  }

  public async Task<T?> PATCH<T>(string siteUrl, string requestUri, string? token, dynamic requestData, bool hasCurrentTenant)
  {
    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

    _httpClient.DefaultRequestHeaders.Remove("currentTenant");
    if (hasCurrentTenant)
      _httpClient.DefaultRequestHeaders.Add("currentTenant", GetDomain(siteUrl));
    if (token != null)
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    // Using SendAsync to support PATCH operation
    var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
    {
      Content = content
    };
    var response = await _httpClient.SendAsync(request);
    var responseContent = await response.Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<T>(responseContent);
  }
}
