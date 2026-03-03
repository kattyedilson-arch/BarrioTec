using System;
using System.Windows.Forms;
using System.Drawing;
using TiendaApp.Controllers;
using TiendaApp.Models;

namespace TiendaApp.Views {
    public class frmClientes : Form {
        private DataGridView dgv         = new DataGridView();
        private TextBox      txtNombre   = new TextBox();
        private TextBox      txtNIT      = new TextBox();
        private Button       btnGuardar  = new Button { Text = "💾 Guardar" };
        private Button       btnEliminar = new Button { Text = "🗑 Eliminar" };
        private Button       btnCerrar   = new Button { Text = "✖ Cerrar" };
        private ClienteController ctrl   = new ClienteController();
        private int selectedId = 0;

        public frmClientes() {
            Text          = "Gestión de Clientes";
            Size          = new Size(700, 500);
            KeyPreview    = true;
            KeyDown      += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

            // Propiedades del grid
            dgv.Dock          = DockStyle.Fill;
            dgv.ReadOnly      = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Anchor        = AnchorStyles.Top | AnchorStyles.Bottom
                              | AnchorStyles.Left | AnchorStyles.Right;

            // Placeholders
            txtNombre.PlaceholderText = "Nombre";
            txtNIT.PlaceholderText    = "NIT";

            // TabIndex orden lógico
            txtNombre  .TabIndex = 0;
            txtNIT     .TabIndex = 1;
            btnGuardar .TabIndex = 2;
            btnEliminar.TabIndex = 3;
            btnCerrar  .TabIndex = 4;

            // Eventos
            dgv.SelectionChanged += dgv_SelectionChanged;
            btnGuardar .Click    += btnGuardar_Click;
            btnEliminar.Click    += btnEliminar_Click;
            btnCerrar  .Click    += (s, e) => this.Close();

            // Layout
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 85 };
            txtNombre  .Location = new Point(10,  10); txtNombre  .Size = new Size(200, 30);
            txtNIT     .Location = new Point(220, 10); txtNIT     .Size = new Size(150, 30);
            btnGuardar .Location = new Point(10,  45); btnGuardar .Size = new Size(110, 30);
            btnEliminar.Location = new Point(130, 45); btnEliminar.Size = new Size(110, 30);
            btnCerrar  .Location = new Point(560, 45); btnCerrar  .Size = new Size(100, 30);

            pnlTop.Controls.AddRange(new Control[] {
                txtNombre, txtNIT, btnGuardar, btnEliminar, btnCerrar
            });

            Controls.Add(dgv);
            Controls.Add(pnlTop);
            CargarDatos();
        }

        private void CargarDatos() {
            dgv.DataSource = ctrl.GetAll();
        }

        private void btnGuardar_Click(object sender, EventArgs e) {
            var c = new Cliente {
                Id     = selectedId,
                Nombre = txtNombre.Text,
                NIT    = txtNIT.Text
            };
            if (selectedId == 0) ctrl.Insert(c);
            else                 ctrl.Update(c);
            selectedId     = 0;
            txtNombre.Text = "";
            txtNIT.Text    = "";
            CargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e) {
            if (selectedId == 0) return;
            ctrl.Delete(selectedId);
            selectedId = 0;
            CargarDatos();
        }

        private void dgv_SelectionChanged(object sender, EventArgs e) {
            if (dgv.SelectedRows.Count == 0) return;
            var row        = dgv.SelectedRows[0];
            selectedId     = (int)row.Cells["Id"].Value;
            txtNombre.Text = row.Cells["Nombre"].Value.ToString();
            txtNIT.Text    = row.Cells["NIT"].Value.ToString();
        }
    }
}