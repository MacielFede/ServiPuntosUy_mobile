using Microsoft.Extensions.Configuration;
using ServiPuntos.uy_mobile.Services.Interfaces;
using ServiPuntos.uy_mobile.Models;
using System.Diagnostics;
using ServiPuntos.uy_mobile.Models.Enums;
using ServiPuntos.uy_mobile.Helpers;

namespace ServiPuntos.uy_mobile.Services;

public class TenantService(IConfiguration configs) : ApiService(configs), ITenantService
{
  private readonly IConfiguration _config = configs;
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
          Debug.WriteLine($"ESTOY error obteniendo ui: {tenantUIRepsonse.Message}");
        }
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"Estoy : {ex.Message}");
    }
  }
}
