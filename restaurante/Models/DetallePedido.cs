namespace restaurante.Models;

public class DetallePedido
{
    public int Id { get; set; }
    public int MesaId { get; set; }
    public Mesa? Mesa { get; set; }
    public int PlatilloId { get; set; }
    public Platillo? Platillo { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
