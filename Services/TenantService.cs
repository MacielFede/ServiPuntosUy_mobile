using Microsoft.Extensions.Configuration;
using ServiPuntosUy_mobile.Services.Interfaces;
using ServiPuntosUy_mobile.Models;
using System.Diagnostics;
using ServiPuntosUy_mobile.Models.Enums;
using ServiPuntosUy_mobile.Helpers;
using Newtonsoft.Json;
using ServiPuntosUy_mobile.Converters;

namespace ServiPuntosUy_mobile.Services;

public class TenantService(IConfiguration configs) : ApiService(configs), ITenantService
{
  public async Task LoadTenantUIAsync()
  {
    try
    {
      var primaryColor = await SecureStorage.Default.GetAsync(SecureStorageType.PrimaryColor.ToString());
      var secondaryColor = await SecureStorage.Default.GetAsync(SecureStorageType.SecondaryColor.ToString());
      var logoUrl = await SecureStorage.Default.GetAsync(SecureStorageType.LogoUrl.ToString());
      if (Application.Current?.Resources != null)
      {
        if (!string.IsNullOrEmpty(primaryColor))
        {
          Application.Current.Resources["PrimaryColor"] = Color.FromArgb(primaryColor);
          // Dynamically choose black or white based on contrast
          Application.Current.Resources["PrimaryTextColor"] = ColorHelper.GetBlackOrWhiteBasedOn(Color.FromArgb(primaryColor));
        }

        if (!string.IsNullOrEmpty(secondaryColor))
        {
          Application.Current.Resources["SecondaryColor"] = Color.FromArgb(secondaryColor);
          // Dynamically choose black or white based on contrast
          Application.Current.Resources["SecondaryTextColor"] = ColorHelper.GetBlackOrWhiteBasedOn(Color.FromArgb(secondaryColor));
        }

        if (!string.IsNullOrEmpty(logoUrl))
          Application.Current.Resources["LogoUrl"] = logoUrl;
      }

      var tenantUIRepsonse = await GET<TenantUI>("tenantui/public");
      if (tenantUIRepsonse is { Error: false, Data: TenantUI uiConfigs })
      {
        await SecureStorage.SetAsync(SecureStorageType.LogoUrl.ToString(), uiConfigs.LogoUrl);
        await SecureStorage.SetAsync(SecureStorageType.PrimaryColor.ToString(), uiConfigs.PrimaryColor);
        await SecureStorage.SetAsync(SecureStorageType.SecondaryColor.ToString(), uiConfigs.SecondaryColor);
        if (Application.Current?.Resources != null)
        {
          Application.Current.Resources["PrimaryColor"] = Color.FromArgb(uiConfigs.PrimaryColor);
          // Dynamically choose black or white based on contrast
          Application.Current.Resources["PrimaryTextColor"] = ColorHelper.GetBlackOrWhiteBasedOn(Color.FromArgb(uiConfigs.PrimaryColor));
          Application.Current.Resources["SecondaryColor"] = Color.FromArgb(uiConfigs.SecondaryColor);
          // Dynamically choose black or white based on contrast
          Application.Current.Resources["SecondaryTextColor"] = ColorHelper.GetBlackOrWhiteBasedOn(Color.FromArgb(uiConfigs.SecondaryColor));
          Application.Current.Resources["LogoUrl"] = uiConfigs.LogoUrl;
        }
        else
        {
          Debug.WriteLine($"Error obteniendo ui: {tenantUIRepsonse.Message}");
        }
      }
      else
      {
        Debug.WriteLine($"Error obteniendo ui: {tenantUIRepsonse.Message}");
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"Error obteniendo ui: {ex.Message}");
    }
  }

  public async Task LoadTenantParameters()
  {
    try
    {
      var response = await GET<TenantParameter[]>("generalParameter");
      if (response is { Error: false, Data: not null })
      {
        var json = JsonConvert.SerializeObject(response.Data);
        await SecureStorage.SetAsync(SecureStorageType.TenantParameters.ToString(), json);
      }
      else
      {
        Debug.WriteLine(response.Message);
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine(ex.Message);
    }
  }
  public async Task<int> GetTenantPointValue()
  {
    try
    {
      var response = await GET<TenantLoyaltyProgram>("loyaltyProgram");
      if (response is { Error: false, Data: not null })
      {
        return response.Data.PointsValue;
      }
      else
      {
        Debug.WriteLine(response.Message);
        return 0;
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine(ex.Message);
      return 0;
    }
  }
}
