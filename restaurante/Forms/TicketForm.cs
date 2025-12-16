using restaurante.Data;
using restaurante.Models;
using System.Drawing.Printing;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using DrawingSize = System.Drawing.Size;
using DrawingColor = System.Drawing.Color;

namespace restaurante.Forms;

public partial class TicketForm : Form
{
    private RestauranteContext db;
    private Mesa mesa;
    private List<DetallePedido> detalles;
    private decimal descuento;
    private decimal valorDescuento;
    private string tipoDescuento;
    private string nombreMesero;
    private string ticketContent = "";

    public TicketForm(RestauranteContext context, Mesa mesa, List<DetallePedido> detalles, 
        decimal descuento, decimal valorDescuento, string tipoDescuento, string nombreMesero)
    {
        this.db = context;
        this.mesa = mesa;
        this.detalles = detalles;
        this.descuento = descuento;
        this.valorDescuento = valorDescuento;
        this.tipoDescuento = tipoDescuento;
        this.nombreMesero = nombreMesero;
        
        // Configurar licencia de QuestPDF (Community license)
        QuestPDF.Settings.License = LicenseType.Community;
        
        InitializeComponent();
        GenerarTicket();
    }

    private void InitializeComponent()
    {
        this.Text = "Ticket de Venta";
        this.Size = new DrawingSize(480, 650);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.BackColor = DrawingColor.FromArgb(236, 240, 241);

        Panel panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = DrawingColor.FromArgb(52, 73, 94)
        };

        Label lblTitulo = new Label
        {
            Text = "?? TICKET DE VENTA",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = DrawingColor.White,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelHeader.Controls.Add(lblTitulo);

        Panel panelContenido = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            BackColor = DrawingColor.White
        };

        TextBox txtTicket = new TextBox
        {
            Name = "txtTicket",
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Courier New", 9),
            ReadOnly = true,
            BackColor = DrawingColor.White,
            BorderStyle = BorderStyle.FixedSingle
        };
        panelContenido.Controls.Add(txtTicket);

