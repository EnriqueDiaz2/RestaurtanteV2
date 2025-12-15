using Microsoft.EntityFrameworkCore;
using restaurante.Data;
using restaurante.Models;

namespace restaurante.Forms;

public partial class AdminForm : Form
{
    private RestauranteContext db;

    public AdminForm(RestauranteContext context)
    {
        db = context;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Panel de Administración";
        this.Size = new Size(1000, 700);
        this.StartPosition = FormStartPosition.CenterParent;
        this.BackColor = Color.FromArgb(236, 240, 241);

        Panel panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = Color.FromArgb(155, 89, 182)
        };

        Label lblTitulo = new Label
        {
            Text = "?? PANEL DE ADMINISTRACIÓN",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelHeader.Controls.Add(lblTitulo);

        TabControl tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 11),
            Padding = new Point(15, 8)
        };

        // Tab de Configuración
        TabPage tabConfiguracion = new TabPage("?? Configuración");
        tabConfiguracion.BackColor = Color.White;
        
        // Tab de Categorías
        TabPage tabCategorias = new TabPage("?? Categorías");
        tabCategorias.BackColor = Color.White;
        
        // Tab de Platillos
        TabPage tabPlatillos = new TabPage("??? Platillos");
        tabPlatillos.BackColor = Color.White;
        
        // Tab de Artículos por Categoría
        TabPage tabArticulosPorCategoria = new TabPage("?? Por Categoría");
        tabArticulosPorCategoria.BackColor = Color.White;
        
        // Tab de Mesas
        TabPage tabMesas = new TabPage("?? Mesas");
        tabMesas.BackColor = Color.White;

        // Agregar las tabs PRIMERO antes de crear el contenido
        tabControl.TabPages.AddRange(new TabPage[] { 
            tabConfiguracion, 
            tabCategorias, 
            tabPlatillos, 
            tabArticulosPorCategoria, 
            tabMesas 
        });

        // Ahora crear el contenido de cada tab
        CrearTabConfiguracion(tabConfiguracion);
        CrearTabCategorias(tabCategorias);
        CrearTabPlatillos(tabPlatillos);
        CrearTabArticulosPorCategoria(tabArticulosPorCategoria);
        CrearTabMesas(tabMesas);
        
        this.Controls.Add(tabControl);
        this.Controls.Add(panelHeader);
    }

    private void CrearTabConfiguracion(TabPage tab)
    {
        Panel panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            BackColor = Color.White
        };

        Label lblTitulo = new Label
        {
            Text = "CONFIGURACIÓN DEL RESTAURANTE",
            Location = new Point(30, 30),
            Size = new Size(900, 40),
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        // Nombre del Restaurante
        Label lblNombre = new Label
        {
            Text = "Nombre del Restaurante:",
            Location = new Point(30, 100),
            Size = new Size(220, 30),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        TextBox txtNombre = new TextBox
        {
            Name = "txtNombreRestaurante",
            Location = new Point(260, 100),
            Size = new Size(500, 30),
            Font = new Font("Segoe UI", 11),
            BorderStyle = BorderStyle.FixedSingle
        };

        // Dirección
        Label lblDireccion = new Label
        {
            Text = "Dirección:",
            Location = new Point(30, 160),
            Size = new Size(220, 30),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        TextBox txtDireccion = new TextBox
        {
            Name = "txtDireccion",
            Location = new Point(260, 160),
            Size = new Size(500, 30),
            Font = new Font("Segoe UI", 11),
            BorderStyle = BorderStyle.FixedSingle
        };

        // Teléfono
        Label lblTelefono = new Label
        {
            Text = "Teléfono:",
            Location = new Point(30, 220),
            Size = new Size(220, 30),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        TextBox txtTelefono = new TextBox
        {
            Name = "txtTelefono",
            Location = new Point(260, 220),
            Size = new Size(250, 30),
            Font = new Font("Segoe UI", 11),
            BorderStyle = BorderStyle.FixedSingle
        };

        Button btnGuardarConfig = new Button
        {
            Text = "?? GUARDAR CONFIGURACIÓN",
            Location = new Point(260, 280),
            Size = new Size(300, 50),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnGuardarConfig.FlatAppearance.BorderSize = 0;
        btnGuardarConfig.Click += BtnGuardarConfig_Click;

        panel.Controls.AddRange(new Control[] { 
            lblTitulo, lblNombre, txtNombre, lblDireccion, txtDireccion, 
            lblTelefono, txtTelefono, btnGuardarConfig 
        });

        tab.Controls.Add(panel);

        // Cargar datos
        var config = db.Configuraciones.FirstOrDefault();
        if (config != null)
        {
            txtNombre.Text = config.NombreRestaurante;
            txtDireccion.Text = config.Direccion;
            txtTelefono.Text = config.Telefono;
        }
    }

    private void CrearTabCategorias(TabPage tab)
    {
        Panel panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            BackColor = Color.White
        };

        Label lblTitulo = new Label
        {
            Text = "GESTIÓN DE CATEGORÍAS",
            Location = new Point(30, 30),
            Size = new Size(900, 40),
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        Panel panelAgregar = new Panel
        {
            Location = new Point(30, 90),
            Size = new Size(900, 110),
            BackColor = Color.FromArgb(236, 240, 241),
            Padding = new Padding(15)
        };

        Label lblNombre = new Label
        {
            Text = "Nueva Categoría:",
            Location = new Point(15, 15),
            Size = new Size(150, 30),
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };

        TextBox txtNombreCategoria = new TextBox
        {
            Name = "txtNombreCategoria",
            Location = new Point(170, 15),
            Size = new Size(350, 30),
            Font = new Font("Segoe UI", 11),
            BorderStyle = BorderStyle.FixedSingle
        };

        Label lblColor = new Label
        {
            Text = "Color:",
            Location = new Point(15, 55),
            Size = new Size(150, 30),
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };

        ComboBox cmbColores = new ComboBox
        {
            Name = "cmbColores",
            Location = new Point(170, 55),
            Size = new Size(200, 30),
            Font = new Font("Segoe UI", 11),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        // Agregar colores predefinidos
        var colores = new Dictionary<string, string>
        {
            { "?? Azul", "#3498db" },
            { "?? Verde", "#2ecc71" },
            { "?? Rojo", "#e74c3c" },
            { "?? Amarillo", "#f1c40f" },
            { "?? Naranja", "#e67e22" },
            { "?? Morado", "#9b59b6" },
            { "?? Café", "#8b4513" },
            { "? Gris Oscuro", "#34495e" },
            { "?? Naranja Oscuro", "#d35400" },
            { "?? Verde Esmeralda", "#16a085" },
            { "?? Azul Claro", "#5dade2" },
            { "?? Rosa", "#ec7063" },
            { "?? Naranja Claro", "#f39c12" },
            { "?? Violeta", "#8e44ad" },
            { "?? Rosa Claro", "#f1948a" },
            { "? Gris Medio", "#7f8c8d" },
            { "?? Rojo Oscuro", "#c0392b" },
            { "?? Azul Medio", "#2980b9" },
            { "?? Verde Oscuro", "#27ae60" },
            { "?? Púrpura", "#7d3c98" },
            { "?? Coral", "#d64541" },
            { "?? Verde Agua", "#58b19f" },
            { "? Dorado", "#f5b041" },
            { "?? Azul Cielo", "#52b3d9" },
            { "?? Mandarina", "#ff8c42" },
            { "?? Uva", "#8b5a99" },
            { "?? Pino", "#2d5f3e" },
            { "?? Fresa", "#dc3545" },
            { "?? Kiwi", "#86c232" },
            { "?? Limón", "#ffd700" }
        };

        cmbColores.DisplayMember = "Key";
        cmbColores.ValueMember = "Value";
        cmbColores.DataSource = new BindingSource(colores, null);
        // IMPORTANTE: Establecer SelectedIndex DESPUÉS de asignar el DataSource
        if (cmbColores.Items.Count > 0)
        {
            cmbColores.SelectedIndex = 0;
        }

        Panel panelVistaPrevia = new Panel
        {
            Name = "panelVistaPrevia",
            Location = new Point(380, 55),
            Size = new Size(50, 30),
            BackColor = ColorTranslator.FromHtml("#3498db"),
            BorderStyle = BorderStyle.FixedSingle
        };

        cmbColores.SelectedIndexChanged += (s, e) =>
        {
            if (cmbColores.SelectedValue != null)
            {
                string colorHex = cmbColores.SelectedValue.ToString() ?? "#3498db";
                panelVistaPrevia.BackColor = ColorTranslator.FromHtml(colorHex);
            }
        };

        Button btnAgregarCategoria = new Button
        {
            Text = "? AGREGAR",
            Location = new Point(450, 50),
            Size = new Size(180, 40),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnAgregarCategoria.FlatAppearance.BorderSize = 0;
        btnAgregarCategoria.Click += BtnAgregarCategoria_Click;

        panelAgregar.Controls.AddRange(new Control[] { lblNombre, txtNombreCategoria, lblColor, cmbColores, panelVistaPrevia, btnAgregarCategoria });

        Label lblLista = new Label
        {
            Text = "Categorías Existentes:",
            Location = new Point(30, 220),
            Size = new Size(900, 30),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        DataGridView dgvCategorias = new DataGridView
        {
            Name = "dgvCategorias",
            Location = new Point(30, 260),
            Size = new Size(900, 240),
            Font = new Font("Segoe UI", 10),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            RowHeadersVisible = false,
            RowTemplate = { Height = 35 },
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle 
            { 
                BackColor = Color.FromArgb(236, 240, 241) 
            }
        };

        Button btnEliminarCategoria = new Button
        {
            Text = "??? ELIMINAR CATEGORÍA SELECCIONADA",
            Location = new Point(30, 515),
            Size = new Size(350, 45),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnEliminarCategoria.FlatAppearance.BorderSize = 0;
        btnEliminarCategoria.Click += BtnEliminarCategoria_Click;

        Button btnCambiarColor = new Button
        {
            Text = "?? CAMBIAR COLOR",
            Location = new Point(390, 515),
            Size = new Size(250, 45),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCambiarColor.FlatAppearance.BorderSize = 0;
        btnCambiarColor.Click += BtnCambiarColorCategoria_Click;

        panel.Controls.AddRange(new Control[] { 
            lblTitulo, panelAgregar, lblLista, dgvCategorias, btnEliminarCategoria, btnCambiarColor
        });

        tab.Controls.Add(panel);
        CargarCategoriasGrid(dgvCategorias);
    }

    private void CrearTabPlatillos(TabPage tab)
    {
        Panel panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            BackColor = Color.White
        };

        Label lblTitulo = new Label
        {
            Text = "GESTIÓN DE PLATILLOS",
            Location = new Point(30, 30),
            Size = new Size(900, 40),
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        Panel panelAgregar = new Panel
        {
            Location = new Point(30, 90),
            Size = new Size(900, 120),
            BackColor = Color.FromArgb(236, 240, 241),
            Padding = new Padding(15)
        };

        // Fila 1
        Label lblNombre = new Label
        {
            Text = "Nombre Completo:",
            Location = new Point(15, 15),
            Size = new Size(140, 25),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        TextBox txtNombrePlatillo = new TextBox
        {
            Name = "txtNombrePlatillo",
            Location = new Point(160, 15),
            Size = new Size(300, 25),
            Font = new Font("Segoe UI", 10),
            BorderStyle = BorderStyle.FixedSingle
        };

        Label lblNombreCorto = new Label
        {
            Text = "Nombre Corto:",
            Location = new Point(480, 15),
            Size = new Size(110, 25),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        TextBox txtNombreCorto = new TextBox
        {
            Name = "txtNombreCorto",
            Location = new Point(595, 15),
            Size = new Size(250, 25),
            Font = new Font("Segoe UI", 10),
            BorderStyle = BorderStyle.FixedSingle
        };

        // Fila 2
        Label lblPrecio = new Label
        {
            Text = "Precio ($):",
            Location = new Point(15, 55),
            Size = new Size(140, 25),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        NumericUpDown numPrecio = new NumericUpDown
        {
            Name = "numPrecio",
            Location = new Point(160, 55),
            Size = new Size(120, 25),
            Font = new Font("Segoe UI", 10),
            DecimalPlaces = 2,
            Maximum = 9999,
            Minimum = 0
        };

        Label lblCategoria = new Label
        {
            Text = "Categoría:",
            Location = new Point(300, 55),
            Size = new Size(80, 25),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        ComboBox cmbCategoria = new ComboBox
        {
            Name = "cmbCategoria",
            Location = new Point(385, 55),
            Size = new Size(200, 25),
            Font = new Font("Segoe UI", 10),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        Button btnAgregarPlatillo = new Button
        {
            Text = "? AGREGAR PLATILLO",
            Location = new Point(605, 50),
            Size = new Size(240, 35),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnAgregarPlatillo.FlatAppearance.BorderSize = 0;
        btnAgregarPlatillo.Click += BtnAgregarPlatillo_Click;

        panelAgregar.Controls.AddRange(new Control[] { 
            lblNombre, txtNombrePlatillo, lblNombreCorto, txtNombreCorto,
            lblPrecio, numPrecio, lblCategoria, cmbCategoria, btnAgregarPlatillo
        });

        Label lblLista = new Label
        {
            Text = "Platillos Existentes:",
            Location = new Point(30, 230),
            Size = new Size(900, 30),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        DataGridView dgvPlatillos = new DataGridView
        {
            Name = "dgvPlatillos",
            Location = new Point(30, 270),
            Size = new Size(900, 260),
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

        Button btnEliminarPlatillo = new Button
        {
            Text = "??? ELIMINAR",
            Location = new Point(30, 545),
            Size = new Size(180, 45),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnEliminarPlatillo.FlatAppearance.BorderSize = 0;
        btnEliminarPlatillo.Click += BtnEliminarPlatillo_Click;

        Button btnModificarPlatillo = new Button
        {
            Text = "?? MODIFICAR PRECIO",
            Location = new Point(220, 545),
            Size = new Size(220, 45),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnModificarPlatillo.FlatAppearance.BorderSize = 0;
        btnModificarPlatillo.Click += BtnModificarPlatillo_Click;

        panel.Controls.AddRange(new Control[] { 
            lblTitulo, panelAgregar, lblLista, dgvPlatillos, 
            btnEliminarPlatillo, btnModificarPlatillo
        });

        tab.Controls.Add(panel);

        CargarCategoriasCbo(cmbCategoria);
        CargarPlatillos(dgvPlatillos);
    }

    private void CrearTabArticulosPorCategoria(TabPage tab)
    {
        Panel panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            BackColor = Color.White
        };

        Label lblTitulo = new Label
        {
            Text = "ARTÍCULOS POR CATEGORÍA",
            Location = new Point(30, 30),
            Size = new Size(900, 40),
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        Label lblSeleccione = new Label
        {
            Text = "Seleccione una categoría:",
            Location = new Point(30, 90),
            Size = new Size(200, 30),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        ComboBox cmbCategoriaFiltro = new ComboBox
        {
            Name = "cmbCategoriaFiltro",
            Location = new Point(240, 90),
            Size = new Size(300, 30),
            Font = new Font("Segoe UI", 11),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        Button btnFiltrar = new Button
        {
            Text = "?? MOSTRAR",
            Location = new Point(550, 87),
            Size = new Size(150, 35),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnFiltrar.FlatAppearance.BorderSize = 0;

        Button btnMostrarTodos = new Button
        {
            Text = "?? TODOS",
            Location = new Point(710, 87),
            Size = new Size(150, 35),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(149, 165, 166),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnMostrarTodos.FlatAppearance.BorderSize = 0;

        Label lblResultados = new Label
        {
            Name = "lblResultados",
            Text = "Todos los artículos",
            Location = new Point(30, 145),
            Size = new Size(900, 30),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        DataGridView dgvPlatillosFiltrados = new DataGridView
        {
            Name = "dgvPlatillosFiltrados",
            Location = new Point(30, 185),
            Size = new Size(900, 335),
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

        Panel panelBotones = new Panel
        {
            Location = new Point(30, 535),
            Size = new Size(900, 50),
            BackColor = Color.Transparent
        };

        Button btnEliminarSeleccionado = new Button
        {
            Text = "??? ELIMINAR SELECCIONADO",
            Location = new Point(0, 5),
            Size = new Size(280, 45),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnEliminarSeleccionado.FlatAppearance.BorderSize = 0;

        Button btnModificarPrecio = new Button
        {
            Text = "?? MODIFICAR PRECIO",
            Location = new Point(290, 5),
            Size = new Size(250, 45),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnModificarPrecio.FlatAppearance.BorderSize = 0;

        Button btnEditarNombre = new Button
        {
            Text = "?? EDITAR NOMBRE",
            Location = new Point(550, 5),
            Size = new Size(250, 45),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(155, 89, 182),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnEditarNombre.FlatAppearance.BorderSize = 0;

        panelBotones.Controls.AddRange(new Control[] { btnEliminarSeleccionado, btnModificarPrecio, btnEditarNombre });

        panel.Controls.AddRange(new Control[] { 
            lblTitulo, lblSeleccione, cmbCategoriaFiltro, btnFiltrar, btnMostrarTodos,
            lblResultados, dgvPlatillosFiltrados, panelBotones
        });

        tab.Controls.Add(panel);

        // Cargar categorías en el combo
        CargarCategoriasCombo(cmbCategoriaFiltro);
        
        // Cargar todos los platillos inicialmente
        CargarPlatillosFiltrados(dgvPlatillosFiltrados, null);

        // Eventos
        btnFiltrar.Click += (s, e) =>
        {
            if (cmbCategoriaFiltro.SelectedValue != null)
            {
                int categoriaId = (int)cmbCategoriaFiltro.SelectedValue;
                var categoria = db.Categorias.Find(categoriaId);
                CargarPlatillosFiltrados(dgvPlatillosFiltrados, categoriaId);
                
                var lblRes = panel.Controls["lblResultados"] as Label;
                if (lblRes != null && categoria != null)
                    lblRes.Text = $"Artículos en categoría: {categoria.Nombre}";
            }
        };

        btnMostrarTodos.Click += (s, e) =>
        {
            CargarPlatillosFiltrados(dgvPlatillosFiltrados, null);
            var lblRes = panel.Controls["lblResultados"] as Label;
            if (lblRes != null)
                lblRes.Text = "Todos los artículos";
        };

        btnEliminarSeleccionado.Click += (s, e) =>
        {
            if (dgvPlatillosFiltrados.CurrentRow == null)
            {
                MessageBox.Show("?? Seleccione un artículo para eliminar.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show("¿Está seguro de eliminar este artículo?", "Confirmar", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (resultado == DialogResult.Yes)
            {
                try
                {
                    int platilloId = (int)dgvPlatillosFiltrados.CurrentRow.Cells["Id"].Value;
                    var platillo = db.Platillos.Find(platilloId);
                    if (platillo != null)
                    {
                        db.Platillos.Remove(platillo);
                        db.SaveChanges();
                        
                        // Recargar con el filtro actual
                        if (cmbCategoriaFiltro.SelectedValue != null)
                            CargarPlatillosFiltrados(dgvPlatillosFiltrados, (int)cmbCategoriaFiltro.SelectedValue);
                        else
                            CargarPlatillosFiltrados(dgvPlatillosFiltrados, null);
                        
                        MessageBox.Show("? Artículo eliminado correctamente.", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"? Error: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        };

        btnModificarPrecio.Click += (s, e) =>
        {
            if (dgvPlatillosFiltrados.CurrentRow == null)
            {
                MessageBox.Show("?? Seleccione un artículo para modificar.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int platilloId = (int)dgvPlatillosFiltrados.CurrentRow.Cells["Id"].Value;
                var platillo = db.Platillos.Find(platilloId);
                if (platillo != null)
                {
                    string? nuevoPrecioStr = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Ingrese el nuevo precio para:\n\n{platillo.Nombre}\n\nPrecio actual: ${platillo.Precio:F2}",
                        "Modificar Precio",
                        platillo.Precio.ToString());

                    if (!string.IsNullOrWhiteSpace(nuevoPrecioStr) && decimal.TryParse(nuevoPrecioStr, out decimal nuevoPrecio))
                    {
                        platillo.Precio = nuevoPrecio;
                        db.SaveChanges();
                        
                        // Recargar con el filtro actual
                        if (cmbCategoriaFiltro.SelectedValue != null)
                            CargarPlatillosFiltrados(dgvPlatillosFiltrados, (int)cmbCategoriaFiltro.SelectedValue);
                        else
                            CargarPlatillosFiltrados(dgvPlatillosFiltrados, null);
                        
                        MessageBox.Show("? Precio modificado correctamente.", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"? Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        btnEditarNombre.Click += (s, e) =>
        {
            if (dgvPlatillosFiltrados.CurrentRow == null)
            {
                MessageBox.Show("?? Seleccione un artículo para editar.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int platilloId = (int)dgvPlatillosFiltrados.CurrentRow.Cells["Id"].Value;
                var platillo = db.Platillos.Find(platilloId);
                if (platillo != null)
                {
                    string? nuevoNombre = Microsoft.VisualBasic.Interaction.InputBox(
                        $"Ingrese el nuevo nombre para:\n\n{platillo.Nombre}",
                        "Editar Nombre",
                        platillo.Nombre);

                    if (!string.IsNullOrWhiteSpace(nuevoNombre))
                    {
                        platillo.Nombre = nuevoNombre.Trim();
                        db.SaveChanges();
                        
                        // Recargar con el filtro actual
                        if (cmbCategoriaFiltro.SelectedValue != null)
                            CargarPlatillosFiltrados(dgvPlatillosFiltrados, (int)cmbCategoriaFiltro.SelectedValue);
                        else
                            CargarPlatillosFiltrados(dgvPlatillosFiltrados, null);
                        
                        MessageBox.Show("? Nombre modificado correctamente.", "Éxito", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"? Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };
    }

    private void CrearTabMesas(TabPage tab)
    {
        Panel panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(30),
            BackColor = Color.White
        };

        Label lblTitulo = new Label
        {
            Text = "GESTIÓN DE MESAS",
            Location = new Point(30, 30),
            Size = new Size(900, 40),
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        Panel panelAgregar = new Panel
        {
            Location = new Point(30, 90),
            Size = new Size(900, 80),
            BackColor = Color.FromArgb(236, 240, 241),
            Padding = new Padding(15)
        };

        Label lblNumero = new Label
        {
            Text = "Número de Mesa:",
            Location = new Point(15, 25),
            Size = new Size(150, 30),
            Font = new Font("Segoe UI", 11, FontStyle.Bold)
        };

        NumericUpDown numMesa = new NumericUpDown
        {
            Name = "numMesa",
            Location = new Point(170, 25),
            Size = new Size(120, 30),
            Font = new Font("Segoe UI", 11),
            Minimum = 1,
            Maximum = 999,
            Value = 1
        };

        Button btnAgregarMesa = new Button
        {
            Text = "? AGREGAR MESA",
            Location = new Point(310, 20),
            Size = new Size(200, 40),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(46, 204, 113),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnAgregarMesa.FlatAppearance.BorderSize = 0;
        btnAgregarMesa.Click += BtnAgregarMesa_Click;

        Button btnAgregarVariasMesas = new Button
        {
            Text = "?? AGREGAR VARIAS MESAS",
            Location = new Point(520, 20),
            Size = new Size(280, 40),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(52, 152, 219),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnAgregarVariasMesas.FlatAppearance.BorderSize = 0;
        btnAgregarVariasMesas.Click += BtnAgregarVariasMesas_Click;

        panelAgregar.Controls.AddRange(new Control[] { lblNumero, numMesa, btnAgregarMesa, btnAgregarVariasMesas });

        Label lblLista = new Label
        {
            Text = "Mesas Existentes:",
            Location = new Point(30, 190),
            Size = new Size(900, 30),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(52, 73, 94)
        };

        DataGridView dgvMesas = new DataGridView
        {
            Name = "dgvMesas",
            Location = new Point(30, 230),
            Size = new Size(900, 270),
            Font = new Font("Segoe UI", 10),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            RowHeadersVisible = false,
            RowTemplate = { Height = 35 },
            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle 
            { 
                BackColor = Color.FromArgb(236, 240, 241) 
            }
        };

        Button btnEliminarMesa = new Button
        {
            Text = "??? ELIMINAR MESA SELECCIONADA",
            Location = new Point(30, 515),
            Size = new Size(350, 45),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(231, 76, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnEliminarMesa.FlatAppearance.BorderSize = 0;
        btnEliminarMesa.Click += BtnEliminarMesa_Click;

        panel.Controls.AddRange(new Control[] { 
            lblTitulo, panelAgregar, lblLista, dgvMesas, btnEliminarMesa
        });

        tab.Controls.Add(panel);
        CargarMesasGrid(dgvMesas);
    }

    private void CargarMesasGrid(DataGridView dgv)
    {
        var mesas = db.Mesas
            .Where(m => m.NumeroMesa > 0)
            .OrderBy(m => m.NumeroMesa)
            .Select(m => new
            {
                m.Id,
                NumeroMesa = m.NumeroMesa,
                Estado = m.EstaActiva ? "?? Ocupada" : "?? Libre",
                FechaApertura = m.FechaApertura.HasValue ? m.FechaApertura.Value.ToString("dd/MM/yyyy HH:mm") : "-"
            })
            .ToList();

        dgv.DataSource = mesas;
        
        if (dgv.Columns["Id"] != null)
            dgv.Columns["Id"]!.Visible = false;
        
        if (dgv.Columns["NumeroMesa"] != null)
            dgv.Columns["NumeroMesa"]!.HeaderText = "Número";
    }

    private void CargarCategoriasGrid(DataGridView dgv)
    {
        var categorias = db.Categorias
            .OrderBy(c => c.Nombre)
            .Select(c => new
            {
                c.Id,
                c.Nombre,
                ColorHex = c.ColorHex,
                ColorMuestra = "        "
            })
            .ToList();

        dgv.DataSource = categorias;
        
        if (dgv.Columns["Id"] != null)
            dgv.Columns["Id"]!.Visible = false;

        if (dgv.Columns["ColorHex"] != null)
            dgv.Columns["ColorHex"]!.Visible = false;
        
        if (dgv.Columns["ColorMuestra"] != null)
        {
            dgv.Columns["ColorMuestra"]!.HeaderText = "Color";
            dgv.Columns["ColorMuestra"]!.Width = 80;
        }

        dgv.CellPainting += (s, e) =>
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgv.Columns[e.ColumnIndex].Name == "ColorMuestra")
            {
                e.PaintBackground(e.CellBounds, true);
                
                if (e.RowIndex < categorias.Count)
                {
                    string colorHex = categorias[e.RowIndex].ColorHex ?? "#3498db";
                    Color color = ColorTranslator.FromHtml(colorHex);
                    
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        Rectangle rect = new Rectangle(e.CellBounds.X + 5, e.CellBounds.Y + 5, e.CellBounds.Width - 10, e.CellBounds.Height - 10);
                        e.Graphics.FillRectangle(brush, rect);
                        e.Graphics.DrawRectangle(Pens.Gray, rect);
                    }
                }
                
                e.Handled = true;
            }
        };
    }

    private void BtnGuardarConfig_Click(object? sender, EventArgs e)
    {
        try
        {
            var txtNombre = this.Controls.Find("txtNombreRestaurante", true)[0] as TextBox;
            var txtDireccion = this.Controls.Find("txtDireccion", true)[0] as TextBox;
            var txtTelefono = this.Controls.Find("txtTelefono", true)[0] as TextBox;

            if (string.IsNullOrWhiteSpace(txtNombre?.Text))
            {
                MessageBox.Show("?? El nombre del restaurante es obligatorio.", "Aviso", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var config = db.Configuraciones.FirstOrDefault();
            if (config == null)
            {
                config = new Configuracion { Id = 1 };
                db.Configuraciones.Add(config);
            }

            config.NombreRestaurante = txtNombre?.Text ?? "";
            config.Direccion = txtDireccion?.Text ?? "";
            config.Telefono = txtTelefono?.Text ?? "";

            db.SaveChanges();
            MessageBox.Show("? Configuración guardada correctamente.", "Éxito", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAgregarCategoria_Click(object? sender, EventArgs e)
    {
        var txtNombre = this.Controls.Find("txtNombreCategoria", true)[0] as TextBox;
        var cmbColores = this.Controls.Find("cmbColores", true)[0] as ComboBox;
        
        if (string.IsNullOrWhiteSpace(txtNombre?.Text))
        {
            MessageBox.Show("?? Ingrese un nombre para la categoría.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            string colorHex = "#3498db";
            if (cmbColores?.SelectedValue != null)
            {
                colorHex = cmbColores.SelectedValue.ToString() ?? "#3498db";
            }

            var categoria = new Categoria
            {
                Nombre = txtNombre.Text.Trim(),
                ColorHex = colorHex
            };

            db.Categorias.Add(categoria);
            db.SaveChanges();

            txtNombre.Clear();
            var dgvCategorias = this.Controls.Find("dgvCategorias", true)[0] as DataGridView;
            if (dgvCategorias != null)
            {
                CargarCategoriasGrid(dgvCategorias);
            }

            MessageBox.Show("? Categoría agregada correctamente.", "Éxito", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnEliminarCategoria_Click(object? sender, EventArgs e)
    {
        var dgvCategorias = this.Controls.Find("dgvCategorias", true)[0] as DataGridView;
        if (dgvCategorias?.CurrentRow == null)
        {
            MessageBox.Show("?? Seleccione una categoría para eliminar.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            int categoriaId = (int)dgvCategorias.CurrentRow.Cells["Id"].Value;
            
            // Verificar si tiene platillos
            var tienePlatillos = db.Platillos.Any(p => p.CategoriaId == categoriaId);
            if (tienePlatillos)
            {
                MessageBox.Show("?? No se puede eliminar la categoría porque tiene platillos asociados.", 
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show("¿Está seguro de eliminar esta categoría?", "Confirmar", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (resultado == DialogResult.Yes)
            {
                var categoria = db.Categorias.Find(categoriaId);
                if (categoria != null)
                {
                    db.Categorias.Remove(categoria);
                    db.SaveChanges();
                    CargarCategoriasGrid(dgvCategorias);
                    MessageBox.Show("? Categoría eliminada correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CargarCategoriasCombo(ComboBox cmb)
    {
        var categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
        cmb.DataSource = categorias;
        cmb.DisplayMember = "Nombre";
        cmb.ValueMember = "Id";
    }

    private void CargarCategoriasCbo(ComboBox cmb)
    {
        var categorias = db.Categorias.OrderBy(c => c.Nombre).ToList();
        cmb.DataSource = categorias;
        cmb.DisplayMember = "Nombre";
        cmb.ValueMember = "Id";
    }

    private void CargarPlatillosFiltrados(DataGridView dgv, int? categoriaId)
    {
        var query = db.Platillos.Include(p => p.Categoria).AsQueryable();
        
        if (categoriaId.HasValue)
        {
            query = query.Where(p => p.CategoriaId == categoriaId.Value);
        }

        var platillos = query
            .Select(p => new
            {
                p.Id,
                p.Nombre,
                p.NombreCorto,
                Precio = p.Precio,
                Categoria = p.Categoria!.Nombre
            })
            .OrderBy(p => p.Categoria)
            .ThenBy(p => p.Nombre)
            .ToList();

        dgv.DataSource = platillos;
        
        if (dgv.Columns["Id"] != null)
            dgv.Columns["Id"]!.Visible = false;
        
        if (dgv.Columns["Precio"] != null)
            dgv.Columns["Precio"]!.DefaultCellStyle.Format = "C2";
        
        if (dgv.Columns["Nombre"] != null)
            dgv.Columns["Nombre"]!.HeaderText = "Nombre Completo";
        
        if (dgv.Columns["NombreCorto"] != null)
            dgv.Columns["NombreCorto"]!.HeaderText = "Nombre Corto";
    }

    private void CargarPlatillos(DataGridView dgv)
    {
        var platillos = db.Platillos
            .Include(p => p.Categoria)
            .Select(p => new
            {
                p.Id,
                p.Nombre,
                p.NombreCorto,
                Precio = p.Precio,
                Categoria = p.Categoria!.Nombre
            })
            .ToList();

        dgv.DataSource = platillos;
        
        if (dgv.Columns["Id"] != null)
            dgv.Columns["Id"]!.Visible = false;
        
        if (dgv.Columns["Precio"] != null)
            dgv.Columns["Precio"]!.DefaultCellStyle.Format = "C2";
    }

    private void BtnAgregarPlatillo_Click(object? sender, EventArgs e)
    {
        var txtNombre = this.Controls.Find("txtNombrePlatillo", true)[0] as TextBox;
        var txtNombreCorto = this.Controls.Find("txtNombreCorto", true)[0] as TextBox;
        var numPrecio = this.Controls.Find("numPrecio", true)[0] as NumericUpDown;
        var cmbCategoria = this.Controls.Find("cmbCategoria", true)[0] as ComboBox;

        if (string.IsNullOrWhiteSpace(txtNombre?.Text) || string.IsNullOrWhiteSpace(txtNombreCorto?.Text))
        {
            MessageBox.Show("?? Complete todos los campos.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (cmbCategoria?.SelectedValue == null)
        {
            MessageBox.Show("?? Seleccione una categoría.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var platillo = new Platillo
            {
                Nombre = txtNombre.Text.Trim(),
                NombreCorto = txtNombreCorto.Text.Trim(),
                Precio = numPrecio?.Value ?? 0,
                CategoriaId = (int)cmbCategoria.SelectedValue
            };

            db.Platillos.Add(platillo);
            db.SaveChanges();

            txtNombre.Clear();
            txtNombreCorto.Clear();
            if (numPrecio != null)
                numPrecio.Value = 0;

            var dgvPlatillos = this.Controls.Find("dgvPlatillos", true)[0] as DataGridView;
            if (dgvPlatillos != null)
                CargarPlatillos(dgvPlatillos);

            MessageBox.Show("? Platillo agregado correctamente.", "Éxito", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnEliminarPlatillo_Click(object? sender, EventArgs e)
    {
        var dgvPlatillos = this.Controls.Find("dgvPlatillos", true)[0] as DataGridView;
        if (dgvPlatillos?.CurrentRow == null)
        {
            MessageBox.Show("?? Seleccione un platillo para eliminar.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var resultado = MessageBox.Show("¿Está seguro de eliminar este platillo?", "Confirmar", 
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (resultado == DialogResult.Yes)
        {
            try
            {
                int platilloId = (int)dgvPlatillos.CurrentRow.Cells["Id"].Value;
                var platillo = db.Platillos.Find(platilloId);
                if (platillo != null)
                {
                    db.Platillos.Remove(platillo);
                    db.SaveChanges();
                    CargarPlatillos(dgvPlatillos);
                    MessageBox.Show("? Platillo eliminado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"? Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnModificarPlatillo_Click(object? sender, EventArgs e)
    {
        var dgvPlatillos = this.Controls.Find("dgvPlatillos", true)[0] as DataGridView;
        if (dgvPlatillos?.CurrentRow == null)
        {
            MessageBox.Show("?? Seleccione un platillo para modificar.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            int platilloId = (int)dgvPlatillos.CurrentRow.Cells["Id"].Value;
            var platillo = db.Platillos.Find(platilloId);
            if (platillo != null)
            {
                string? nuevoPrecioStr = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Ingrese el nuevo precio para:\n\n{platillo.Nombre}\n\nPrecio actual: ${platillo.Precio:F2}",
                    "Modificar Precio",
                    platillo.Precio.ToString());

                if (!string.IsNullOrWhiteSpace(nuevoPrecioStr) && decimal.TryParse(nuevoPrecioStr, out decimal nuevoPrecio))
                {
                    platillo.Precio = nuevoPrecio;
                    db.SaveChanges();
                    CargarPlatillos(dgvPlatillos);
                    MessageBox.Show("? Precio modificado correctamente.", "Éxito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAgregarMesa_Click(object? sender, EventArgs e)
    {
        var numMesa = this.Controls.Find("numMesa", true)[0] as NumericUpDown;
        if (numMesa == null) return;

        int numeroMesa = (int)numMesa.Value;

        if (db.Mesas.Any(m => m.NumeroMesa == numeroMesa))
        {
            MessageBox.Show($"?? Ya existe una mesa con el número {numeroMesa}.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var mesa = new Mesa
            {
                NumeroMesa = numeroMesa,
                EstaActiva = false
            };

            db.Mesas.Add(mesa);
            db.SaveChanges();

            var dgvMesas = this.Controls.Find("dgvMesas", true)[0] as DataGridView;
            if (dgvMesas != null)
                CargarMesasGrid(dgvMesas);

            MessageBox.Show($"? Mesa {numeroMesa} agregada correctamente.", "Éxito", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAgregarVariasMesas_Click(object? sender, EventArgs e)
    {
        string? input = Microsoft.VisualBasic.Interaction.InputBox(
            "Ingrese el rango de mesas a crear:\n\nFormato: inicio-fin\nEjemplo: 1-20 (creará mesas del 1 al 20)",
            "Agregar Varias Mesas",
            "1-10");

        if (string.IsNullOrWhiteSpace(input)) return;

        var partes = input.Split('-');
        if (partes.Length != 2 || !int.TryParse(partes[0], out int inicio) || !int.TryParse(partes[1], out int fin))
        {
            MessageBox.Show("?? Formato inválido. Use: inicio-fin (ejemplo: 1-20)", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (inicio > fin || inicio < 1 || fin > 999)
        {
            MessageBox.Show("?? Rango inválido. El inicio debe ser menor al fin y entre 1-999.", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            int agregadas = 0;
            int existentes = 0;

            for (int i = inicio; i <= fin; i++)
            {
                if (!db.Mesas.Any(m => m.NumeroMesa == i))
                {
                    db.Mesas.Add(new Mesa { NumeroMesa = i, EstaActiva = false });
                    agregadas++;
                }
                else
                {
                    existentes++;
                }
            }

            db.SaveChanges();

            var dgvMesas = this.Controls.Find("dgvMesas", true)[0] as DataGridView;
            if (dgvMesas != null)
                CargarMesasGrid(dgvMesas);

            string mensaje = $"? Operación completada:\n\n";
            mensaje += $"• Mesas agregadas: {agregadas}\n";
            if (existentes > 0)
                mensaje += $"• Mesas que ya existían: {existentes}";

            MessageBox.Show(mensaje, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnEliminarMesa_Click(object? sender, EventArgs e)
    {
        var dgvMesas = this.Controls.Find("dgvMesas", true)[0] as DataGridView;
        if (dgvMesas?.CurrentRow == null)
        {
            MessageBox.Show("?? Seleccione una mesa para eliminar.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            int mesaId = (int)dgvMesas.CurrentRow.Cells["Id"].Value;
            var mesa = db.Mesas.Include(m => m.Detalles).FirstOrDefault(m => m.Id == mesaId);
            
            if (mesa == null) return;

            if (mesa.EstaActiva || mesa.Detalles.Any())
            {
                MessageBox.Show("?? No se puede eliminar esta mesa porque tiene pedidos asociados o está activa.", 
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show($"¿Está seguro de eliminar la Mesa {mesa.NumeroMesa}?", "Confirmar", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (resultado == DialogResult.Yes)
            {
                db.Mesas.Remove(mesa);
                db.SaveChanges();
                CargarMesasGrid(dgvMesas);
                MessageBox.Show("? Mesa eliminada correctamente.", "Éxito", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCambiarColorCategoria_Click(object? sender, EventArgs e)
    {
        var dgvCategorias = this.Controls.Find("dgvCategorias", true)[0] as DataGridView;
        if (dgvCategorias?.CurrentRow == null)
        {
            MessageBox.Show("?? Seleccione una categoría para cambiar el color.", "Aviso", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            int categoriaId = (int)dgvCategorias.CurrentRow.Cells["Id"].Value;
            var categoria = db.Categorias.Find(categoriaId);
            
            if (categoria == null) return;

            Form formColor = new Form
            {
                Text = $"Seleccionar Color - {categoria.Nombre}",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitulo = new Label
            {
                Text = "Seleccione un color:",
                Location = new Point(20, 20),
                Size = new Size(450, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            var colores = new Dictionary<string, string>
            {
                { "?? Azul", "#3498db" },
                { "?? Verde", "#2ecc71" },
                { "?? Rojo", "#e74c3c" },
                { "?? Amarillo", "#f1c40f" },
                { "?? Naranja", "#e67e22" },
                { "?? Morado", "#9b59b6" },
                { "?? Café", "#8b4513" },
                { "? Gris Oscuro", "#34495e" },
                { "?? Naranja Oscuro", "#d35400" },
                { "?? Verde Esmeralda", "#16a085" },
                { "?? Azul Claro", "#5dade2" },
                { "?? Rosa", "#ec7063" },
                { "?? Naranja Claro", "#f39c12" },
                { "?? Violeta", "#8e44ad" },
                { "?? Rosa Claro", "#f1948a" },
                { "? Gris Medio", "#7f8c8d" },
                { "?? Rojo Oscuro", "#c0392b" },
                { "?? Azul Medio", "#2980b9" },
                { "?? Verde Oscuro", "#27ae60" },
                { "?? Púrpura", "#7d3c98" },
                { "?? Coral", "#d64541" },
                { "?? Verde Agua", "#58b19f" },
                { "? Dorado", "#f5b041" },
                { "?? Azul Cielo", "#52b3d9" },
                { "?? Mandarina", "#ff8c42" },
                { "?? Uva", "#8b5a99" },
                { "?? Pino", "#2d5f3e" },
                { "?? Fresa", "#dc3545" },
                { "?? Kiwi", "#86c232" },
                { "?? Limón", "#ffd700" }
            };

            ListBox lstColores = new ListBox
            {
                Location = new Point(20, 60),
                Size = new Size(250, 250),
                Font = new Font("Segoe UI", 11),
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 30
            };

            foreach (var color in colores)
            {
                lstColores.Items.Add(color);
            }

            lstColores.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                
                e.DrawBackground();
                var item = (KeyValuePair<string, string>)lstColores.Items[e.Index];
                
                Rectangle colorRect = new Rectangle(e.Bounds.X + 5, e.Bounds.Y + 5, 30, e.Bounds.Height - 10);
                using (SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml(item.Value)))
                {
                    e.Graphics.FillRectangle(brush, colorRect);
                }
                e.Graphics.DrawRectangle(Pens.Black, colorRect);
                
                using (SolidBrush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(item.Key, e.Font!, textBrush, e.Bounds.X + 45, e.Bounds.Y + 5);
                }
                
                e.DrawFocusRectangle();
            };

            Panel panelVistaPrevia = new Panel
            {
                Location = new Point(290, 60),
                Size = new Size(180, 180),
                BackColor = ColorTranslator.FromHtml(categoria.ColorHex ?? "#3498db"),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblPreview = new Label
            {
                Text = "Vista Previa",
                Location = new Point(290, 250),
                Size = new Size(180, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lstColores.SelectedIndexChanged += (s, e) =>
            {
                if (lstColores.SelectedItem != null)
                {
                    var item = (KeyValuePair<string, string>)lstColores.SelectedItem;
                    panelVistaPrevia.BackColor = ColorTranslator.FromHtml(item.Value);
                }
            };

            Button btnAceptar = new Button
            {
                Text = "?? Aplicar",
                Location = new Point(150, 320),
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnAceptar.FlatAppearance.BorderSize = 0;

            Button btnCancelar = new Button
            {
                Text = "?? Cancelar",
                Location = new Point(280, 320),
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };
            btnCancelar.FlatAppearance.BorderSize = 0;

            formColor.Controls.AddRange(new Control[] { lblTitulo, lstColores, panelVistaPrevia, lblPreview, btnAceptar, btnCancelar });

            if (formColor.ShowDialog() == DialogResult.OK && lstColores.SelectedItem != null)
            {
                var selectedColor = (KeyValuePair<string, string>)lstColores.SelectedItem;
                categoria.ColorHex = selectedColor.Value;
                db.SaveChanges();
                CargarCategoriasGrid(dgvCategorias);
                MessageBox.Show("? Color actualizado correctamente.", "Éxito", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"? Error: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
