
using System.Windows.Forms;
using System.Drawing;
using TiendaApp.Controllers;
using TiendaApp.Models;

namespace TiendaApp.Views {
    public class frmBusquedaCliente : Form {

        public Cliente? ClienteSeleccionado { get; private set; }

        private DataGridView      dgv       = new DataGridView();
        private TextBox           txtFiltro = new TextBox();
        private ClienteController ctrl      = new ClienteController();

        public frmBusquedaCliente() {
            Text       = "Buscar Cliente";
            Size       = new Size(500, 400);
            KeyPreview = true;
            KeyDown   += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

            dgv.Dock          = DockStyle.Fill;
            dgv.ReadOnly      = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            txtFiltro.PlaceholderText = "Buscar por nombre...";
            txtFiltro.TextChanged    += (s, e) => {
                dgv.DataSource = ctrl.Search(txtFiltro.Text);
            };

            dgv.CellDoubleClick += (s, e) => {
                if (dgv.SelectedRows.Count == 0) return;
                var row = dgv.SelectedRows[0];
                ClienteSeleccionado = new Cliente {
                    Id     = (int)row.Cells["Id"].Value,
                    Nombre = row.Cells["Nombre"].Value.ToString(),
                    NIT    = row.Cells["NIT"].Value.ToString()
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 40 };
            txtFiltro.Dock = DockStyle.Fill;
            pnlTop.Controls.Add(txtFiltro);

            Controls.Add(dgv);
            Controls.Add(pnlTop);

            dgv.DataSource = ctrl.GetAll();
        }
    }
}