        Panel panelBotones = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            BackColor = DrawingColor.White,
            Padding = new Padding(12)
        };

        Button btnImprimirFisico = new Button
        {
            Text = "??? IMPRIMIR",
            Location = new Point(12, 15),
            Size = new DrawingSize(135, 42),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = DrawingColor.FromArgb(52, 152, 219),
            ForeColor = DrawingColor.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnImprimirFisico.FlatAppearance.BorderSize = 0;
        btnImprimirFisico.Click += BtnImprimirFisico_Click;

        Button btnGuardarPDF = new Button
        {
            Text = "?? GUARDAR PDF",
            Location = new Point(157, 15),
            Size = new DrawingSize(135, 42),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
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
            Location = new Point(302, 15),
            Size = new DrawingSize(135, 42),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = DrawingColor.FromArgb(149, 165, 166),
            ForeColor = DrawingColor.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCerrar.FlatAppearance.BorderSize = 0;
        btnCerrar.Click += (s, e) => this.Close();

        panelBotones.Controls.AddRange(new Control[] { btnImprimirFisico, btnGuardarPDF, btnCerrar });

        this.Controls.Add(panelContenido);
        this.Controls.Add(panelBotones);
        this.Controls.Add(panelHeader);
    }

    private void GenerarTicket()
    {
        var txtTicket = this.Controls.Find("txtTicket", true)[0] as TextBox;
        if (txtTicket == null) return;

        var config = db.Configuraciones.FirstOrDefault();
        if (config == null)
        {
            config = new Configuracion { Id = 1 };
        }

        StringBuilder ticket = new StringBuilder();

        // ANCHO OPTIMIZADO PARA IMPRESORA TÉRMICA: 32 caracteres
        int anchoTicket = 32;

        // Encabezado - Nombre del restaurante centrado
        ticket.AppendLine(CentrarTexto(config.NombreRestaurante, anchoTicket));
        ticket.AppendLine(CentrarTexto(config.Direccion, anchoTicket));
        ticket.AppendLine(CentrarTexto($"Tel: {config.Telefono}", anchoTicket));
        ticket.AppendLine();
        ticket.AppendLine("================================");
        
        // Información de la mesa
        if (mesa.NumeroMesa == 0)
            ticket.AppendLine("MESA: PARA LLEVAR");
        else
            ticket.AppendLine($"MESA: {mesa.NumeroMesa}");

        ticket.AppendLine($"Mesero: {nombreMesero}");
        ticket.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
        
        ticket.AppendLine("--------------------------------");
        
        // Encabezado de productos COMPACTO
        ticket.AppendLine("ART              CANT  PREC");
        ticket.AppendLine("--------------------------------");

        // Detalles de productos
        decimal subtotal = 0;
        foreach (var detalle in detalles)
        {
            var platillo = detalle.Platillo ?? db.Platillos.Find(detalle.PlatilloId);
            if (platillo != null)
            {
                decimal importe = detalle.Cantidad * detalle.PrecioUnitario;
                subtotal += importe;

                // ACORTAR NOMBRE (máximo 17 caracteres para evitar corte)
                string nombrePlatillo = platillo.NombreCorto;
                if (nombrePlatillo.Length > 17)
                    nombrePlatillo = nombrePlatillo.Substring(0, 17);

                // Formato COMPACTO: Nombre(17) + Cant(4) + Precio(8)
                // Total: 17 + 4 + 8 = 29 caracteres (cabe en 32)
                ticket.AppendLine($"{nombrePlatillo,-17}{detalle.Cantidad,4} ${importe,6:F2}");
            }
        }

        ticket.AppendLine("--------------------------------");

        // Totales alineados (ANCHO REDUCIDO: 24 + 7 = 31)
        ticket.AppendLine($"{"Subtotal:",24}${subtotal,7:F2}");

        // IVA (0.00%)
        decimal iva = 0;
        ticket.AppendLine($"{"IVA (0.00%):",24}${iva,7:F2}");

        // Descuento
        if (descuento > 0)
        {
            if (tipoDescuento == "%")
            {
                // Acortar "Descuento" a "Desc" para ahorrar espacio
                string descText = $"Desc ({valorDescuento:F2}%):";
                ticket.AppendLine($"{descText,24}${descuento,7:F2}");
            }
            else
            {
                ticket.AppendLine($"{"Descuento:",24}${descuento,7:F2}");
            }
        }
        else
        {
            ticket.AppendLine($"{"Desc (0.00%):",24}${descuento,7:F2}");
        }

        ticket.AppendLine();
        ticket.AppendLine("================================");
        
        // Total final centrado
        decimal total = subtotal + iva - descuento;
        string lineaTotal = $"TOTAL: $ {total:F2}";
        ticket.AppendLine(CentrarTexto(lineaTotal, anchoTicket));
        
        ticket.AppendLine("================================");
        ticket.AppendLine();
        ticket.AppendLine(CentrarTexto("Gracias por su compra", anchoTicket));
        ticket.AppendLine();

        ticketContent = ticket.ToString();
        txtTicket.Text = ticketContent;
    }

    private string CentrarTexto(string texto, int ancho)
    {
        // Si el texto es muy largo, recortarlo
        if (texto.Length >= ancho)
            return texto.Substring(0, ancho);

        int espacios = (ancho - texto.Length) / 2;
        return new string(' ', espacios) + texto;
    }

    private void BtnImprimirFisico_Click(object? sender, EventArgs e)
    {
        try
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(ImprimirTicket);
            
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;
            
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
                MessageBox.Show("? Ticket enviado a impresora correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al imprimir: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ImprimirTicket(object sender, PrintPageEventArgs e)
    {
        // USAR FUENTE MÁS GRANDE Y NEGRITA para mejor legibilidad
        Font font = new Font("Courier New", 10, FontStyle.Bold);
        Brush brush = Brushes.Black; // Color negro sólido
        
        float yPos = 10; // Margen superior reducido
        float leftMargin = 5; // Margen izquierdo MUY REDUCIDO para evitar corte
        
        string[] lines = ticketContent.Split('\n');
        
        foreach (string line in lines)
        {
            // Dibujar texto con fuente negrita y tinta negra sólida
            e.Graphics!.DrawString(line, font, brush, leftMargin, yPos);
            yPos += font.GetHeight(e.Graphics);
        }
    }

    private void BtnGuardarPDF_Click(object? sender, EventArgs e)
    {
        try
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Guardar Ticket como PDF",
                FileName = $"Ticket_Mesa{(mesa.NumeroMesa == 0 ? "ParaLlevar" : mesa.NumeroMesa.ToString())}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                GenerarPDF(saveDialog.FileName);
                
                var resultado = MessageBox.Show(
                    $"? PDF guardado exitosamente en:\n\n{saveDialog.FileName}\n\n¿Desea abrir el archivo?",
                    "PDF Generado",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (resultado == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = saveDialog.FileName,
                        UseShellExecute = true
                    });
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al guardar PDF: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void GenerarPDF(string rutaArchivo)
    {
        var config = db.Configuraciones.FirstOrDefault() ?? new Configuracion { Id = 1 };
        decimal subtotal = detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
        decimal total = subtotal - descuento;

        Document.Create(container =>
        {
            container.Page(page =>
            {
                // Ancho optimizado para ticket térmico
                page.Size(58, 250, Unit.Millimetre); // 58mm es estándar para tickets
                page.Margin(2, Unit.Millimetre); // Márgenes mínimos
                
                page.Content().Column(col =>
                {
                    // Encabezado con fuente más grande y NEGRITA
                    col.Item().AlignCenter().Text(config.NombreRestaurante)
                        .FontSize(11).Bold();
                    
                    col.Item().AlignCenter().Text(config.Direccion)
                        .FontSize(7);
                    
                    col.Item().AlignCenter().Text($"Tel: {config.Telefono}")
                        .FontSize(7);
                    
                    col.Item().PaddingVertical(2).LineHorizontal(1);
                    
                    // Información de la mesa CON NEGRITA
                    col.Item().Text(mesa.NumeroMesa == 0 ? "MESA: PARA LLEVAR" : $"MESA: {mesa.NumeroMesa}")
                        .FontSize(8).Bold();
                    
                    col.Item().Text($"Mesero: {nombreMesero}")
                        .FontSize(7).Bold();
                    
                    col.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(7).Bold();
                    
                    col.Item().PaddingVertical(1).LineHorizontal(0.5f);
                    
                    // Encabezado de tabla CON NEGRITA
                    col.Item().Row(row =>
                    {
                        row.RelativeItem(3).Text("ART").FontSize(7).Bold();
                        row.RelativeItem(1).AlignRight().Text("CANT").FontSize(7).Bold();
                        row.RelativeItem(2).AlignRight().Text("PRECIO").FontSize(7).Bold();
                    });
                    
                    col.Item().PaddingVertical(0.5f).LineHorizontal(0.5f);
                    
                    // Productos CON NEGRITA
                    foreach (var detalle in detalles)
                    {
                        var platillo = detalle.Platillo ?? db.Platillos.Find(detalle.PlatilloId);
                        if (platillo != null)
                        {
                            decimal importe = detalle.Cantidad * detalle.PrecioUnitario;
                            
                            // Acortar nombre si es muy largo
                            string nombre = platillo.NombreCorto.Length > 15 
                                ? platillo.NombreCorto.Substring(0, 15) 
                                : platillo.NombreCorto;
                            
                            col.Item().Row(row =>
                            {
                                row.RelativeItem(3).Text(nombre).FontSize(7).Bold();
                                row.RelativeItem(1).AlignRight().Text(detalle.Cantidad.ToString()).FontSize(7).Bold();
                                row.RelativeItem(2).AlignRight().Text($"${importe:F2}").FontSize(7).Bold();
                            });
                        }
                    }
                    
                    col.Item().PaddingVertical(1).LineHorizontal(0.5f);
                    
                    // Subtotales CON NEGRITA
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Subtotal:").FontSize(7).Bold();
                        row.RelativeItem().AlignRight().Text($"${subtotal:F2}").FontSize(7).Bold();
                    });
                    
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("IVA (0.00%):").FontSize(7).Bold();
                        row.RelativeItem().AlignRight().Text("$0.00").FontSize(7).Bold();
                    });
                    
                    if (descuento > 0)
                    {
                        string textoDesc = tipoDescuento == "%" 
                            ? $"Desc ({valorDescuento:F2}%):" 
                            : "Descuento:";
                        
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text(textoDesc).FontSize(7).Bold();
                            row.RelativeItem().AlignRight().Text($"${descuento:F2}").FontSize(7).Bold();
                        });
                    }
                    else
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Desc (0.00%):").FontSize(7).Bold();
                            row.RelativeItem().AlignRight().Text("$0.00").FontSize(7).Bold();
                        });
                    }
                    
                    col.Item().PaddingVertical(2).LineHorizontal(1);
                    
                    // Total CON FUENTE MÁS GRANDE Y NEGRITA
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("TOTAL:").FontSize(10).Bold();
                        row.RelativeItem().AlignRight().Text($"${total:F2}").FontSize(10).Bold();
                    });
                    
                    col.Item().PaddingVertical(2).LineHorizontal(1);
                    
                    // Mensaje de despedida
                    col.Item().PaddingTop(3).AlignCenter().Text("Gracias por su compra")
                        .FontSize(8).Bold().Italic();
                });
            });
        }).GeneratePdf(rutaArchivo);
    }
}
