using Microsoft.EntityFrameworkCore;
using restaurante.Data;
using restaurante.Models;

namespace restaurante.Forms;

public partial class MainForm : Form
{
    private Mesa? mesaActual;
    private List<DetallePedido> pedidoTemporal = new();
    private RestauranteContext db;
    private Panel panelTotales = null!;
    private Panel panelBotones = null!;
    private string nombreMesero = "";

    public MainForm()
    {
        InitializeComponent();
        db = new RestauranteContext();
        db.Database.EnsureCreated();
        CargarCategorias();
        ActualizarListaPedido();
        ActualizarEstadoMesas();
        
        // Asegurar que los controles se redimensionen correctamente al cargar
        this.Load += MainForm_Load;
        this.Shown += MainForm_Shown;
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        // Forzar redimensionamiento de categorías cuando el formulario se carga
        var flowCategorias = this.Controls.Find("flowCategorias", true).FirstOrDefault() as FlowLayoutPanel;
        if (flowCategorias != null)
        {
            flowCategorias.PerformLayout();
        }
    }

    private void MainForm_Shown(object? sender, EventArgs e)
    {
        // Redimensionar categorías después de que el formulario sea completamente visible
        var timer = new System.Windows.Forms.Timer { Interval = 100 };
        timer.Tick += (timerS, timerE) =>
        {
            timer.Stop();
            timer.Dispose();
            
            var flowCategorias = this.Controls.Find("flowCategorias", true).FirstOrDefault() as FlowLayoutPanel;
            if (flowCategorias != null && flowCategorias.Width > 0)
            {
                // Trigger resize event manualmente
                var resizeMethod = typeof(Control).GetMethod("OnResize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                resizeMethod?.Invoke(flowCategorias, new object[] { EventArgs.Empty });
            }
        };
        timer.Start();
    }

    private void InitializeComponent()
    {
        this.Text = "Sistema de Restaurante - POS";
        this.WindowState = FormWindowState.Maximized;
        this.BackColor = Color.FromArgb(236, 240, 241);
        this.MinimumSize = new Size(1024, 768);

        // Panel Superior - Más compacto
        Panel panelSuperior = new Panel
        {
            Dock = DockStyle.Top,
            Height = 85,  // Reducido de 110 a 85
            BackColor = Color.FromArgb(26, 188, 156),
            Padding = new Padding(12)  // Reducido de 15 a 12
        };

        Button btnSeleccionarMesa = new Button
        {
            Text = "??? MESAS",
            Location = new Point(15, 15),  // Reducido de 20,25 a 15,15
            Size = new Size(150, 50),  // Reducido de 180x60 a 150x50
            Font = new Font("Segoe UI", 12, FontStyle.Bold),  // Reducido de 14 a 12
            BackColor = Color.FromArgb(22, 160, 133),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnSeleccionarMesa.FlatAppearance.BorderSize = 0;
        btnSeleccionarMesa.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 180, 150);
        btnSeleccionarMesa.Click += BtnSeleccionarMesa_Click;

        Button btnParaLlevar = new Button
        {
            Text = "?? PARA LLEVAR",
            Location = new Point(175, 15),  // Ajustado
            Size = new Size(170, 50),  // Reducido de 200x60 a 170x50
            Font = new Font("Segoe UI", 12, FontStyle.Bold),  // Reducido de 14 a 12
            BackColor = Color.FromArgb(241, 196, 15),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnParaLlevar.FlatAppearance.BorderSize = 0;
        btnParaLlevar.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 156, 18);
        btnParaLlevar.Click += BtnParaLlevar_Click;

        Button btnAdministracion = new Button
        {
            Text = "?? ADMIN",
            Location = new Point(355, 15),  // Ajustado
            Size = new Size(130, 50),  // Reducido de 160x60 a 130x50
            Font = new Font("Segoe UI", 11, FontStyle.Bold),  // Reducido de 13 a 11
            BackColor = Color.FromArgb(155, 89, 182),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnAdministracion.FlatAppearance.BorderSize = 0;
        btnAdministracion.FlatAppearance.MouseOverBackColor = Color.FromArgb(142, 68, 173);
        btnAdministracion.Click += BtnAdministracion_Click;

        Label lblMesaActual = new Label
        {
            Name = "lblMesaActual",
            Text = "Seleccione una mesa",
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            AutoSize = false,
            Size = new Size(350, 50),  // Reducido de 400x60 a 350x50
            Font = new Font("Segoe UI", 14, FontStyle.Bold),  // Reducido de 16 a 14
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 15, 15, 0)  // Reducido padding
        };

        panelSuperior.Controls.AddRange(new Control[] { btnSeleccionarMesa, btnParaLlevar, btnAdministracion, lblMesaActual });
        
        panelSuperior.Resize += (s, e) =>
        {
            lblMesaActual.Left = panelSuperior.Width - lblMesaActual.Width - 20;
        };

        this.Controls.Add(panelSuperior);

        // TableLayoutPanel más compacto
        TableLayoutPanel mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(12),  // Reducido de 20 a 12
            BackColor = Color.FromArgb(236, 240, 241)
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        // Panel Izquierdo más compacto
        Panel panelIzquierdo = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(6),  // Reducido de 10 a 6
            BackColor = Color.Transparent
        };

        Label lblCategorias = new Label
        {
            Text = "?? CATEGORÍAS",
            Dock = DockStyle.Top,
            Height = 40,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 12, 0, 0),
            BackColor = Color.FromArgb(248, 249, 250),
            Margin = new Padding(0, 280, 0, 0)  // Aumentado de 200px a 280px para mover mucho más hacia abajo
        };

