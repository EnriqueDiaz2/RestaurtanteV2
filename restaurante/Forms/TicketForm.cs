using restaurante.Data;
using restaurante.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Drawing.Printing;
using System.Text;
using DrawingSize = System.Drawing.Size;
using DrawingColor = System.Drawing.Color;

namespace restaurante.Forms;

public class TicketForm : Form
{
    private RestauranteContext db;
    private Mesa mesa;
    private List<DetallePedido> pedido;
    private decimal descuento;
    private decimal valorDescuento;
    private string tipoDescuento;
    private string mesero;
    private TextBox txtTicket;
    private Configuracion? config;

    public TicketForm(RestauranteContext context, Mesa mesa, List<DetallePedido> pedido, decimal descuento, decimal valorDescuento, string tipoDescuento, string mesero)
    {
        this.db = context;
        this.mesa = mesa;
        this.pedido = pedido;
        this.descuento = descuento;
        this.valorDescuento = valorDescuento;
        this.tipoDescuento = tipoDescuento;
        this.mesero = mesero;
        
        // Cargar configuración
        config = db.Configuraciones.FirstOrDefault();
        if (config == null)
        {
            config = new Configuracion
            {
                NombreRestaurante = "Mariscos Pulido",
                Direccion = "F. Villa #22 Buenavista, Jal.",
                Telefono = "Tel: 3857333334"
            };
            db.Configuraciones.Add(config);
            db.SaveChanges();
        }
        
        QuestPDF.Settings.License = LicenseType.Community;
        InitializeComponent();
        GenerarVistaPrevia();
    }

    private void InitializeComponent()
    {
        this.Text = "Ticket de Venta";
        this.Size = new DrawingSize(450, 700);
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = DrawingColor.FromArgb(236, 240, 241);
        this.MinimumSize = new DrawingSize(400, 600);

        // Panel superior
        Panel panelTitulo = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = DrawingColor.FromArgb(52, 73, 94)
        };

