using System.Windows.Forms;
using System.Drawing;
using TiendaApp.Controllers;

namespace TiendaApp.Views {
    public class frmHistorialVentas : Form {
        private DataGridView    dgv  = new DataGridView();
        private VentaController ctrl = new VentaController();

       public frmHistorialVentas() {
    Text       = "Historial de Ventas";
    Size       = new Size(750, 500);
    KeyPreview = true;
    KeyDown   += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

    dgv.Dock          = DockStyle.Fill;
    dgv.ReadOnly      = true;
    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    dgv.Anchor        = AnchorStyles.Top | AnchorStyles.Bottom
                      | AnchorStyles.Left | AnchorStyles.Right;

    dgv.CellDoubleClick += (s, e) => {
        if (dgv.SelectedRows.Count == 0) return;
        int ventaId = (int)dgv.SelectedRows[0].Cells["Id"].Value;
        new frmDetalleVenta(ventaId).ShowDialog();
    };

    var btnCerrar = new Button {
        Text   = "✖ Cerrar",
        Dock   = DockStyle.Bottom,
        Height = 35
    };
    btnCerrar.Click += (s, e) => this.Close();

    Controls.Add(dgv);
    Controls.Add(btnCerrar);

    dgv.DataSource = ctrl.GetHistorial();
}
    }
}