        FlowLayoutPanel flowCategorias = new FlowLayoutPanel
        {
            Name = "flowCategorias",
            Dock = DockStyle.Top,
            Height = 160,  // Aumentado de 120 a 160 para hacer mucho más alta la sección
            AutoScroll = false,
            WrapContents = true,
            Padding = new Padding(5),
            BackColor = Color.White,
            Margin = new Padding(0, 2, 0, 20),
            FlowDirection = FlowDirection.LeftToRight
        };

        // Evento mejorado para hacer responsivos los botones de categoría
        flowCategorias.Resize += (s, e) =>
        {
            var timer = new System.Windows.Forms.Timer { Interval = 30 };
            timer.Tick += (timerS, timerE) =>
            {
                timer.Stop();
                timer.Dispose();
                
                if (flowCategorias.IsDisposed || flowCategorias.Width <= 0) return;
                
                int anchoDisponible = Math.Max(300, flowCategorias.ClientSize.Width - 15);
                int anchoMinimoPorBoton = 120;  // Reducido de 140 a 120
                int separacionEntreColumnas = 8;  // Reducido de 12 a 8
                
                // Calcular número de columnas para que se vean bien distribuidos
                int numeroColumnas = Math.Max(4, Math.Min(8, anchoDisponible / (anchoMinimoPorBoton + separacionEntreColumnas)));
                int anchoBotones = Math.Max(anchoMinimoPorBoton, (anchoDisponible - ((numeroColumnas - 1) * separacionEntreColumnas)) / numeroColumnas);
                
                foreach (Control control in flowCategorias.Controls)
                {
                    if (control is Button btn)
                    {
                        btn.Size = new Size(anchoBotones, 80);  // Aumentado de 65px a 80px de altura
                        btn.Margin = new Padding(3, 3, 3, 3);
                    }
                }
                
                flowCategorias.Invalidate();
            };
            timer.Start();
        };

        flowCategorias.VisibleChanged += (s, e) =>
        {
            if (flowCategorias.Visible)
            {
                flowCategorias.PerformLayout();
            }
        };

