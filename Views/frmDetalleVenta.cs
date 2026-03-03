using System.Windows.Forms;
using System.Drawing;
using TiendaApp.Controllers;

namespace TiendaApp.Views {
    public class frmDetalleVenta : Form {
        public frmDetalleVenta(int ventaId) {
            Text       = $"Detalle de Venta #{ventaId}";
            Size       = new Size(550, 400);
            KeyPreview = true;
            KeyDown   += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

            var dgv = new DataGridView {
                Dock          = DockStyle.Fill,
                ReadOnly      = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            var btnCerrar = new Button {
                Text   = "✖ Cerrar",
                Dock   = DockStyle.Bottom,
                Height = 35
            };
            btnCerrar.Click += (s, e) => this.Close();

            Controls.Add(dgv);
            Controls.Add(btnCerrar);

            var ctrl       = new VentaController();
            dgv.DataSource = ctrl.GetDetalle(ventaId);
        }
    }
}