namespace ServiPuntosUy_mobile.Models;

public class ApiResponse<T>(bool error, T? data, string message)
{
  public bool Error { get; set; } = error;
  public T? Data { get; set; } = data;
  public string Message { get; set; } = message;
}