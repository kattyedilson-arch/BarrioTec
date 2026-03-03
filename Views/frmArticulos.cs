using System;
using System.Windows.Forms;
using System.Drawing;
using TiendaApp.Controllers;
using TiendaApp.Models;

namespace TiendaApp.Views {
    public class frmArticulos : Form {
        private DataGridView dgv         = new DataGridView();
        private TextBox      txtNombre   = new TextBox();
        private TextBox      txtPrecio   = new TextBox();
        private TextBox      txtStock    = new TextBox();
        private Button       btnGuardar  = new Button { Text = "💾 Guardar" };
        private Button       btnEliminar = new Button { Text = "🗑 Eliminar" };
        private Button       btnCerrar   = new Button { Text = "✖ Cerrar" };
        private ArticuloController ctrl  = new ArticuloController();
        private int selectedId = 0;

        public frmArticulos() {
            Text       = "Gestión de Artículos";
            Size       = new Size(700, 500);
            KeyPreview = true;
            KeyDown   += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

            dgv.Dock          = DockStyle.Fill;
            dgv.ReadOnly      = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Anchor        = AnchorStyles.Top | AnchorStyles.Bottom
                              | AnchorStyles.Left | AnchorStyles.Right;

            txtNombre.PlaceholderText = "Nombre";
            txtPrecio.PlaceholderText = "Precio";
            txtStock .PlaceholderText = "Stock";

            txtNombre  .TabIndex = 0;
            txtPrecio  .TabIndex = 1;
            txtStock   .TabIndex = 2;
            btnGuardar .TabIndex = 3;
            btnEliminar.TabIndex = 4;
            btnCerrar  .TabIndex = 5;

            dgv.SelectionChanged += dgv_SelectionChanged;
            btnGuardar .Click    += btnGuardar_Click;
            btnEliminar.Click    += btnEliminar_Click;
            btnCerrar  .Click    += (s, e) => this.Close();

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 85 };
            txtNombre  .Location = new Point(10,  10); txtNombre  .Size = new Size(160, 30);
            txtPrecio  .Location = new Point(180, 10); txtPrecio  .Size = new Size(100, 30);
            txtStock   .Location = new Point(290, 10); txtStock   .Size = new Size(80,  30);
            btnGuardar .Location = new Point(10,  45); btnGuardar .Size = new Size(110, 30);
            btnEliminar.Location = new Point(130, 45); btnEliminar.Size = new Size(110, 30);
            btnCerrar  .Location = new Point(560, 45); btnCerrar  .Size = new Size(100, 30);

            pnlTop.Controls.AddRange(new Control[] {
                txtNombre, txtPrecio, txtStock, btnGuardar, btnEliminar, btnCerrar
            });

            Controls.Add(dgv);
            Controls.Add(pnlTop);
            CargarDatos();
        }

        private void CargarDatos() {
            dgv.DataSource = ctrl.GetAll();
        }

        private void btnGuardar_Click(object sender, EventArgs e) {
            var a = new Articulo {
                Id     = selectedId,
                Nombre = txtNombre.Text,
                Precio = decimal.TryParse(txtPrecio.Text, out var p) ? p : 0,
                Stock  = int.TryParse(txtStock.Text, out var s) ? s : 0
            };
            if (selectedId == 0) ctrl.Insert(a);
            else                 ctrl.Update(a);
            selectedId     = 0;
            txtNombre.Text = "";
            txtPrecio.Text = "";
            txtStock .Text = "";
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
            txtPrecio.Text = row.Cells["Precio"].Value.ToString();
            txtStock .Text = row.Cells["Stock"].Value.ToString();
        }
    }
}