        Label lblPlatillos = new Label
        {
            Text = "PLATILLOS",
            Dock = DockStyle.Top,
            Height = 35,
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 8, 0, 0),
            BackColor = Color.FromArgb(248, 249, 250),
            Margin = new Padding(0, 15, 0, 0)  // Reducido de 30px a 15px ya que las categorías ahora están más abajo
        };

        FlowLayoutPanel flowPlatillos = new FlowLayoutPanel
        {
            Name = "flowPlatillos",
            Dock = DockStyle.Fill,
            AutoScroll = true,
            WrapContents = true,
            Padding = new Padding(10),  // Reducido de 15 a 10
            BackColor = Color.White
        };

        // Evento para hacer responsivos los paneles de platillos
        flowPlatillos.Resize += (s, e) =>
        {
            int anchoDisponible = flowPlatillos.Width - 35; // Más espacio para padding y scrollbar
            int numeroColumnas = Math.Max(3, anchoDisponible / 185);
            int anchoPlatillos = Math.Min(180, (anchoDisponible - (numeroColumnas * 12)) / numeroColumnas);
            
            foreach (Control control in flowPlatillos.Controls)
            {
                if (control is Panel panel)
                {
                    panel.Width = anchoPlatillos;
                }
            }
        };

        panelIzquierdo.Controls.Add(flowPlatillos);
        panelIzquierdo.Controls.Add(lblPlatillos);
        panelIzquierdo.Controls.Add(flowCategorias);
        panelIzquierdo.Controls.Add(lblCategorias);

        // Panel Derecho más compacto
        Panel panelDerecho = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(6),  // Reducido de 10 a 6
            BackColor = Color.Transparent
        };

        Panel headerPedido = new Panel
        {
            Dock = DockStyle.Top,
            Height = 45,  // Reducido de 60 a 45
            BackColor = Color.FromArgb(52, 73, 94)
        };

        Label lblPedido = new Label
        {
            Text = "?? PEDIDO ACTUAL",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),  // Reducido de 14 a 12
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        };
        headerPedido.Controls.Add(lblPedido);

        // Panel para mesero más compacto
        Panel panelMesero = new Panel
        {
            Dock = DockStyle.Top,
            Height = 55,  // Reducido de 75 a 55
            BackColor = Color.FromArgb(240, 243, 244),
            Padding = new Padding(10),  // Reducido de 15 a 10
            Margin = new Padding(0, 0, 0, 5)  // Reducido margin
        };

        Label lblMeseroLabel = new Label
        {
            Text = "?? Mesero:",
            Location = new Point(10, 18),  // Ajustado para nuevo tamaño
            Size = new Size(75, 20),  // Reducido
            Font = new Font("Segoe UI", 9, FontStyle.Bold),  // Reducido de 10 a 9
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        TextBox txtMesero = new TextBox
        {
            Name = "txtMesero",
            Location = new Point(90, 16),  // Ajustado
            Size = new Size(180, 22),  // Reducido de 220x25 a 180x22
            Font = new Font("Segoe UI", 9, FontStyle.Bold),  // Reducido de 10 a 9
            PlaceholderText = "Ingrese nombre del mesero"
        };
        txtMesero.TextChanged += (s, e) => nombreMesero = txtMesero.Text;

        panelMesero.Controls.AddRange(new Control[] { lblMeseroLabel, txtMesero });

        // Separador más visible con mejor color
        Panel separador = new Panel
        {
            Dock = DockStyle.Top,
            Height = 8,  // Reducido de 12 a 8
            BackColor = Color.FromArgb(200, 205, 210),  // Color más visible pero sutil
            Margin = new Padding(0, 5, 0, 5)  // Margins superior e inferior
        };

        // Panel contenedor para el ListBox con más padding
        Panel panelContenedorPedidos = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(15),  // Aumentado de 12 a 15
            BackColor = Color.FromArgb(240, 243, 244)  // Fondo sutil diferente
        };

        // Panel interno con borde para el ListBox
        Panel panelListBox = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(3),
            BackColor = Color.FromArgb(52, 73, 94)
        };

        ListBox lstPedido = new ListBox
        {
            Name = "lstPedido",
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 9, FontStyle.Regular),  // Reducido de 11 a 9
            BackColor = Color.FromArgb(255, 255, 255),
            ForeColor = Color.FromArgb(52, 73, 94),
            BorderStyle = BorderStyle.None,
            ItemHeight = 32,  // Reducido de 45 a 32
            DrawMode = DrawMode.OwnerDrawFixed,
            IntegralHeight = false,
            SelectionMode = SelectionMode.One
        };
        
        // Dibujar items con mejor formato y separación
        lstPedido.DrawItem += (s, e) =>
        {
            if (e.Index < 0) return;
            
            Color bgColor = e.Index % 2 == 0 
                ? Color.White 
                : Color.FromArgb(248, 250, 252);
            
            e.Graphics.FillRectangle(new SolidBrush(bgColor), e.Bounds);
            
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(180, 220, 255)), e.Bounds);
            }
            
            // Línea separadora más sutil
            e.Graphics.DrawLine(
                new Pen(Color.FromArgb(230, 235, 240)), 
                e.Bounds.Left + 8, 
                e.Bounds.Bottom - 1, 
                e.Bounds.Right - 8, 
                e.Bounds.Bottom - 1
            );
            
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(44, 62, 80)))
            {
                e.Graphics.DrawString(
                    lstPedido.Items[e.Index].ToString(),
                    e.Font!,
                    brush,
                    e.Bounds.Left + 10,  // Reducido de 15 a 10
                    e.Bounds.Top + 8     // Reducido de 15 a 8
                );
            }
        };

        panelListBox.Controls.Add(lstPedido);
        panelContenedorPedidos.Controls.Add(panelListBox);

        // Panel de Totales más compacto
        panelTotales = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 160,  // Reducido de 200 a 160
            BackColor = Color.FromArgb(52, 73, 94),
            Padding = new Padding(12),  // Reducido de 18 a 12
            Margin = new Padding(0, 8, 0, 0)  // Reducido margin
        };

        Label lblSubtotal = new Label
        {
            Name = "lblSubtotal",
            Text = "Subtotal: $0.00",
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            Location = new Point(12, 12),  // Ajustado
            Size = new Size(300, 22),  // Reducido
            Font = new Font("Segoe UI", 10, FontStyle.Bold),  // Reducido de 12 a 10
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleRight
        };

        // Panel para descuento más compacto
        Panel panelDescuento = new Panel
        {
            Location = new Point(12, 40),  // Ajustado
            Size = new Size(300, 30),  // Reducido
            BackColor = Color.Transparent
        };

        Label lblDescuentoLabel = new Label
        {
            Text = "Descuento:",
            Location = new Point(0, 6),  // Ajustado
            Size = new Size(75, 18),  // Reducido
            Font = new Font("Segoe UI", 9),  // Reducido de 10 a 9
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleLeft
        };

        TextBox txtDescuento = new TextBox
        {
            Name = "txtDescuento",
            Location = new Point(80, 4),  // Ajustado
            Size = new Size(70, 20),  // Reducido de 90x22 a 70x20
            Font = new Font("Segoe UI", 9),  // Reducido de 10 a 9
            Text = "0",
            TextAlign = HorizontalAlignment.Right
        };
        txtDescuento.TextChanged += (s, e) => ActualizarTotales();

        ComboBox cmbTipoDescuento = new ComboBox
        {
            Name = "cmbTipoDescuento",
            Location = new Point(155, 4),  // Ajustado
            Size = new Size(50, 20),  // Reducido de 65x22 a 50x20
            Font = new Font("Segoe UI", 9),  // Reducido de 10 a 9
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbTipoDescuento.Items.AddRange(new object[] { "%", "$" });
        cmbTipoDescuento.SelectedIndex = 0;
        cmbTipoDescuento.SelectedIndexChanged += (s, e) => ActualizarTotales();

        panelDescuento.Controls.AddRange(new Control[] { lblDescuentoLabel, txtDescuento, cmbTipoDescuento });

        // Panel del Total más compacto
        Panel panelTotal = new Panel
        {
            Name = "panelTotal",
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            Location = new Point(12, 80),  // Ajustado
            Size = new Size(300, 65),  // Reducido de 350x85 a 300x65
            BackColor = Color.FromArgb(231, 76, 60)
        };

        Label lblTotal = new Label
        {
            Name = "lblTotal",
            Text = "TOTAL: $0.00",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),  // Reducido de 20 a 16
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelTotal.Controls.Add(lblTotal);

        panelTotales.Controls.AddRange(new Control[] { lblSubtotal, panelDescuento, panelTotal });

        // Panel de Botones más compacto
        panelBotones = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 140,  // Reducido de 180 a 140
            BackColor = Color.White,
            Padding = new Padding(12),  // Reducido de 18 a 12
            Margin = new Padding(0, 8, 0, 0)  // Reducido margin
        };

        Button btnEliminarItem = new Button
        {
            Name = "btnEliminarItem",
            Text = "??? Eliminar",
            Location = new Point(12, 12),  // Ajustado
            Size = new Size(130, 35),  // Reducido de 160x44 a 130x35
            Font = new Font("Segoe UI", 9, FontStyle.Bold),  // Reducido de 10 a 9
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnEliminarItem.FlatAppearance.BorderSize = 0;
        btnEliminarItem.Click += BtnEliminarItem_Click;

        Button btnLimpiar = new Button
        {
            Name = "btnLimpiar",
            Text = "?? Limpiar",
            Location = new Point(150, 12),  // Ajustado
            Size = new Size(130, 35),  // Reducido de 160x44 a 130x35
            Font = new Font("Segoe UI", 9, FontStyle.Bold),  // Reducido de 10 a 9
            BackColor = Color.FromArgb(149, 165, 166),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnLimpiar.FlatAppearance.BorderSize = 0;
        btnLimpiar.Click += BtnLimpiar_Click;

        Button btnGuardarPedido = new Button
        {
            Name = "btnGuardarPedido",
            Text = "?? GUARDAR PEDIDO",
            Location = new Point(12, 55),  // Ajustado
            Size = new Size(268, 35),  // Reducido de 330x44 a 268x35
            Font = new Font("Segoe UI", 10, FontStyle.Bold),  // Reducido de 11 a 10
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnGuardarPedido.FlatAppearance.BorderSize = 0;
        btnGuardarPedido.Click += BtnGuardarPedido_Click;

        Button btnImprimirTicket = new Button
        {
            Name = "btnImprimirTicket",
            Text = "??? IMPRIMIR",
            Location = new Point(12, 98),  // Ajustado
            Size = new Size(130, 35),  // Reducido de 160x44 a 130x35
            Font = new Font("Segoe UI", 9, FontStyle.Bold),  // Reducido de 10 a 9
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnImprimirTicket.FlatAppearance.BorderSize = 0;
        btnImprimirTicket.Click += BtnImprimirTicket_Click;

        Button btnCerrarMesa = new Button
        {
            Name = "btnCerrarMesa",
            Text = "? Cerrar Mesa",
            Location = new Point(150, 98),  // Ajustado
            Size = new Size(130, 35),  // Reducido de 160x44 a 130x35
            Font = new Font("Segoe UI", 9, FontStyle.Bold),  // Reducido de 10 a 9
            BackColor = Color.FromArgb(192, 57, 43),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCerrarMesa.FlatAppearance.BorderSize = 0;
        btnCerrarMesa.Click += BtnCerrarMesa_Click;

        panelBotones.Controls.AddRange(new Control[] { btnEliminarItem, btnLimpiar, btnGuardarPedido, btnImprimirTicket, btnCerrarMesa });

        // Agregar controles al panel derecho en orden correcto
        panelDerecho.Controls.Add(panelContenedorPedidos);
        panelDerecho.Controls.Add(panelTotales);
        panelDerecho.Controls.Add(panelBotones);
        panelDerecho.Controls.Add(separador);
        panelDerecho.Controls.Add(panelMesero);
        panelDerecho.Controls.Add(headerPedido);

        // Agregar paneles al layout principal
        mainLayout.Controls.Add(panelIzquierdo, 0, 0);
        mainLayout.Controls.Add(panelDerecho, 1, 0);

        this.Controls.Add(mainLayout);

        // Eventos de redimensionamiento para hacer responsivo
        panelBotones.Resize += PanelBotones_Resize;
        panelTotales.Resize += PanelTotales_Resize;
    }

    private void PanelBotones_Resize(object? sender, EventArgs e)
    {
        if (panelBotones == null) return;

        int ancho = (panelBotones.Width - 34) / 2;  // Ajustado para nuevo padding (12+12+10)
        
        var btnEliminarItem = panelBotones.Controls["btnEliminarItem"] as Button;
        var btnLimpiar = panelBotones.Controls["btnLimpiar"] as Button;
        var btnGuardarPedido = panelBotones.Controls["btnGuardarPedido"] as Button;
        var btnImprimirTicket = panelBotones.Controls["btnImprimirTicket"] as Button;
        var btnCerrarMesa = panelBotones.Controls["btnCerrarMesa"] as Button;

        if (btnEliminarItem != null)
        {
            btnEliminarItem.Width = ancho;
        }

        if (btnLimpiar != null)
        {
            btnLimpiar.Left = 12 + ancho + 10;
            btnLimpiar.Width = ancho;
        }

        if (btnGuardarPedido != null)
        {
            btnGuardarPedido.Width = panelBotones.Width - 24;
        }

        if (btnImprimirTicket != null)
        {
            btnImprimirTicket.Width = ancho;
        }

        if (btnCerrarMesa != null)
        {
            btnCerrarMesa.Left = 12 + ancho + 10;
            btnCerrarMesa.Width = ancho;
        }
    }

    private void PanelTotales_Resize(object? sender, EventArgs e)
    {
        if (panelTotales == null) return;

        int anchoDisponible = panelTotales.Width - 24;  // Ajustado para nuevo padding (12+12)

        var lblSubtotal = this.Controls.Find("lblSubtotal", true).FirstOrDefault() as Label;
        if (lblSubtotal != null)
        {
            lblSubtotal.Width = anchoDisponible;
        }

        var panelDescuento = panelTotales.Controls.OfType<Panel>().FirstOrDefault(p => p.Controls.Count > 2);
        if (panelDescuento != null)
        {
            panelDescuento.Width = anchoDisponible;
        }

        var panelTotal = panelTotales.Controls["panelTotal"] as Panel;
        if (panelTotal != null)
        {
            panelTotal.Width = anchoDisponible;
        }
    }

    private void CargarCategorias()
    {
        var flowCategorias = this.Controls.Find("flowCategorias", true)[0] as FlowLayoutPanel;
        if (flowCategorias == null) return;
        
        flowCategorias.Controls.Clear();

        var colores = new[] {
            Color.FromArgb(231, 76, 60),   // Rojo - BEBIDAS
            Color.FromArgb(52, 152, 219),  // Azul - CAMARONES
            Color.FromArgb(46, 204, 113),  // Verde - CERVEZAS
            Color.FromArgb(155, 89, 182),  // Morado - FILETES
            Color.FromArgb(230, 126, 34),  // Naranja - extras
            Color.FromArgb(26, 188, 156),  // Turquesa
            Color.FromArgb(241, 196, 15),  // Amarillo
            Color.FromArgb(52, 73, 94)     // Gris oscuro
        };

        var categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
        if (!categorias.Any()) return;
        
        int colorIndex = 0;

        // Calcular dimensiones basadas en el ancho disponible del FlowLayoutPanel
        int anchoDisponible = Math.Max(300, flowCategorias.ClientSize.Width > 0 ? flowCategorias.ClientSize.Width - 15 : 500);
        int anchoMinimoPorBoton = 120;  // Reducido para que quepan más
        int separacionEntreColumnas = 8;  // Separación más pequeña
        
        // Determinar número de columnas óptimo - permitir más columnas para botones más pequeños
        int numeroColumnas = Math.Max(4, Math.Min(8, anchoDisponible / (anchoMinimoPorBoton + separacionEntreColumnas)));
        int anchoBotones = Math.Max(anchoMinimoPorBoton, (anchoDisponible - ((numeroColumnas - 1) * separacionEntreColumnas)) / numeroColumnas);

        foreach (var categoria in categorias)
        {
            Button btnCategoria = new Button
            {
                Text = categoria.Nombre.ToUpper(),
                Size = new Size(anchoBotones, 80),  // Aumentado de 65px a 80px de altura
                Font = new Font("Segoe UI", 11, FontStyle.Bold),  // Aumentado de 10 a 11 para mejor proporción
                BackColor = colores[colorIndex % colores.Length],
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag = categoria.Id,
                Margin = new Padding(3, 3, 3, 3),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false,
                TabStop = false
            };
            
            // Configurar apariencia del botón
            btnCategoria.FlatAppearance.BorderSize = 0;
            btnCategoria.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Max(0, btnCategoria.BackColor.R - 15),
                Math.Max(0, btnCategoria.BackColor.G - 15),
                Math.Max(0, btnCategoria.BackColor.B - 15)
            );
            
            // Eventos del botón
            btnCategoria.Click += (s, e) => {
                try 
                {
                    // Resetear el estilo de todos los botones de categoría
                    foreach (Control control in flowCategorias.Controls)
                    {
                        if (control is Button otroBtn)
                        {
                            otroBtn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                        }
                    }
                    
                    // Marcar el botón seleccionado
                    btnCategoria.Font = new Font("Segoe UI", 11, FontStyle.Bold | FontStyle.Underline);
                    
                    CargarPlatillosPorCategoria((int)btnCategoria.Tag);
                } 
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar platillos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            
            // Efectos visuales más sutiles
            btnCategoria.MouseEnter += (s, e) => {
                if (btnCategoria.Font.Style != (FontStyle.Bold | FontStyle.Underline))
                {
                    btnCategoria.Font = new Font("Segoe UI", 11, FontStyle.Bold | FontStyle.Italic);
                }
            };
            btnCategoria.MouseLeave += (s, e) => {
                if (btnCategoria.Font.Style != (FontStyle.Bold | FontStyle.Underline))
                {
                    btnCategoria.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                }
            };
            
            flowCategorias.Controls.Add(btnCategoria);
            colorIndex++;
        }

        // Forzar un redimensionamiento después de agregar todos los botones
        flowCategorias.PerformLayout();
        
        // Auto-seleccionar la primera categoría si existe
        if (flowCategorias.Controls.Count > 0)
        {
            var primerBoton = flowCategorias.Controls[0] as Button;
            primerBoton?.PerformClick();
        }
    }

    private void CargarPlatillosPorCategoria(int categoriaId)
    {
        var flowPlatillos = this.Controls.Find("flowPlatillos", true)[0] as FlowLayoutPanel;
        flowPlatillos?.Controls.Clear();

        var platillos = db.Platillos
            .Where(p => p.CategoriaId == categoriaId)
            .OrderBy(p => p.Nombre)
            .ToList();

        int anchoDisponible = flowPlatillos?.Width ?? 800;
        anchoDisponible -= 35; // Ajustado para nuevo padding
        int numeroColumnas = Math.Max(3, anchoDisponible / 185);
        int anchoPlatillos = Math.Min(180, (anchoDisponible - (numeroColumnas * 12)) / numeroColumnas);

        foreach (var platillo in platillos)
        {
            Panel panelPlatillo = new Panel
            {
                Size = new Size(anchoPlatillos, 130),  // Reducido de 160 a 130
                BackColor = Color.White,
                Margin = new Padding(5, 4, 5, 4),  // Reducido margins
                Tag = platillo,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            Panel headerPlato = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(anchoPlatillos - 2, 55),  // Reducido de 78 a 55
                BackColor = Color.FromArgb(52, 73, 94)
            };

            Label lblNombre = new Label
            {
                Text = platillo.NombreCorto,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),  // Reducido de 9 a 8
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(3, 8, 3, 8)  // Reducido padding
            };
            headerPlato.Controls.Add(lblNombre);

            Label lblPrecio = new Label
            {
                Text = $"${platillo.Precio:F2}",
                Location = new Point(5, 60),  // Ajustado para nueva posición
                Size = new Size(anchoPlatillos - 10, 25),  // Reducido de 34 a 25
                Font = new Font("Segoe UI", 12, FontStyle.Bold),  // Reducido de 14 a 12
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(39, 174, 96)
            };

            int anchoControles = 130;  // Reducido de 150 a 130
            int inicioControles = (anchoPlatillos - anchoControles) / 2;
            if (inicioControles < 5) inicioControles = 5;

            NumericUpDown numCantidad = new NumericUpDown
            {
                Location = new Point(inicioControles, 92),  // Ajustado para nueva altura
                Size = new Size(50, 25),  // Reducido de 60x32 a 50x25
                Minimum = 1,
                Maximum = 99,
                Value = 1,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),  // Reducido de 11 a 9
                TextAlign = HorizontalAlignment.Center
            };

            Button btnAgregar = new Button
            {
                Text = "? Agregar",
                Location = new Point(inicioControles + 55, 92),  // Ajustado
                Size = new Size(75, 25),  // Reducido de 90x32 a 75x25
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold),  // Reducido de 9 a 8
                Cursor = Cursors.Hand
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += (s, e) => AgregarPlatilloAPedido(platillo, (int)numCantidad.Value);

            panelPlatillo.Controls.AddRange(new Control[] { headerPlato, lblPrecio, numCantidad, btnAgregar });
            
            panelPlatillo.MouseEnter += (s, e) => panelPlatillo.BackColor = Color.FromArgb(240, 240, 240);
            panelPlatillo.MouseLeave += (s, e) => panelPlatillo.BackColor = Color.White;

            flowPlatillos?.Controls.Add(panelPlatillo);
        }
    }

    private void AgregarPlatilloAPedido(Platillo platillo, int cantidad)
    {
        if (mesaActual == null)
        {
            MessageBox.Show("?? Seleccione una mesa o para llevar primero.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var itemExistente = pedidoTemporal.FirstOrDefault(d => d.PlatilloId == platillo.Id);
        if (itemExistente != null)
        {
            itemExistente.Cantidad += cantidad;
        }
        else
        {
            pedidoTemporal.Add(new DetallePedido
            {
                PlatilloId = platillo.Id,
                Platillo = platillo,
                Cantidad = cantidad,
                PrecioUnitario = platillo.Precio,
                MesaId = mesaActual.Id
            });
        }

        ActualizarListaPedido();
        
        var lstPedido = this.Controls.Find("lstPedido", true)[0] as ListBox;
        if (lstPedido != null && lstPedido.Items.Count > 0)
        {
            lstPedido.SelectedIndex = lstPedido.Items.Count - 1;
        }
    }

    private void ActualizarListaPedido()
    {
        var lstPedido = this.Controls.Find("lstPedido", true)[0] as ListBox;
        if (lstPedido == null) return;

        lstPedido.Items.Clear();
        foreach (var detalle in pedidoTemporal)
        {
            var platillo = detalle.Platillo ?? db.Platillos.Find(detalle.PlatilloId);
            if (platillo != null)
            {
                string linea = $" {detalle.Cantidad,2}x  {platillo.NombreCorto,-18}  ${detalle.PrecioUnitario * detalle.Cantidad,7:F2}";
                lstPedido.Items.Add(linea);
            }
        }

        ActualizarTotales();
    }

    private void ActualizarTotales()
    {
        decimal subtotal = pedidoTemporal.Sum(d => d.PrecioUnitario * d.Cantidad);

        var lblSubtotal = this.Controls.Find("lblSubtotal", true)[0] as Label;
        if (lblSubtotal != null)
            lblSubtotal.Text = $"Subtotal: ${subtotal:F2}";

        var txtDescuento = this.Controls.Find("txtDescuento", true)[0] as TextBox;
        var cmbTipoDescuento = this.Controls.Find("cmbTipoDescuento", true)[0] as ComboBox;
        var lblTotal = this.Controls.Find("lblTotal", true)[0] as Label;

        decimal descuento = 0;
        if (decimal.TryParse(txtDescuento?.Text, out decimal valorDescuento))
        {
            if (cmbTipoDescuento?.SelectedItem?.ToString() == "%")
            {
                descuento = subtotal * (valorDescuento / 100);
            }
            else
            {
                descuento = valorDescuento;
            }
        }

        decimal total = subtotal - descuento;
        if (lblTotal != null)
        {
            lblTotal.Text = $"TOTAL: ${total:F2}";
        }
    }

    private void BtnSeleccionarMesa_Click(object? sender, EventArgs e)
    {
        using var mesasForm = new MesasForm(db);
        if (mesasForm.ShowDialog() == DialogResult.OK && mesasForm.MesaSeleccionada != null)
        {
            SeleccionarMesa(mesasForm.MesaSeleccionada);
        }
    }

    private void BtnParaLlevar_Click(object? sender, EventArgs e)
    {
        var mesaParaLlevar = db.Mesas.FirstOrDefault(m => m.NumeroMesa == 0);
        if (mesaParaLlevar != null)
        {
            SeleccionarMesa(mesaParaLlevar);
        }
    }

    private void SeleccionarMesa(Mesa mesa)
    {
        mesaActual = mesa;

        var lblMesaActual = this.Controls.Find("lblMesaActual", true)[0] as Label;
        if (lblMesaActual != null)
        {
            if (mesa.NumeroMesa == 0)
                lblMesaActual.Text = "?? PARA LLEVAR";
            else
                lblMesaActual.Text = $"??? MESA #{mesa.NumeroMesa}";
        }

        pedidoTemporal.Clear();
        nombreMesero = "";
        
        var txtMesero = this.Controls.Find("txtMesero", true)[0] as TextBox;
        if (txtMesero != null)
            txtMesero.Clear();

        if (mesa.EstaActiva)
        {
            var detalles = db.DetallesPedidos
                .Include(d => d.Platillo)
                .Where(d => d.MesaId == mesa.Id)
                .ToList();
            pedidoTemporal.AddRange(detalles);
        }

        ActualizarListaPedido();
    }

    private void BtnGuardarPedido_Click(object? sender, EventArgs e)
    {
        if (mesaActual == null || pedidoTemporal.Count == 0)
        {
            MessageBox.Show("?? No hay pedido para guardar.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            if (!mesaActual.EstaActiva)
            {
                mesaActual.EstaActiva = true;
                mesaActual.FechaApertura = DateTime.Now;
            }

            var detallesAnteriores = db.DetallesPedidos.Where(d => d.MesaId == mesaActual.Id);
            db.DetallesPedidos.RemoveRange(detallesAnteriores);

            foreach (var detalle in pedidoTemporal)
            {
                detalle.MesaId = mesaActual.Id;
                db.DetallesPedidos.Add(detalle);
            }

            db.SaveChanges();
            MessageBox.Show("? Pedido guardado correctamente.", "Éxito", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            ActualizarEstadoMesas();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error al guardar: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnImprimirTicket_Click(object? sender, EventArgs e)
    {
        if (mesaActual == null || pedidoTemporal.Count == 0)
        {
            MessageBox.Show("?? No hay pedido para imprimir.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string meseroParaTicket = string.IsNullOrWhiteSpace(nombreMesero) ? "N/A" : nombreMesero;

        var ticketForm = new TicketForm(db, mesaActual, pedidoTemporal, ObtenerDescuento(), ObtenerValorDescuento(), ObtenerTipoDescuento(), meseroParaTicket);
        ticketForm.ShowDialog();
    }

    private decimal ObtenerDescuento()
    {
        decimal subtotal = pedidoTemporal.Sum(d => d.PrecioUnitario * d.Cantidad);
        var txtDescuento = this.Controls.Find("txtDescuento", true)[0] as TextBox;
        var cmbTipoDescuento = this.Controls.Find("cmbTipoDescuento", true)[0] as ComboBox;

        decimal descuento = 0;
        if (decimal.TryParse(txtDescuento?.Text, out decimal valorDescuento))
        {
            if (cmbTipoDescuento?.SelectedItem?.ToString() == "%")
            {
                descuento = subtotal * (valorDescuento / 100);
            }
            else
            {
                descuento = valorDescuento;
            }
        }

        return descuento;
    }

    private decimal ObtenerValorDescuento()
    {
        var txtDescuento = this.Controls.Find("txtDescuento", true)[0] as TextBox;
        if (decimal.TryParse(txtDescuento?.Text, out decimal valor))
            return valor;
        return 0;
    }

    private string ObtenerTipoDescuento()
    {
        var cmbTipoDescuento = this.Controls.Find("cmbTipoDescuento", true)[0] as ComboBox;
        return cmbTipoDescuento?.SelectedItem?.ToString() ?? "%";
    }

    private void BtnCerrarMesa_Click(object? sender, EventArgs e)
    {
        if (mesaActual == null)
        {
            MessageBox.Show("?? Seleccione una mesa primero.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var resultado = MessageBox.Show("¿Desea cerrar esta mesa/orden?", "Confirmar", 
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (resultado == DialogResult.Yes)
        {
            try
            {
                var detalles = db.DetallesPedidos.Where(d => d.MesaId == mesaActual.Id);
                db.DetallesPedidos.RemoveRange(detalles);

                mesaActual.EstaActiva = false;
                mesaActual.FechaApertura = null;

                db.SaveChanges();

                pedidoTemporal.Clear();
                mesaActual = null;
                nombreMesero = "";

                var lblMesaActual = this.Controls.Find("lblMesaActual", true)[0] as Label;
                if (lblMesaActual != null)
                    lblMesaActual.Text = "Seleccione una mesa";

                var txtMesero = this.Controls.Find("txtMesero", true)[0] as TextBox;
                if (txtMesero != null)
                    txtMesero.Clear();

                ActualizarListaPedido();
                ActualizarEstadoMesas();

                MessageBox.Show("? Mesa cerrada correctamente.", "Éxito", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"? Error al cerrar mesa: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnLimpiar_Click(object? sender, EventArgs e)
    {
        var resultado = MessageBox.Show("¿Desea limpiar el pedido actual?", "Confirmar", 
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (resultado == DialogResult.Yes)
        {
            pedidoTemporal.Clear();
            ActualizarListaPedido();
        }
    }

    private void BtnEliminarItem_Click(object? sender, EventArgs e)
    {
        var lstPedido = this.Controls.Find("lstPedido", true)[0] as ListBox;
        if (lstPedido?.SelectedIndex >= 0 && lstPedido.SelectedIndex < pedidoTemporal.Count)
        {
            pedidoTemporal.RemoveAt(lstPedido.SelectedIndex);
            ActualizarListaPedido();
        }
        else
        {
            MessageBox.Show("?? Seleccione un item para eliminar.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnAdministracion_Click(object? sender, EventArgs e)
    {
        using var adminForm = new AdminForm(db);
        adminForm.ShowDialog();
        CargarCategorias();
    }

    private void ActualizarEstadoMesas()
    {
        db = new RestauranteContext();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        db?.Dispose();
        base.OnFormClosing(e);
    }
}
