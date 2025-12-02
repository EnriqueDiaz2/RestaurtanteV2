namespace restaurante.Models;

public class Platillo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string NombreCorto { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
}