        Label lblTitulo = new Label
        {
            Text = "?? TICKET DE VENTA",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = DrawingColor.White,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelTitulo.Controls.Add(lblTitulo);

        // Panel central con vista previa
        Panel panelCentral = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),
            BackColor = DrawingColor.FromArgb(236, 240, 241)
        };

        txtTicket = new TextBox
        {
            Dock = DockStyle.Fill,
            BackColor = DrawingColor.White,
            Font = new Font("Courier New", 9, FontStyle.Regular),
            ReadOnly = true,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            BorderStyle = BorderStyle.None,
            Padding = new Padding(10),
            WordWrap = false
        };

        panelCentral.Controls.Add(txtTicket);

        // Panel de botones
        Panel panelBotones = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            BackColor = DrawingColor.FromArgb(52, 73, 94),
            Padding = new Padding(15)
        };

        Button btnImprimir = new Button
        {
            Text = "??? IMPRIMIR",
            Location = new Point(15, 15),
            Size = new DrawingSize(130, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = DrawingColor.FromArgb(52, 152, 219),
            ForeColor = DrawingColor.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnImprimir.FlatAppearance.BorderSize = 0;
        btnImprimir.Click += BtnImprimir_Click;

        Button btnGuardarPDF = new Button
        {
            Text = "?? GUARDAR PDF",
            Location = new Point(155, 15),
            Size = new DrawingSize(145, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = DrawingColor.FromArgb(46, 204, 113),
            ForeColor = DrawingColor.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnGuardarPDF.FlatAppearance.BorderSize = 0;
        btnGuardarPDF.Click += BtnGuardarPDF_Click;

        Button btnCerrar = new Button
        {
            Text = "? CERRAR",
            Location = new Point(310, 15),
            Size = new DrawingSize(110, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = DrawingColor.FromArgb(149, 165, 166),
            ForeColor = DrawingColor.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCerrar.FlatAppearance.BorderSize = 0;
        btnCerrar.Click += (s, e) => this.Close();

        panelBotones.Controls.AddRange(new Control[] { btnImprimir, btnGuardarPDF, btnCerrar });

        // Agregar controles al formulario
        this.Controls.Add(panelCentral);
        this.Controls.Add(panelBotones);
        this.Controls.Add(panelTitulo);
    }

    private void GenerarVistaPrevia()
    {
        StringBuilder sb = new StringBuilder();
        
        // Centrar nombre del restaurante
        sb.AppendLine(CentrarTexto(config!.NombreRestaurante, 38));
        sb.AppendLine(CentrarTexto(config.Direccion, 38));
        sb.AppendLine(CentrarTexto(config.Telefono, 38));
        sb.AppendLine("======================================");
        sb.AppendLine();

        // Mesa e información
        string mesaTexto = mesa.NumeroMesa == 0 ? "PARA LLEVAR" : $"MESA #{mesa.NumeroMesa}";
        sb.AppendLine($"MESA: {mesaTexto}");
        sb.AppendLine($"Mesero: {mesero}");
        sb.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
        sb.AppendLine("--------------------------------------");
        
        // Encabezados de tabla
        sb.AppendLine($"{"ART",-20} {"CANT",4} {"PREC",9}");
        sb.AppendLine("--------------------------------------");

        // Detalle de productos
        decimal subtotal = 0;
        foreach (var detalle in pedido)
        {
            var platillo = detalle.Platillo ?? db.Platillos.Find(detalle.PlatilloId);
            if (platillo != null)
            {
                string nombreCorto = platillo.NombreCorto.Length > 19 ? 
                    platillo.NombreCorto.Substring(0, 16) + "..." : 
                    platillo.NombreCorto;
                
                sb.AppendLine($"{nombreCorto,-20} {detalle.Cantidad,4} ${detalle.PrecioUnitario,7:F2}");
                subtotal += detalle.Cantidad * detalle.PrecioUnitario;
            }
        }

        sb.AppendLine("--------------------------------------");

        // Totales alineados a la derecha
        sb.AppendLine($"{"Subtotal:$",28} {subtotal,9:F2}");
        sb.AppendLine($"{"IVA (0.00%):$",28} {0.00,9:F2}");
        
        if (descuento > 0)
        {
            string descTexto = tipoDescuento == "%" ? $"{valorDescuento:F2}%" : $"{valorDescuento:F2}";
            sb.AppendLine($"{$"Desc ({descTexto}):$",28} {descuento,9:F2}");
        }
        else
        {
            sb.AppendLine($"{"Desc (0.00%):$",28} {0.00,9:F2}");
        }

        sb.AppendLine("======================================");

        // Total
        decimal total = subtotal - descuento;
        sb.AppendLine($"{"TOTAL: $",28} {total,9:F2}");
        sb.AppendLine("======================================");
        sb.AppendLine();
        sb.AppendLine(CentrarTexto("Gracias por su compra", 38));

        txtTicket.Text = sb.ToString();
    }

    private string CentrarTexto(string texto, int ancho)
    {
        if (texto.Length >= ancho) return texto;
        int espacios = (ancho - texto.Length) / 2;
        return new string(' ', espacios) + texto;
    }

    private void BtnImprimir_Click(object? sender, EventArgs e)
    {
        try
        {
            var document = GenerarDocumentoPDF();
            
            // Generar PDF temporal
            byte[] pdfBytes = document.GeneratePdf();
            string tempPath = Path.Combine(Path.GetTempPath(), $"ticket_{DateTime.Now:yyyyMMddHHmmss}.pdf");
            File.WriteAllBytes(tempPath, pdfBytes);
            
            // Abrir con el visor predeterminado para que muestre el diálogo de impresión
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = tempPath,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
            
            MessageBox.Show(
                "? Ticket generado correctamente.\n\n" +
                "El PDF se ha abierto. Para imprimir:\n" +
                "1. Presione Ctrl+P (o haga clic en Imprimir)\n" +
                "2. Seleccione su impresora\n" +
                "3. Haga clic en Aceptar",
                "Ticket Listo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al generar ticket: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnGuardarPDF_Click(object? sender, EventArgs e)
    {
        try
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Archivos PDF (*.pdf)|*.pdf",
                FileName = $"Ticket_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                DefaultExt = "pdf"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var document = GenerarDocumentoPDF();
                document.GeneratePdf(saveDialog.FileName);
                
                MessageBox.Show("? Ticket guardado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al guardar PDF: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Document GenerarDocumentoPDF()
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A7);
                page.Margin(0.25f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial").SemiBold());

                page.Content().Column(column =>
                {
                    column.Spacing(1.5f); // Espaciado reducido pero manteniendo legibilidad

                    // Encabezado centrado
                    column.Item().AlignCenter().Text(config!.NombreRestaurante)
                        .FontSize(10).Bold();
                    column.Item().AlignCenter().Text(config.Direccion).FontSize(7).SemiBold();
                    column.Item().AlignCenter().Text(config.Telefono).FontSize(7).SemiBold();
                    
                    column.Item().PaddingVertical(1);
                    column.Item().BorderBottom(1).BorderColor(Colors.Black);
                    column.Item().PaddingVertical(1);

                    // Información de la mesa
                    string mesaTexto = mesa.NumeroMesa == 0 ? "PARA LLEVAR" : $"MESA #{mesa.NumeroMesa}";
                    column.Item().Text($"MESA: {mesaTexto}").FontSize(9).Bold();
                    column.Item().Text($"Mesero: {mesero}").FontSize(8).SemiBold();
                    column.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8).SemiBold();
                    
                    column.Item().PaddingVertical(1);
                    column.Item().BorderBottom(1).BorderColor(Colors.Black);
                    column.Item().PaddingVertical(1);

                    // Calcular subtotal
                    decimal subtotal = 0;
                    foreach (var detalle in pedido)
                    {
                        subtotal += detalle.Cantidad * detalle.PrecioUnitario;
                    }

                    // Tabla de productos
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Artículo
                            columns.RelativeColumn(1); // Cantidad
                            columns.RelativeColumn(1.5f); // Precio
                        });

                        // Encabezados
                        table.Cell().Text("ARTICULO").FontSize(8).Bold();
                        table.Cell().AlignRight().Text("CNT").FontSize(8).Bold();
                        table.Cell().AlignRight().Text("PRECIO").FontSize(8).Bold();

                        // Línea separadora
                        table.Cell().ColumnSpan(3).PaddingVertical(0.5f).BorderBottom(1).BorderColor(Colors.Black);

                        // Productos
                        foreach (var detalle in pedido)
                        {
                            var platillo = detalle.Platillo ?? db.Platillos.Find(detalle.PlatilloId);
                            if (platillo != null)
                            {
                                string nombreCorto = platillo.NombreCorto.Length > 15 ?
                                    platillo.NombreCorto.Substring(0, 13) + ".." :
                                    platillo.NombreCorto;

                                table.Cell().Text(nombreCorto).FontSize(8).SemiBold();
                                table.Cell().AlignRight().Text(detalle.Cantidad.ToString()).FontSize(8).SemiBold();
                                table.Cell().AlignRight().Text($"$ {detalle.PrecioUnitario:F2}").FontSize(8).SemiBold();
                            }
                        }
                    });

                    column.Item().PaddingVertical(1);
                    column.Item().BorderBottom(1).BorderColor(Colors.Black);
                    column.Item().PaddingVertical(1);

                    // Totales
                    column.Item().AlignRight().Text($"Subtotal: $ {subtotal:F2}")
                        .FontSize(8).SemiBold();
                    column.Item().AlignRight().Text($"IVA (0.00%): $ {0.00:F2}")
                        .FontSize(8).SemiBold();
                    
                    if (descuento > 0)
                    {
                        string descTexto = tipoDescuento == "%" ? $"{valorDescuento:F2}%" : $"{valorDescuento:F2}";
                        column.Item().AlignRight().Text($"Desc ({descTexto}): $ {descuento:F2}")
                            .FontSize(8).SemiBold();
                    }
                    else
                    {
                        column.Item().AlignRight().Text($"Desc (0.00%): $ {0.00:F2}")
                            .FontSize(8).SemiBold();
                    }

                    column.Item().PaddingVertical(1);
                    column.Item().BorderBottom(2).BorderColor(Colors.Black);
                    column.Item().PaddingVertical(1);

                    // Total
                    decimal total = subtotal - descuento;
                    column.Item().AlignCenter().Text($"TOTAL: $ {total:F2}")
                        .FontSize(12).Bold();
                    
                    column.Item().PaddingVertical(1);
                    column.Item().BorderBottom(2).BorderColor(Colors.Black);
                    column.Item().PaddingVertical(1.5f);
                    
                    // Mensaje final
                    column.Item().AlignCenter().Text("Gracias por su compra")
                        .FontSize(8).SemiBold().Italic();
                });
            });
        });
    }
}
