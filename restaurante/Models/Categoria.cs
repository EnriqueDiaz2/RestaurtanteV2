namespace restaurante.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#3498db";
    public List<Platillo> Platillos { get; set; } = new();
}
