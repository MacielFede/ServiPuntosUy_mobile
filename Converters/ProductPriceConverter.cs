using System.Diagnostics;
using System.Globalization;
using ServiPuntosUy_mobile.Helpers;
using ServiPuntosUy_mobile.Models;

namespace ServiPuntosUy_mobile.Converters
{
  public class ProductPriceConverter : IValueConverter
  {
    private static string CurrencySymbol { get; set; } = "$";

    public static async Task InitializeCurrencySymbolAsync()
    {
      CurrencySymbol = await TenantParameterHelper.GetCurrencyLabelAsync();
    }

    private static string ApplyStrikethrough(string text)
    {
      return string.Concat(text.Select(c => c + "\u0336"));
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is null || value is not Product product)
        return "";

      string currency = CurrencySymbol;
      string priceText = product.OriginalPrice > 0 ? product.OriginalPrice.ToString("N2", culture) : product.Price.ToString("N2", culture);
      string promotionPriceText = product.PromotionalPrice.ToString("N2", culture);
      if (product.PromotionalPrice > 0 && product.PromotionalPrice < product.OriginalPrice)
      {
        priceText = ApplyStrikethrough(priceText);
      }
      else
      {
        promotionPriceText = "";
      }
      return $"{currency} {priceText} {promotionPriceText}";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
