using System;
using System.Globalization;
using ServiPuntosUy_mobile.Helpers;

namespace ServiPuntosUy_mobile.Converters;

public class CurrencyFormatConverter : IValueConverter
{
  private static string CurrencySymbol { get; set; } = "$";

  public static async Task InitializeCurrencySymbolAsync()
  {
    CurrencySymbol = await TenantParameterHelper.GetCurrencyLabelAsync();
  }

  public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    decimal decimalValue;
    switch (value)
    {
      case decimal d:
        decimalValue = d;
        break;
      case double dbl:
        decimalValue = (decimal)dbl;
        break;
      case int i:
        decimalValue = i;
        break;
      default:
        return value?.ToString() ?? "";
    }

    return $"{CurrencySymbol} {decimalValue:N2}";
  }

  public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
