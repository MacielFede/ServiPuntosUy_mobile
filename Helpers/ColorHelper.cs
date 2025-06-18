namespace ServiPuntosUy_mobile.Helpers;

public static class ColorHelper
{
  public static Color GetBlackOrWhiteBasedOn(Color background)
  {
    // Luminance formula (perceived brightness)
    double brightness = 0.299 * background.Red +
                        0.587 * background.Green +
                        0.114 * background.Blue;

    return brightness > 0.5 ? Colors.Black : Colors.White;
  }
}