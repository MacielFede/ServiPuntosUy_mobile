using System.Diagnostics;
using Newtonsoft.Json;
using ServiPuntos.uy_mobile.Models;
using ServiPuntos.uy_mobile.Models.Enums;

namespace ServiPuntos.uy_mobile.Helpers;

public static class TenantParameterHelper
{
  public static async Task<string> GetCurrencyLabelAsync()
  {
    try
    {
      string? parametersJson = await SecureStorage.GetAsync(SecureStorageType.TenantParameters.ToString());

      if (string.IsNullOrWhiteSpace(parametersJson))
        return "$";

      var parameters = JsonConvert.DeserializeObject<TenantParameter[]>(parametersJson);
      var currencyParam = parameters?.FirstOrDefault(p => p.Key.Equals("Currency", StringComparison.OrdinalIgnoreCase));
      return currencyParam?.Value ?? "$";
    }
    catch (Exception ex)
    {
      Debug.WriteLine(ex.Message);
      return "$";
    }
  }
}