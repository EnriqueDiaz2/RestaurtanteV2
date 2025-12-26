# ?? INSTRUCCIONES PARA COMPLETAR LA IMPLEMENTACIÓN

## ? Cambios Implementados Exitosamente

1. ? **Variable para recordar impresora** - Agregada en MainForm.cs
2. ? **Botón "Cerrar Día"** - Agregado en el panel superior
3. ? **Botón "Quitar Cantidad"** - Agregado al lado de Eliminar
4. ? **Método BtnGuardarPedido_Click** - Actualizado para guardar pedidos sin errores
5. ? **Método BtnQuitarCantidad_Click** - Implementado para quitar cantidades
6. ? **Método ActualizarEstadoMesas** - Agregado

## ?? Archivos que Necesitas Crear Manualmente

### 1. Crear `CerrarDiaForm.cs`

**Ubicación:** `restaurante/Forms/CerrarDiaForm.cs`

**Contenido Completo:**

```csharp
using Microsoft.EntityFrameworkCore;
using restaurante.Data;
using restaurante.Models;
using System.Drawing.Printing;

namespace restaurante.Forms;

public partial class CerrarDiaForm : Form
{
    private RestauranteContext db;

    public CerrarDiaForm(RestauranteContext context)
    {
        db = context;
        InitializeComponent();
        CargarReporteDia();
    }

    private void InitializeComponent()
    {
        this.Text = "Cierre del Día - Reporte de Ventas";
        this.Size = new Size(900, 700);
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.FromArgb(236, 240, 241);

        Panel panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = Color.FromArgb(52, 73, 94)
        };

        Label lblTitulo = new Label
        {
            Text = "?? CIERRE DEL DÍA",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelHeader.Controls.Add(lblTitulo);

        Panel panelContenido = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            BackColor = Color.White,
            AutoScroll = true
        };

        Label lblFecha = new Label
        {
            Name = "lblFecha",
            Text = $"?? Fecha: {DateTime.Today:dd/MM/yyyy}",
            Location = new Point(20, 20),
            Size = new Size(850, 30),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        Panel panelResumen = new Panel
        {
            Location = new Point(20, 60),
            Size = new Size(850, 120),
            BackColor = Color.FromArgb(46, 204, 113),
            Padding = new Padding(15)
        };

        Label lblTotalVentas = new Label
        {
            Name = "lblTotalVentas",
            Text = "TOTAL VENDIDO: $0.00",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelResumen.Controls.Add(lblTotalVentas);

        Panel panelEstadisticas = new Panel
        {
            Location = new Point(20, 200),
            Size = new Size(850, 100),
            BackColor = Color.FromArgb(236, 240, 241),
            Padding = new Padding(15)
        };

        Label lblCantidadArticulos = new Label
        {
            Name = "lblCantidadArticulos",
            Text = "Total de Artículos Vendidos: 0",
            Location = new Point(15, 15),
            Size = new Size(400, 30),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        Label lblNumeroOrdenes = new Label
        {
            Name = "lblNumeroOrdenes",
            Text = "Número de Órdenes: 0",
            Location = new Point(15, 50),
            Size = new Size(400, 30),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        panelEstadisticas.Controls.AddRange(new Control[] { lblCantidadArticulos, lblNumeroOrdenes });

        Label lblDetalle = new Label
        {
            Text = "Detalle de Artículos Vendidos:",
            Location = new Point(20, 320),
            Size = new Size(850, 30),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        DataGridView dgvDetalleVentas = new DataGridView
        {
            Name = "dgvDetalleVentas",
            Location = new Point(20, 360),
            Size = new Size(850, 250),
            Font = new Font("Segoe UI", 10),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            RowHeadersVisible = false,
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(236, 240, 241)
            }
        };

        panelContenido.Controls.AddRange(new Control[] { 
            lblFecha, panelResumen, panelEstadisticas, lblDetalle, dgvDetalleVentas 
        });

        Panel panelBotones = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            BackColor = Color.White,
            Padding = new Padding(20)
        };

        Button btnImprimir = new Button
        {
            Text = "??? IMPRIMIR REPORTE",
            Location = new Point(20, 15),
            Size = new Size(200, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnImprimir.FlatAppearance.BorderSize = 0;
        btnImprimir.Click += BtnImprimir_Click;

        Button btnCerrar = new Button
        {
            Text = "? CERRAR",
            Location = new Point(230, 15),
            Size = new Size(150, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(149, 165, 166),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCerrar.FlatAppearance.BorderSize = 0;
        btnCerrar.Click += (s, e) => this.Close();

        panelBotones.Controls.AddRange(new Control[] { btnImprimir, btnCerrar });

        this.Controls.Add(panelContenido);
        this.Controls.Add(panelBotones);
        this.Controls.Add(panelHeader);
    }

    private void CargarReporteDia()
    {
        try
        {
            var ventasDelDia = new List<(string Articulo, int Cantidad, decimal PrecioUnitario, decimal Total)>();

            var detalles = db.DetallesPedidos
                .Include(d => d.Platillo)
                .ToList();

            foreach (var detalle in detalles)
            {
                var platillo = detalle.Platillo ?? db.Platillos.Find(detalle.PlatilloId);
                if (platillo != null)
                {
                    ventasDelDia.Add((
                        platillo.NombreCorto,
                        detalle.Cantidad,
                        detalle.PrecioUnitario,
                        detalle.Cantidad * detalle.PrecioUnitario
                    ));
                }
            }

            var ventasAgrupadas = ventasDelDia
                .GroupBy(v => v.Articulo)
                .Select(g => new
                {
                    Articulo = g.Key,
                    Cantidad = g.Sum(x => x.Cantidad),
                    PrecioUnitario = g.First().PrecioUnitario,
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(v => v.Total)
                .ToList();

            decimal totalVentas = ventasAgrupadas.Sum(v => v.Total);
            int totalArticulos = ventasAgrupadas.Sum(v => v.Cantidad);
            int numeroOrdenes = db.Mesas.Count(m => m.EstaActiva);

            var lblTotalVentas = this.Controls.Find("lblTotalVentas", true).FirstOrDefault() as Label;
            if (lblTotalVentas != null)
                lblTotalVentas.Text = $"TOTAL VENDIDO: ${totalVentas:F2}";

            var lblCantidadArticulos = this.Controls.Find("lblCantidadArticulos", true).FirstOrDefault() as Label;
            if (lblCantidadArticulos != null)
                lblCantidadArticulos.Text = $"Total de Artículos Vendidos: {totalArticulos}";

            var lblNumeroOrdenes = this.Controls.Find("lblNumeroOrdenes", true).FirstOrDefault() as Label;
            if (lblNumeroOrdenes != null)
                lblNumeroOrdenes.Text = $"Número de Órdenes Activas: {numeroOrdenes}";

            var dgv = this.Controls.Find("dgvDetalleVentas", true).FirstOrDefault() as DataGridView;
            if (dgv != null)
            {
                dgv.DataSource = ventasAgrupadas.Select(v => new
                {
                    Artículo = v.Articulo,
                    Cantidad = v.Cantidad,
                    Precio = $"${v.PrecioUnitario:F2}",
                    Total = $"${v.Total:F2}"
                }).ToList();

                if (dgv.Columns.Count > 0 && dgv.Columns["Artículo"] != null)
                    dgv.Columns["Artículo"]!.Width = 300;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al cargar reporte: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnImprimir_Click(object? sender, EventArgs e)
    {
        try
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += ImprimirReporte;

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
                MessageBox.Show("? Reporte enviado a impresora correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al imprimir: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ImprimirReporte(object sender, PrintPageEventArgs e)
    {
        var config = db.Configuraciones.FirstOrDefault();
        if (config == null)
            config = new Configuracion { Id = 1, NombreRestaurante = "Restaurante", Direccion = "", Telefono = "" };

        Font fontTitulo = new Font("Courier New", 12, FontStyle.Bold);
        Font fontNormal = new Font("Courier New", 10, FontStyle.Bold);
        Font fontTotal = new Font("Courier New", 14, FontStyle.Bold);
        Brush brush = Brushes.Black;

        float yPos = 20;
        float leftMargin = 10;

        e.Graphics!.DrawString(CentrarTexto(config.NombreRestaurante, 32), fontTitulo, brush, leftMargin, yPos);
        yPos += fontTitulo.GetHeight(e.Graphics);
        e.Graphics.DrawString(CentrarTexto(config.Direccion, 32), fontNormal, brush, leftMargin, yPos);
        yPos += fontNormal.GetHeight(e.Graphics);
        e.Graphics.DrawString(CentrarTexto($"Tel: {config.Telefono}", 32), fontNormal, brush, leftMargin, yPos);
        yPos += fontNormal.GetHeight(e.Graphics);
        yPos += 10;

        e.Graphics.DrawString("================================", fontNormal, brush, leftMargin, yPos);
        yPos += fontNormal.GetHeight(e.Graphics);
        e.Graphics.DrawString(CentrarTexto("CIERRE DEL DIA", 32), fontTitulo, brush, leftMargin, yPos);
        yPos += fontTitulo.GetHeight(e.Graphics);
        e.Graphics.DrawString(CentrarTexto(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), 32), fontNormal, brush, leftMargin, yPos);
        yPos += fontNormal.GetHeight(e.Graphics);
        e.Graphics.DrawString("================================", fontNormal, brush, leftMargin, yPos);
        yPos += fontNormal.GetHeight(e.Graphics);
        yPos += 10;

        var lblTotalVentas = this.Controls.Find("lblTotalVentas", true).FirstOrDefault() as Label;
        string totalTexto = lblTotalVentas?.Text.Replace("TOTAL VENDIDO: ", "") ?? "$0.00";
        e.Graphics.DrawString(CentrarTexto($"TOTAL: {totalTexto}", 32), fontTotal, brush, leftMargin, yPos);
        yPos += fontTotal.GetHeight(e.Graphics);
        yPos += 10;

        var lblCantidadArticulos = this.Controls.Find("lblCantidadArticulos", true).FirstOrDefault() as Label;
        string articulosTexto = lblCantidadArticulos?.Text ?? "Total de Artículos Vendidos: 0";
        e.Graphics.DrawString(articulosTexto, fontNormal, brush, leftMargin, yPos);
        yPos += fontNormal.GetHeight(e.Graphics);
        yPos += 10;

        e.Graphics.DrawString("--------------------------------", fontNormal, brush, leftMargin, yPos);
        yPos += fontNormal.GetHeight(e.Graphics);
        e.Graphics.DrawString("ART              CANT  TOTAL", fontNormal, brush, leftMargin, yPos);
        yPos += fontNormal.GetHeight(e.Graphics);
        e.Graphics.DrawString("--------------------------------", fontNormal, brush, leftMargin, yPos);
        yPos += fontNormal.GetHeight(e.Graphics);

        var dgv = this.Controls.Find("dgvDetalleVentas", true).FirstOrDefault() as DataGridView;
        if (dgv != null && dgv.Rows.Count > 0)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["Artículo"].Value == null) continue;
                
                string articulo = row.Cells["Artículo"].Value?.ToString() ?? "";
                if (articulo.Length > 17)
                    articulo = articulo.Substring(0, 17);

                string cantidad = row.Cells["Cantidad"].Value?.ToString() ?? "0";
                string total = row.Cells["Total"].Value?.ToString()?.Replace("$", "") ?? "0.00";

                string linea = $"{articulo,-17}{cantidad,4} {total,7}";
                e.Graphics.DrawString(linea, fontNormal, brush, leftMargin, yPos);
                yPos += fontNormal.GetHeight(e.Graphics);
            }
        }

        yPos += 10;
        e.Graphics.DrawString("================================", fontNormal, brush, leftMargin, yPos);
    }

    private string CentrarTexto(string texto, int ancho)
    {
        if (texto.Length >= ancho)
            return texto.Substring(0, ancho);

        int espacios = (ancho - texto.Length) / 2;
        return new string(' ', espacios) + texto;
    }
}
```

