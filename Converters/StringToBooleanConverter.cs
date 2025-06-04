using System.Globalization;

namespace ServiPuntos.uy_mobile.Converters;

public class StringToBooleanConverter : IValueConverter
{
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    bool result = false;

    if (value is string text)
      result = !string.IsNullOrEmpty(text) || !string.IsNullOrWhiteSpace(text);

    if (parameter is string param && param == "invert")
      return !result;

    return result;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
