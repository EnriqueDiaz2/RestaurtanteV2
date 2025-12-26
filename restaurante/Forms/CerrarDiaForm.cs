using Microsoft.EntityFrameworkCore;
using restaurante.Data;
using restaurante.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using DrawingSize = System.Drawing.Size;
using DrawingColor = System.Drawing.Color;

namespace restaurante.Forms;

public class CerrarDiaForm : Form
{
    private RestauranteContext db;
    private DateTime fechaSeleccionada;
    private DataGridView dgvReporte;
    private Label lblTotalVentas;
    private Label lblTotalPedidos;
    private Label lblCantidadVentas;
    private DateTimePicker dtpFecha;
    private Configuracion? config;

    public CerrarDiaForm(RestauranteContext context)
    {
        db = context;
        fechaSeleccionada = DateTime.Today;
        
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
        }
        
        QuestPDF.Settings.License = LicenseType.Community;
        InitializeComponent();
        CargarReporte();
    }

    private void InitializeComponent()
    {
        this.Text = "?? Reporte del Día";
        this.Size = new DrawingSize(1000, 700);
        this.StartPosition = FormStartPosition.CenterParent;
        this.MinimumSize = new DrawingSize(900, 600);
        this.BackColor = DrawingColor.FromArgb(236, 240, 241);

        // Panel superior
        Panel panelSuperior = new Panel
        {
            Dock = DockStyle.Top,
            Height = 100,
            BackColor = DrawingColor.FromArgb(52, 73, 94),
            Padding = new Padding(20)
        };

        Label lblTitulo = new Label
        {
            Text = "?? REPORTE DE VENTAS DEL DÍA",
            Location = new Point(20, 15),
            AutoSize = false,
            Size = new DrawingSize(500, 35),
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = DrawingColor.White
        };

        Label lblFechaLabel = new Label
        {
            Text = "Fecha:",
            Location = new Point(20, 55),
            Size = new DrawingSize(60, 25),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = DrawingColor.White
        };

        dtpFecha = new DateTimePicker
        {
            Location = new Point(85, 53),
            Size = new DrawingSize(200, 25),
            Font = new Font("Segoe UI", 10),
            Format = DateTimePickerFormat.Short
        };
        dtpFecha.ValueChanged += (s, e) =>
        {
            fechaSeleccionada = dtpFecha.Value.Date;
            CargarReporte();
        };

        Button btnBuscar = new Button
        {
            Text = "?? Buscar",
            Location = new Point(295, 51),
            Size = new DrawingSize(100, 29),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = DrawingColor.FromArgb(46, 204, 113),
            ForeColor = DrawingColor.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnBuscar.FlatAppearance.BorderSize = 0;
        btnBuscar.Click += (s, e) => CargarReporte();

        Button btnImprimir = new Button
        {
            Text = "??? Imprimir Reporte",
            Location = new Point(410, 51),
            Size = new DrawingSize(160, 29),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = DrawingColor.FromArgb(52, 152, 219),
            ForeColor = DrawingColor.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnImprimir.FlatAppearance.BorderSize = 0;
        btnImprimir.Click += BtnImprimir_Click;

        panelSuperior.Controls.AddRange(new Control[] { lblTitulo, lblFechaLabel, dtpFecha, btnBuscar, btnImprimir });

        // Panel central con DataGridView
        Panel panelCentral = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            BackColor = DrawingColor.FromArgb(236, 240, 241)
        };

        dgvReporte = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = DrawingColor.White,
            BorderStyle = BorderStyle.None,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false,
            Font = new Font("Segoe UI", 10),
            ColumnHeadersHeight = 40
        };

        // Configurar estilo del DataGridView
        dgvReporte.EnableHeadersVisualStyles = false;
        dgvReporte.ColumnHeadersDefaultCellStyle.BackColor = DrawingColor.FromArgb(52, 73, 94);
        dgvReporte.ColumnHeadersDefaultCellStyle.ForeColor = DrawingColor.White;
        dgvReporte.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        dgvReporte.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvReporte.AlternatingRowsDefaultCellStyle.BackColor = DrawingColor.FromArgb(248, 249, 250);
        dgvReporte.DefaultCellStyle.SelectionBackColor = DrawingColor.FromArgb(52, 152, 219);
        dgvReporte.DefaultCellStyle.SelectionForeColor = DrawingColor.White;

        // Agregar columnas
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Hora",
            HeaderText = "Hora Cierre",
            Width = 90,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Mesa",
            HeaderText = "Mesa",
            Width = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Mesero",
            HeaderText = "Mesero",
            Width = 120
        });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Platillo",
            HeaderText = "Platillo",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Cantidad",
            HeaderText = "Cant.",
            Width = 70,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "PrecioUnitario",
            HeaderText = "P. Unit.",
            Width = 100,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }
        });
        dgvReporte.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "Total",
            HeaderText = "Total",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "C2" }
        });

        panelCentral.Controls.Add(dgvReporte);

        // Panel inferior con totales
        Panel panelInferior = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 150,
            BackColor = DrawingColor.FromArgb(52, 73, 94),
            Padding = new Padding(20)
        };

        lblCantidadVentas = new Label
        {
            Text = "Ventas cerradas: 0",
            Location = new Point(20, 15),
            AutoSize = false,
            Size = new DrawingSize(400, 25),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = DrawingColor.White
        };

        lblTotalPedidos = new Label
        {
            Text = "Total de Items: 0",
            Location = new Point(20, 45),
            AutoSize = false,
            Size = new DrawingSize(400, 25),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = DrawingColor.White
        };

        lblTotalVentas = new Label
        {
            Text = "TOTAL VENTAS: $0.00",
            Location = new Point(20, 75),
            AutoSize = false,
            Size = new DrawingSize(500, 40),
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            ForeColor = DrawingColor.FromArgb(46, 204, 113)
        };

        Button btnCerrar = new Button
        {
            Text = "? Cerrar",
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            Location = new Point(840, 50),
            Size = new DrawingSize(120, 50),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = DrawingColor.FromArgb(231, 76, 60),
            ForeColor = DrawingColor.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCerrar.FlatAppearance.BorderSize = 0;
        btnCerrar.Click += (s, e) => this.Close();

        panelInferior.Controls.AddRange(new Control[] { lblCantidadVentas, lblTotalPedidos, lblTotalVentas, btnCerrar });

        // Agregar paneles al formulario
        this.Controls.Add(panelCentral);
        this.Controls.Add(panelInferior);
        this.Controls.Add(panelSuperior);
    }

    private void CargarReporte()
    {
        try
        {
            dgvReporte.Rows.Clear();

            // Obtener todas las ventas cerradas del día seleccionado
            var ventasDelDia = db.VentasCerradas
                .Include(v => v.Detalles)
                .Where(v => v.FechaCierre.Date == fechaSeleccionada.Date)
                .OrderBy(v => v.FechaCierre)
                .ToList();

            decimal totalVentas = 0;
            int totalItems = 0;
            int cantidadVentas = ventasDelDia.Count;
            decimal totalGeneralPlatillos = 0;

            // DEBUG: Mostrar información de depuración
            System.Diagnostics.Debug.WriteLine($"Fecha seleccionada: {fechaSeleccionada:dd/MM/yyyy}");
            System.Diagnostics.Debug.WriteLine($"Ventas encontradas: {cantidadVentas}");

            foreach (var venta in ventasDelDia)
            {
                string hora = venta.FechaCierre.ToString("HH:mm");
                string mesaTexto = venta.NumeroMesa == 0 ? "Para Llevar" : $"Mesa #{venta.NumeroMesa}";

                System.Diagnostics.Debug.WriteLine($"Procesando venta: Mesa={mesaTexto}, Hora={hora}, Detalles={venta.Detalles.Count}");

                foreach (var detalle in venta.Detalles)
                {
                    dgvReporte.Rows.Add(
                        hora,
                        mesaTexto,
                        venta.Mesero,
                        detalle.NombrePlatillo,
                        detalle.Cantidad,
                        detalle.PrecioUnitario,
                        detalle.Total
                    );

                    totalItems += detalle.Cantidad;
                    totalGeneralPlatillos += detalle.Total;
                }

                totalVentas += venta.Total;
            }

            // Agregar fila de totales al final
            if (dgvReporte.Rows.Count > 0)
            {
                int rowIndex = dgvReporte.Rows.Add(
                    "",
                    "",
                    "",
                    "TOTAL VENDIDO:",
                    totalItems,
                    "",
                    totalGeneralPlatillos
                );

                // Aplicar formato especial a la fila de totales
                DataGridViewRow filaTotal = dgvReporte.Rows[rowIndex];
                filaTotal.DefaultCellStyle.BackColor = DrawingColor.FromArgb(52, 73, 94);
                filaTotal.DefaultCellStyle.ForeColor = DrawingColor.White;
                filaTotal.DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                filaTotal.DefaultCellStyle.SelectionBackColor = DrawingColor.FromArgb(52, 73, 94);
                filaTotal.DefaultCellStyle.SelectionForeColor = DrawingColor.White;

                // Centrar la cantidad en la fila de totales
                filaTotal.Cells["Cantidad"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                // Alinear a la derecha el total
                filaTotal.Cells["Total"].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                filaTotal.Cells["Total"].Style.Format = "C2";
            }

            lblCantidadVentas.Text = $"Ventas cerradas: {cantidadVentas}";
            lblTotalPedidos.Text = $"Total de Items: {totalItems}";
            lblTotalVentas.Text = $"TOTAL VENTAS: ${totalVentas:F2}";

            if (dgvReporte.Rows.Count == 0)
            {
                // Verificar si hay ventas en total en la base de datos
                var totalVentasEnBD = db.VentasCerradas.Count();
                System.Diagnostics.Debug.WriteLine($"Total de ventas en BD: {totalVentasEnBD}");
                
                string mensaje = $"No se encontraron ventas para la fecha {fechaSeleccionada:dd/MM/yyyy}";
                if (totalVentasEnBD > 0)
                {
                    mensaje += $"\n\nHay {totalVentasEnBD} venta(s) en total en la base de datos, pero ninguna para esta fecha.";
                }
                else
                {
                    mensaje += "\n\nNo hay ventas cerradas en la base de datos. Asegúrese de cerrar mesas para registrar ventas.";
                }
                
                MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar reporte: {ex.Message}\n\n{ex.InnerException?.Message}\n\nStackTrace: {ex.StackTrace}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnImprimir_Click(object? sender, EventArgs e)
    {
        try
        {
            if (dgvReporte.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para imprimir.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var document = GenerarDocumentoPDF();
            document.GeneratePdfAndShow();

            MessageBox.Show("? Reporte generado correctamente.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al generar reporte: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Document GenerarDocumentoPDF()
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(column =>
                {
                    column.Item().AlignCenter().Text(config!.NombreRestaurante)
                        .FontSize(18).Bold();
                    column.Item().AlignCenter().Text(config.Direccion).FontSize(10);
                    column.Item().AlignCenter().Text(config.Telefono).FontSize(10);
                    column.Item().PaddingVertical(5);
                    column.Item().AlignCenter().Text($"Reporte de Ventas - {fechaSeleccionada:dd/MM/yyyy}")
                        .FontSize(14).Bold();
                    column.Item().PaddingVertical(5);
                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
                });

                page.Content().PaddingVertical(10).Column(column =>
                {
                    // Tabla de ventas
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(60);  // Hora
                            columns.ConstantColumn(70);  // Mesa
                            columns.ConstantColumn(80);  // Mesero
                            columns.RelativeColumn();    // Platillo
                            columns.ConstantColumn(40);  // Cantidad
                            columns.ConstantColumn(60);  // P. Unit.
                            columns.ConstantColumn(70);  // Total
                        });

                        // Encabezados
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Hora").FontColor(Colors.White).Bold().FontSize(9);
                            header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Mesa").FontColor(Colors.White).Bold().FontSize(9);
                            header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Mesero").FontColor(Colors.White).Bold().FontSize(9);
                            header.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("Platillo").FontColor(Colors.White).Bold().FontSize(9);
                            header.Cell().Background(Colors.Grey.Darken2).Padding(5).AlignCenter().Text("Cant.").FontColor(Colors.White).Bold().FontSize(9);
                            header.Cell().Background(Colors.Grey.Darken2).Padding(5).AlignRight().Text("P. Unit.").FontColor(Colors.White).Bold().FontSize(9);
                            header.Cell().Background(Colors.Grey.Darken2).Padding(5).AlignRight().Text("Total").FontColor(Colors.White).Bold().FontSize(9);
                        });

                        // Datos
                        int totalRows = dgvReporte.Rows.Count;
                        for (int i = 0; i < totalRows; i++)
                        {
                            DataGridViewRow row = dgvReporte.Rows[i];
                            
                            // Verificar si es la última fila (fila de totales)
                            bool esFilaTotal = i == totalRows - 1 && 
                                              row.Cells["Platillo"].Value?.ToString() == "TOTAL VENDIDO:";
                            
                            if (esFilaTotal)
                            {
                                // Fila de totales con formato especial
                                table.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("").FontSize(8);
                                table.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("").FontSize(8);
                                table.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("").FontSize(8);
                                table.Cell().Background(Colors.Grey.Darken2).Padding(5).Text("TOTAL VENDIDO:").FontColor(Colors.White).Bold().FontSize(10);
                                table.Cell().Background(Colors.Grey.Darken2).Padding(5).AlignCenter().Text(row.Cells["Cantidad"].Value?.ToString() ?? "").FontColor(Colors.White).Bold().FontSize(10);
                                table.Cell().Background(Colors.Grey.Darken2).Padding(5).AlignRight().Text("").FontSize(8);
                                table.Cell().Background(Colors.Grey.Darken2).Padding(5).AlignRight().Text($"${Convert.ToDecimal(row.Cells["Total"].Value):F2}").FontColor(Colors.White).Bold().FontSize(10);
                            }
                            else
                            {
                                // Filas normales de datos
                                var bgColor = i % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;
                                
                                table.Cell().Background(bgColor).Padding(4).Text(row.Cells["Hora"].Value?.ToString() ?? "").FontSize(8);
                                table.Cell().Background(bgColor).Padding(4).Text(row.Cells["Mesa"].Value?.ToString() ?? "").FontSize(8);
                                table.Cell().Background(bgColor).Padding(4).Text(row.Cells["Mesero"].Value?.ToString() ?? "").FontSize(8);
                                table.Cell().Background(bgColor).Padding(4).Text(row.Cells["Platillo"].Value?.ToString() ?? "").FontSize(8);
                                table.Cell().Background(bgColor).Padding(4).AlignCenter().Text(row.Cells["Cantidad"].Value?.ToString() ?? "").FontSize(8);
                                table.Cell().Background(bgColor).Padding(4).AlignRight().Text($"${Convert.ToDecimal(row.Cells["PrecioUnitario"].Value):F2}").FontSize(8);
                                table.Cell().Background(bgColor).Padding(4).AlignRight().Text($"${Convert.ToDecimal(row.Cells["Total"].Value):F2}").FontSize(8);
                            }
                        }
                    });

                    column.Item().PaddingTop(10);
                });

                page.Footer().Column(column =>
                {
                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
                    column.Item().PaddingVertical(10);
                    
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(lblCantidadVentas.Text).FontSize(11).Bold();
                            col.Item().Text(lblTotalPedidos.Text).FontSize(11).Bold();
                        });
                        
                        row.RelativeItem().AlignRight().Text(lblTotalVentas.Text)
                            .FontSize(16).Bold().FontColor(Colors.Green.Darken2);
                    });
                    
                    column.Item().PaddingTop(10);
                    column.Item().AlignCenter().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(8).Italic();
                });
            });
        });
    }
}