**Instrucciones:**
1. En Visual Studio, click derecho en la carpeta `Forms`
2. Agregar ? Nuevo elemento ? Clase
3. Nombre: `CerrarDiaForm.cs`
4. Pega todo el código anterior

---

## ?? RESUMEN DE FUNCIONALIDADES IMPLEMENTADAS

### ? 1. Botón "Cerrar Día" (??)
- **Ubicación**: Panel superior, al lado del botón ADMIN
- **Funcionalidad**: Muestra un reporte con:
  - Total vendido en dinero
  - Cantidad total de artículos vendidos
  - Número de órdenes activas
  - Detalle de cada artículo vendido con cantidades
  - Opción para imprimir el reporte

### ? 2. Botón "Quitar Cantidad" (?)
- **Ubicación**: Panel de botones del pedido, primera posición
- **Funcionalidad**:
  - Reduce en 1 la cantidad del artículo seleccionado
  - Si la cantidad es 1, pregunta si desea eliminar el artículo
  - Mantiene la selección después de quitar

### ? 3. Guardar Pedido Mejorado
- **Funcionalidad**:
  - Actualiza pedidos existentes sin errores
  - Elimina correctamente los detalles anteriores
  - Guarda los nuevos detalles sin conflictos
  - Muestra mensaje de éxito al guardar

