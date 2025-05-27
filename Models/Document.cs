namespace ServiPuntos.uy_mobile.Models;

public class Document(string documentNumber, string? serialNumber)
{
  public string DocumentNumber { get; set; } = documentNumber;
  public string? SerialNumber { get; set; } = serialNumber;
}
