namespace restaurante.Models;

public class VentaCerrada
{
    public int Id { get; set; }
    public int NumeroMesa { get; set; }
    public DateTime FechaApertura { get; set; }
    public DateTime FechaCierre { get; set; }
    public string Mesero { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
    public List<DetalleVentaCerrada> Detalles { get; set; } = new();
}

public class DetalleVentaCerrada
{
    public int Id { get; set; }
    public int VentaCerradaId { get; set; }
    public VentaCerrada? VentaCerrada { get; set; }
    public string NombrePlatillo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Total { get; set; }
}
