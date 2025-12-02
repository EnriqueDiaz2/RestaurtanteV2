using Microsoft.EntityFrameworkCore;
using restaurante.Data;
using restaurante.Models;

namespace restaurante.Forms;

public partial class MesasForm : Form
{
    private RestauranteContext db;
    public Mesa? MesaSeleccionada { get; private set; }

    public MesasForm(RestauranteContext context)
    {
        db = context;
        InitializeComponent();
        CargarMesas();
    }

    private void InitializeComponent()
    {
        this.Text = "Seleccionar Mesa";
        this.Size = new Size(900, 700);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.BackColor = Color.FromArgb(236, 240, 241);

        Panel panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = Color.FromArgb(52, 73, 94)
        };

        Label lblTitulo = new Label
        {
            Text = "??? SELECCIONE UNA MESA",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter
        };
        panelHeader.Controls.Add(lblTitulo);

        Panel panelLeyenda = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.White,
            Padding = new Padding(20, 15, 20, 15)
        };

        Label lblLibre = new Label
        {
            Text = "?? Libre",
            Location = new Point(250, 18),
            Size = new Size(100, 25),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(46, 204, 113)
        };

        Label lblOcupada = new Label
        {
            Text = "?? Ocupada",
            Location = new Point(400, 18),
            Size = new Size(120, 25),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(231, 76, 60)
        };

        panelLeyenda.Controls.AddRange(new Control[] { lblLibre, lblOcupada });

        FlowLayoutPanel flowMesas = new FlowLayoutPanel
        {
            Name = "flowMesas",
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(20),
            BackColor = Color.FromArgb(236, 240, 241)
        };

        this.Controls.Add(flowMesas);
        this.Controls.Add(panelLeyenda);
        this.Controls.Add(panelHeader);
    }

    private void CargarMesas()
    {
        var flowMesas = this.Controls.Find("flowMesas", true)[0] as FlowLayoutPanel;
        flowMesas?.Controls.Clear();

        var mesas = db.Mesas.Where(m => m.NumeroMesa > 0).OrderBy(m => m.NumeroMesa).ToList();

        foreach (var mesa in mesas)
        {
            Panel panelMesa = new Panel
            {
                Size = new Size(180, 160),
                Margin = new Padding(10),
                Tag = mesa,
                Cursor = Cursors.Hand
            };

            Panel headerMesa = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = mesa.EstaActiva ? Color.FromArgb(231, 76, 60) : Color.FromArgb(46, 204, 113)
            };

            Label lblNumero = new Label
            {
                Text = mesa.NumeroMesa.ToString(),
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 42, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };
            headerMesa.Controls.Add(lblNumero);

            Label lblEstado = new Label
            {
                Text = mesa.EstaActiva ? "OCUPADA" : "LIBRE",
                Dock = DockStyle.Bottom,
                Height = 60,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = mesa.EstaActiva ? Color.FromArgb(231, 76, 60) : Color.FromArgb(46, 204, 113),
                TextAlign = ContentAlignment.MiddleCenter
            };

            panelMesa.Controls.Add(lblEstado);
            panelMesa.Controls.Add(headerMesa);

            // Efecto hover
            panelMesa.MouseEnter += (s, e) =>
            {
                headerMesa.BackColor = mesa.EstaActiva ? Color.FromArgb(192, 57, 43) : Color.FromArgb(39, 174, 96);
            };
            panelMesa.MouseLeave += (s, e) =>
            {
                headerMesa.BackColor = mesa.EstaActiva ? Color.FromArgb(231, 76, 60) : Color.FromArgb(46, 204, 113);
            };

            panelMesa.Click += (s, e) =>
            {
                MesaSeleccionada = mesa;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            // Hacer que todos los controles hijos también respondan al click
            foreach (Control ctrl in panelMesa.Controls)
            {
                ctrl.Click += (s, e) =>
                {
                    MesaSeleccionada = mesa;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };
            }

            flowMesas?.Controls.Add(panelMesa);
        }
    }
}
