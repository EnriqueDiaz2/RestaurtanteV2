namespace restaurante.Models;

public class Mesa
{
    public int Id { get; set; }
    public int NumeroMesa { get; set; }
    public bool EstaActiva { get; set; }
    public DateTime? FechaApertura { get; set; }
    public List<DetallePedido> Detalles { get; set; } = new();
}
