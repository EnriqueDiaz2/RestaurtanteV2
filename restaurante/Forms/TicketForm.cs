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
        this.Size = new DrawingSize(550, 800);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.BackColor = DrawingColor.FromArgb(236, 240, 241);

        Panel panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 70,
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
        panelHeader.Controls.Add(lblTitulo);

        Panel panelContenido = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),
            BackColor = DrawingColor.White
        };

        TextBox txtTicket = new TextBox
        {
            Name = "txtTicket",
            Dock = DockStyle.Fill,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new Font("Courier New", 10),
            ReadOnly = true,
            BackColor = DrawingColor.White,
            BorderStyle = BorderStyle.FixedSingle
        };
        panelContenido.Controls.Add(txtTicket);

        Panel panelBotones = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 100,
            BackColor = DrawingColor.White,
            Padding = new Padding(15)
        };

        Button btnImprimirFisico = new Button
        {
            Text = "??? IMPRIMIR",
            Location = new Point(15, 15),
            Size = new DrawingSize(155, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
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
            Location = new Point(180, 15),
            Size = new DrawingSize(155, 50),
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
            Location = new Point(345, 15),
            Size = new DrawingSize(155, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
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

        // Encabezado - Nombre del restaurante centrado
        ticket.AppendLine(CentrarTexto(config.NombreRestaurante, 40));
        ticket.AppendLine(CentrarTexto(config.Direccion, 40));
        ticket.AppendLine(CentrarTexto($"Tel: {config.Telefono}", 40));
        ticket.AppendLine();
        ticket.AppendLine("========================================");
        
        // Información de la mesa
        if (mesa.NumeroMesa == 0)
            ticket.AppendLine("MESA: PARA LLEVAR");
        else
            ticket.AppendLine($"MESA: {mesa.NumeroMesa}");

        ticket.AppendLine($"Mesero: {nombreMesero}");
        ticket.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
        
        ticket.AppendLine("----------------------------------------");
        
        // Encabezado de productos con espaciado correcto
        ticket.AppendLine("ART                    CANT    PRECIO");
        ticket.AppendLine("----------------------------------------");

        // Detalles de productos
        decimal subtotal = 0;
        foreach (var detalle in detalles)
        {
            var platillo = detalle.Platillo ?? db.Platillos.Find(detalle.PlatilloId);
            if (platillo != null)
            {
                decimal importe = detalle.Cantidad * detalle.PrecioUnitario;
                subtotal += importe;

                string nombrePlatillo = platillo.NombreCorto;
                if (nombrePlatillo.Length > 23)
                    nombrePlatillo = nombrePlatillo.Substring(0, 23);

                // Formato: Nombre (23 chars) + Cant (4 chars) + $ + Precio (7 chars)
                ticket.AppendLine($"{nombrePlatillo,-23}{detalle.Cantidad,4}  ${importe,7:F2}");
            }
        }

        ticket.AppendLine("----------------------------------------");

        // Totales alineados a la derecha
        ticket.AppendLine($"{"Subtotal:",30} ${subtotal,7:F2}");

        // IVA (0.00%)
        decimal iva = 0;
        ticket.AppendLine($"{"IVA (0.00%):",30} ${iva,7:F2}");

        // Descuento
        if (descuento > 0)
        {
            if (tipoDescuento == "%")
            {
                string descText = $"Descuento ({valorDescuento:F2}%):";
                ticket.AppendLine($"{descText,30} ${descuento,7:F2}");
            }
            else
            {
                ticket.AppendLine($"{"Descuento:",30} ${descuento,7:F2}");
            }
        }
        else
        {
            ticket.AppendLine($"{"Descuento (0.00%):",30} ${descuento,7:F2}");
        }

        ticket.AppendLine();
        ticket.AppendLine("========================================");
        
        // Total final centrado
        decimal total = subtotal + iva - descuento;
        string lineaTotal = $"TOTAL: $ {total:F2}";
        ticket.AppendLine(CentrarTexto(lineaTotal, 40));
        
        ticket.AppendLine("========================================");
        ticket.AppendLine();
        ticket.AppendLine(CentrarTexto("Gracias por su compra", 40));
        ticket.AppendLine();

        ticketContent = ticket.ToString();
        txtTicket.Text = ticketContent;
    }

    private string CentrarTexto(string texto, int ancho)
    {
        if (texto.Length >= ancho)
            return texto;

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
        Font font = new Font("Courier New", 9);
        float yPos = 20;
        float leftMargin = 10;
        
        string[] lines = ticketContent.Split('\n');
        
        foreach (string line in lines)
        {
            e.Graphics!.DrawString(line, font, Brushes.Black, leftMargin, yPos);
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
                page.Size(80, 250, Unit.Millimetre); // Tamaño de ticket térmico (80mm x 250mm)
                page.Margin(5, Unit.Millimetre);
                
                page.Content().Column(col =>
                {
                    // Encabezado
                    col.Item().AlignCenter().Text(config.NombreRestaurante)
                        .FontSize(12).Bold();
                    
                    col.Item().AlignCenter().Text(config.Direccion)
                        .FontSize(8);
                    
                    col.Item().AlignCenter().Text($"Tel: {config.Telefono}")
                        .FontSize(8);
                    
                    col.Item().PaddingVertical(3).LineHorizontal(1);
                    
                    // Información de la mesa
                    col.Item().Text(mesa.NumeroMesa == 0 ? "MESA: PARA LLEVAR" : $"MESA: {mesa.NumeroMesa}")
                        .FontSize(9).Bold();
                    
                    col.Item().Text($"Mesero: {nombreMesero}")
                        .FontSize(8);
                    
                    col.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(8);
                    
                    col.Item().PaddingVertical(2).LineHorizontal(0.5f);
                    
                    // Encabezado de tabla
                    col.Item().Row(row =>
                    {
                        row.RelativeItem(3).Text("ART").FontSize(8).Bold();
                        row.RelativeItem(1).AlignRight().Text("CANT").FontSize(8).Bold();
                        row.RelativeItem(2).AlignRight().Text("PRECIO").FontSize(8).Bold();
                    });
                    
                    col.Item().PaddingVertical(1).LineHorizontal(0.5f);
                    
                    // Productos
                    foreach (var detalle in detalles)
                    {
                        var platillo = detalle.Platillo ?? db.Platillos.Find(detalle.PlatilloId);
                        if (platillo != null)
                        {
                            decimal importe = detalle.Cantidad * detalle.PrecioUnitario;
                            
                            col.Item().Row(row =>
                            {
                                row.RelativeItem(3).Text(platillo.NombreCorto).FontSize(8);
                                row.RelativeItem(1).AlignRight().Text(detalle.Cantidad.ToString()).FontSize(8);
                                row.RelativeItem(2).AlignRight().Text($"${importe:F2}").FontSize(8);
                            });
                        }
                    }
                    
                    col.Item().PaddingVertical(2).LineHorizontal(0.5f);
                    
                    // Subtotales
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("Subtotal:").FontSize(8);
                        row.RelativeItem().AlignRight().Text($"${subtotal:F2}").FontSize(8);
                    });
                    
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("IVA (0.00%):").FontSize(8);
                        row.RelativeItem().AlignRight().Text("$0.00").FontSize(8);
                    });
                    
                    if (descuento > 0)
                    {
                        string textoDesc = tipoDescuento == "%" 
                            ? $"Descuento ({valorDescuento:F2}%):" 
                            : "Descuento:";
                        
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text(textoDesc).FontSize(8);
                            row.RelativeItem().AlignRight().Text($"${descuento:F2}").FontSize(8);
                        });
                    }
                    else
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Descuento (0.00%):").FontSize(8);
                            row.RelativeItem().AlignRight().Text("$0.00").FontSize(8);
                        });
                    }
                    
                    col.Item().PaddingVertical(3).LineHorizontal(1);
                    
                    // Total
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("TOTAL:").FontSize(11).Bold();
                        row.RelativeItem().AlignRight().Text($"${total:F2}").FontSize(11).Bold();
                    });
                    
                    col.Item().PaddingVertical(3).LineHorizontal(1);
                    
                    // Mensaje de despedida
                    col.Item().PaddingTop(5).AlignCenter().Text("Gracias por su compra")
                        .FontSize(9).Italic();
                });
            });
        }).GeneratePdf(rutaArchivo);
    }
}
