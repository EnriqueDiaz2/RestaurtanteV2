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
            
            // Recargar configuración para asegurar que tiene los últimos valores
            var configActualizada = db.Configuraciones.FirstOrDefault();
            if (configActualizada != null)
            {
                config = configActualizada;
            }
            
            // Verificar si ya hay una impresora configurada (con protección contra null)
            bool tieneImpresoraConfigurada = false;
            try
            {
                tieneImpresoraConfigurada = !string.IsNullOrEmpty(config?.ImpresoraSeleccionada) && 
                                            (config?.ImprimirDirectamente ?? false);
            }
            catch
            {
                // Si hay error al acceder a las propiedades, asumimos que no están configuradas
                tieneImpresoraConfigurada = false;
            }
            
            if (tieneImpresoraConfigurada && config != null)
            {
                // Imprimir directamente sin mostrar diálogo
                ImprimirDirecto(document, config.ImpresoraSeleccionada!);
            }
            else
            {
                // Primera vez o usuario quiere cambiar - mostrar diálogo
                var resultado = MostrarDialogoImpresion(document);
                
                if (resultado.DialogResult == DialogResult.OK && resultado.ImpresoraSeleccionada != null)
                {
                    // Preguntar si quiere recordar esta configuración
                    var respuesta = MessageBox.Show(
                        "¿Desea usar siempre esta impresora sin mostrar este diálogo?\n\n" +
                        $"Impresora seleccionada: {resultado.ImpresoraSeleccionada}\n\n" +
                        "Puede cambiar esta configuración más tarde en la sección de Administración.",
                        "Recordar Impresora",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    
                    if (respuesta == DialogResult.Yes && config != null)
                    {
                        try
                        {
                            // Guardar la configuración
                            config.ImpresoraSeleccionada = resultado.ImpresoraSeleccionada;
                            config.ImprimirDirectamente = true;
                            db.SaveChanges();
                            
                            MessageBox.Show(
                                "? Configuración guardada.\n\n" +
                                "Los próximos tickets se imprimirán automáticamente en esta impresora.",
                                "Éxito",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        catch (Exception exSave)
                        {
                            MessageBox.Show(
                                $"? No se pudo guardar la configuración de impresora:\n{exSave.Message}\n\n" +
                                "La impresión se realizó correctamente, pero deberá seleccionar\n" +
                                "la impresora nuevamente la próxima vez.",
                                "Advertencia",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al imprimir ticket: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ImprimirDirecto(Document document, string nombreImpresora)
    {
        try
        {
            // Generar PDF en memoria
            byte[] pdfBytes = document.GeneratePdf();
            
            // Guardar temporalmente para imprimir
            string tempPath = Path.Combine(Path.GetTempPath(), $"ticket_{Guid.NewGuid()}.pdf");
            File.WriteAllBytes(tempPath, pdfBytes);
            
            // Configurar e imprimir
            using (PrintDocument pd = new PrintDocument())
            {
                pd.PrinterSettings.PrinterName = nombreImpresora;
                
                if (pd.PrinterSettings.IsValid)
                {
                    // Abrir el PDF con el visor predeterminado y enviarlo a imprimir
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = tempPath,
                        Verb = "print",
                        CreateNoWindow = true,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                    };
                    
                    var process = System.Diagnostics.Process.Start(psi);
                    
                    // Esperar un momento y luego cerrar
                    if (process != null)
                    {
                        System.Threading.Thread.Sleep(3000);
                        process.CloseMainWindow();
                        process.Close();
                    }
                    
                    MessageBox.Show(
                        $"? Ticket enviado a impresora: {nombreImpresora}",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    
                    // Limpiar archivo temporal después de un tiempo
                    Task.Delay(5000).ContinueWith(_ => 
                    {
                        try { File.Delete(tempPath); } catch { }
                    });
                }
                else
                {
                    throw new Exception($"La impresora '{nombreImpresora}' no está disponible.");
                }
            }
        }
        catch (Exception ex)
        {
            // Si falla la impresión directa, ofrecer mostrar el diálogo
            var respuesta = MessageBox.Show(
                $"No se pudo imprimir directamente:\n{ex.Message}\n\n" +
                "¿Desea seleccionar otra impresora?",
                "Error de Impresión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            
            if (respuesta == DialogResult.Yes)
            {
                // Resetear configuración y mostrar diálogo
                config!.ImprimirDirectamente = false;
                db.SaveChanges();
                MostrarDialogoImpresion(document);
            }
        }
    }

    private (DialogResult DialogResult, string? ImpresoraSeleccionada) MostrarDialogoImpresion(Document document)
    {
        try
        {
            // Generar PDF y mostrar diálogo de impresión
            byte[] pdfBytes = document.GeneratePdf();
            string tempPath = Path.Combine(Path.GetTempPath(), $"ticket_{Guid.NewGuid()}.pdf");
            File.WriteAllBytes(tempPath, pdfBytes);
            
            // Abrir con el visor predeterminado que mostrará el diálogo de impresión
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = tempPath,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
            
            // Preguntar al usuario qué impresora seleccionó
            var formConfig = new Form
            {
                Text = "Configurar Impresora",
                Size = new DrawingSize(450, 250),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            
            Label lblInfo = new Label
            {
                Text = "Seleccione la impresora que desea usar:",
                Location = new Point(20, 20),
                Size = new DrawingSize(400, 25),
                Font = new Font("Segoe UI", 10)
            };
            
            ComboBox cmbImpresoras = new ComboBox
            {
                Location = new Point(20, 50),
                Size = new DrawingSize(390, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            
            // Llenar con impresoras disponibles
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cmbImpresoras.Items.Add(printer);
            }
            
            // Seleccionar impresora predeterminada
            PrinterSettings ps = new PrinterSettings();
            cmbImpresoras.SelectedItem = ps.PrinterName;
            
            CheckBox chkRecordar = new CheckBox
            {
                Text = "Recordar esta impresora y no volver a preguntar",
                Location = new Point(20, 90),
                Size = new DrawingSize(400, 25),
                Font = new Font("Segoe UI", 9),
                Checked = false
            };
            
            Button btnAceptar = new Button
            {
                Text = "Aceptar",
                Location = new Point(160, 140),
                Size = new DrawingSize(120, 40),
                DialogResult = DialogResult.OK,
                BackColor = DrawingColor.FromArgb(52, 152, 219),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAceptar.FlatAppearance.BorderSize = 0;
            
            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(290, 140),
                Size = new DrawingSize(120, 40),
                DialogResult = DialogResult.Cancel,
                BackColor = DrawingColor.FromArgb(149, 165, 166),
                ForeColor = DrawingColor.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancelar.FlatAppearance.BorderSize = 0;
            
            formConfig.Controls.AddRange(new Control[] { lblInfo, cmbImpresoras, chkRecordar, btnAceptar, btnCancelar });
            formConfig.AcceptButton = btnAceptar;
            formConfig.CancelButton = btnCancelar;
            
            DialogResult resultado = formConfig.ShowDialog();
            
            if (resultado == DialogResult.OK && cmbImpresoras.SelectedItem != null)
            {
                string impresoraSeleccionada = cmbImpresoras.SelectedItem.ToString()!;
                
                if (chkRecordar.Checked)
                {
                    // Guardar configuración
                    config!.ImpresoraSeleccionada = impresoraSeleccionada;
                    config.ImprimirDirectamente = true;
                    db.SaveChanges();
                    
                    MessageBox.Show(
                        "? Configuración guardada.\n\n" +
                        $"Los próximos tickets se imprimirán automáticamente en: {impresoraSeleccionada}",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                
                return (DialogResult.OK, impresoraSeleccionada);
            }
            
            // Limpiar archivo temporal
            Task.Delay(5000).ContinueWith(_ => 
            {
                try { File.Delete(tempPath); } catch { }
            });
            
            return (DialogResult.Cancel, null);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al mostrar diálogo de impresión: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return (DialogResult.Cancel, null);
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
                page.Margin(0.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                // Aumentar tamaño base de fuente de 9 a 11 y usar negrita
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial").Bold());

                page.Content().Column(column =>
                {
                    column.Spacing(6);

                    // Encabezado - más grande y negrita
                    column.Item().AlignCenter().Text(config!.NombreRestaurante)
                        .FontSize(14).Bold();
                    column.Item().AlignCenter().Text(config.Direccion).FontSize(10).SemiBold();
                    column.Item().AlignCenter().Text(config.Telefono).FontSize(10).SemiBold();
                    column.Item().AlignCenter().Text("==============================").FontSize(10).Bold();
                    column.Item().PaddingVertical(3);

                    // Información de la mesa - más grande
                    string mesaTexto = mesa.NumeroMesa == 0 ? "PARA LLEVAR" : $"MESA #{mesa.NumeroMesa}";
                    column.Item().Text($"MESA: {mesaTexto}").FontSize(12).Bold();
                    column.Item().Text($"Mesero: {mesero}").FontSize(11).SemiBold();
                    column.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(11).SemiBold();
                    column.Item().Text("------------------------------").FontSize(10).Bold();

                    // Encabezados de tabla - más grande y negrita
                    column.Item().Text(text =>
                    {
                        text.Span($"{"ARTICULO",-15} {"CANT",5} {"PRECIO",8}").Bold().FontSize(11);
                    });
                    column.Item().Text("------------------------------").FontSize(10).Bold();

                    decimal subtotal = 0;
                    foreach (var detalle in pedido)
                    {
                        var platillo = detalle.Platillo ?? db.Platillos.Find(detalle.PlatilloId);
                        if (platillo != null)
                        {
                            string nombreCorto = platillo.NombreCorto.Length > 14 ?
                                platillo.NombreCorto.Substring(0, 12) + ".." :
                                platillo.NombreCorto;

                            // Productos con letra más grande y negrita
                            column.Item().Text($"{nombreCorto,-15} {detalle.Cantidad,5} ${detalle.PrecioUnitario,7:F2}")
                                .FontSize(11).Bold();
                            subtotal += detalle.Cantidad * detalle.PrecioUnitario;
                        }
                    }

                    column.Item().Text("------------------------------").FontSize(10).Bold();

                    // Totales - más grandes y negritas
                    column.Item().AlignRight().Text($"Subtotal: $ {subtotal,9:F2}")
                        .FontSize(11).Bold();
                    column.Item().AlignRight().Text($"IVA (0.00%): $ {0.00,9:F2}")
                        .FontSize(11).Bold();
                    
                    if (descuento > 0)
                    {
                        string descTexto = tipoDescuento == "%" ? $"{valorDescuento:F2}%" : $"{valorDescuento:F2}";
                        column.Item().AlignRight().Text($"Desc ({descTexto}): $ {descuento,9:F2}")
                            .FontSize(11).Bold();
                    }
                    else
                    {
                        column.Item().AlignRight().Text($"Desc (0.00%): $ {0.00,9:F2}")
                            .FontSize(11).Bold();
                    }

                    column.Item().Text("==============================").FontSize(10).Bold();

                    decimal total = subtotal - descuento;
                    // Total aún más grande y muy negrita
                    column.Item().AlignRight().Text($"TOTAL: $ {total,9:F2}")
                        .FontSize(14).Bold();
                    
                    column.Item().Text("==============================").FontSize(10).Bold();
                    column.Item().PaddingVertical(6);
                    column.Item().AlignCenter().Text("Gracias por su compra")
                        .FontSize(11).SemiBold().Italic();
                });
            });
        });
    }
}