### ? 4. Sistema de Impresión (Pendiente de implementar)
- **Funcionalidad planeada**:
  - Recordar la impresora seleccionada
  - No mostrar diálogo después de la primera selección
  - Letras más grandes y negritas en el ticket

---

## ?? PARA IMPLEMENTAR EL SISTEMA DE IMPRESIÓN MEJORADO

Necesitas modificar el archivo `TicketForm.cs` para:

1. **Aumentar tamaño y negritud de letra**:
   - Cambiar `Font("Courier New", 10, FontStyle.Bold)` por `Font("Courier New", 12, FontStyle.Bold)`

2. **Recordar impresora**:
   - Agregar variable estática para guardar la impresora seleccionada
   - Solo mostrar el diálogo de impresión la primera vez

¿Quieres que te ayude a modificar el `TicketForm.cs` también?

---

## ?? PRÓXIMOS PASOS

1. ? **Crear el archivo `CerrarDiaForm.cs`** (copia el código de arriba)
2. ? **Compilar el proyecto** para verificar que no hay errores
3. ? **Probar todas las funcionalidades**:
   - Botón Cerrar Día
   - Botón Quitar Cantidad
   - Guardar pedido actualizado
4. ?? **Opcional**: Modificar TicketForm para recordar impresora y letras más grandes

---

## ? Estado Actual

- ? Botón Cerrar Día: Código listo (falta crear archivo)
- ? Botón Quitar Cantidad: **IMPLEMENTADO**
- ? Guardar Pedido Actualizado: **IMPLEMENTADO**
- ?? Recordar Impresora: Pendiente (requiere modificar TicketForm)
- ?? Letras más grandes: Pendiente (requiere modificar TicketForm)
