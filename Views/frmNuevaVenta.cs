
using System;
using System.Windows.Forms;
using System.Drawing;
using TiendaApp.Controllers;
using TiendaApp.Models;

namespace TiendaApp.Views {
    public class frmNuevaVenta : Form {
        private int             selectedClienteId = 0;
        private Venta           ventaActual       = new Venta();
        private VentaController ctrl              = new VentaController();

        private Label        lblCliente        = new Label();
        private Label        lblTotal          = new Label();
        private Button       btnBuscarCliente  = new Button { Text = "👤 Buscar Cliente" };
        private Button       btnBuscarArticulo = new Button { Text = "📦 Agregar Artículo" };
        private Button       btnConfirmar      = new Button { Text = "✅ Confirmar Venta" };
        private Button       btnCerrar         = new Button { Text = "✖ Cerrar" };
        private DataGridView dgvCarrito        = new DataGridView();

       public frmNuevaVenta() {
    Text       = "Nueva Venta";
    Size       = new Size(800, 550);
    KeyPreview = true;
    KeyDown   += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

    dgvCarrito.Dock          = DockStyle.Fill;
    dgvCarrito.ReadOnly      = true;
    dgvCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

    lblCliente.Text = "Sin cliente seleccionado";
    lblTotal.Text   = "Total: Q0.00";
    lblTotal.Font   = new Font("Arial", 12, FontStyle.Bold);

    btnBuscarCliente .Click += btnBuscarCliente_Click;
    btnBuscarArticulo.Click += btnBuscarArticulo_Click;
    btnConfirmar     .Click += btnConfirmar_Click;
    btnCerrar        .Click += (s, e) => this.Close();

    var pnlTop = new Panel { Dock = DockStyle.Top, Height = 100 };

    lblCliente       .Location = new Point(10,  8);  lblCliente       .Size = new Size(500, 25);
    btnBuscarCliente .Location = new Point(10,  40); btnBuscarCliente .Size = new Size(160, 35);
    btnBuscarArticulo.Location = new Point(180, 40); btnBuscarArticulo.Size = new Size(160, 35);
    btnConfirmar     .Location = new Point(350, 40); btnConfirmar     .Size = new Size(150, 35);
    btnCerrar        .Location = new Point(660, 40); btnCerrar        .Size = new Size(100, 35);
    lblTotal         .Location = new Point(10,  78); lblTotal         .Size = new Size(300, 25);

    pnlTop.Controls.AddRange(new System.Windows.Forms.Control[] {
        lblCliente, btnBuscarCliente, btnBuscarArticulo,
        btnConfirmar, btnCerrar, lblTotal
    });

    Controls.Add(dgvCarrito);
    Controls.Add(pnlTop);

    ActiveControl = btnBuscarCliente;
}
        private void btnBuscarCliente_Click(object sender, EventArgs e) {
            using (var buscador = new frmBusquedaCliente()) {
                if (buscador.ShowDialog() == DialogResult.OK && buscador.ClienteSeleccionado != null) {
                    lblCliente.Text   = "Cliente: " + buscador.ClienteSeleccionado.Nombre;
                    selectedClienteId = buscador.ClienteSeleccionado.Id;
                }
            }
        }

        private void btnBuscarArticulo_Click(object sender, EventArgs e) {
            using (var buscador = new frmBusquedaArticulo()) {
                if (buscador.ShowDialog() == DialogResult.OK && buscador.ArticuloSeleccionado != null) {
                    var art = buscador.ArticuloSeleccionado;

                    string input = Microsoft.VisualBasic.Interaction.InputBox(
                        "¿Cuántas unidades?", "Cantidad", "1");

                    if (!int.TryParse(input, out int cantidad) || cantidad <= 0) return;

                    ventaActual.Detalles.Add(new DetalleVenta {
                        ArticuloId     = art.Id,
                        ArticuloNombre = art.Nombre,
                        Cantidad       = cantidad,
                        Subtotal       = art.Precio * cantidad
                    });
                    ActualizarCarrito();
                }
            }
        }

        private void ActualizarCarrito() {
            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = ventaActual.Detalles;
            ventaActual.Total     = 0;
            foreach (var d in ventaActual.Detalles)
                ventaActual.Total += d.Subtotal;
            lblTotal.Text = $"Total: Q{ventaActual.Total:F2}";
        }

        private void btnConfirmar_Click(object sender, EventArgs e) {
            if (selectedClienteId == 0 || ventaActual.Detalles.Count == 0) {
                MessageBox.Show("Seleccione un cliente y al menos un artículo.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ventaActual.ClienteId = selectedClienteId;
            ctrl.RegistrarVenta(ventaActual);
            MessageBox.Show("✅ Venta registrada con éxito.", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            ventaActual       = new Venta();
            selectedClienteId = 0;
            lblCliente.Text   = "Sin cliente seleccionado";
            ActualizarCarrito();
        }
    }
}