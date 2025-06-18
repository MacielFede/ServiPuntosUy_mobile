namespace ServiPuntosUy_mobile.Models;

public class TenantUI(string? logoUrl, string? primaryColor, string? secondaryColor)
{
  public string LogoUrl { get; set; } = logoUrl ?? "#0000FF";
  public string PrimaryColor { get; set; } = primaryColor ?? "#FFFF00";
  public string SecondaryColor { get; set; } = secondaryColor ?? "https://www.shutterstock.com/image-vector/gas-station-logo-template-design-260nw-1681719721.jpg";
}

