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
    if (value is decimal decimalValue)
    {
      return $"{CurrencySymbol}{decimalValue:N2}";
    }

    return value?.ToString() ?? "";
  }

  public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
