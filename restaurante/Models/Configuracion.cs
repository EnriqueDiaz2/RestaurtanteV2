namespace restaurante.Models;

public class Configuracion
{
    public int Id { get; set; }
    public string NombreRestaurante { get; set; } = "Mariscos El Delfín";
    public string Direccion { get; set; } = "Av. Principal #123";
    public string Telefono { get; set; } = "(555) 123-4567";
    
    // Configuración de impresora
    public string? ImpresoraSeleccionada { get; set; }
    public bool ImprimirDirectamente { get; set; } = false;
}
