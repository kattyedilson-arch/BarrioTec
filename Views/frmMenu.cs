using System;
using System.Windows.Forms;
using System.Drawing;

namespace TiendaApp.Views {
    public class frmMenu : Form {
        public frmMenu() {
            Text          = "BarrioTec - Menú Principal";
            Size          = new Size(400, 380);
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponents();
        }

        private void InitializeComponents() {
            var font = new Font("Arial", 12);

            var btnClientes  = new Button { Text = "👤 Clientes",    Dock = DockStyle.Top, Height = 70, Font = font };
            var btnArticulos = new Button { Text = "📦 Artículos",   Dock = DockStyle.Top, Height = 70, Font = font };
            var btnVentas    = new Button { Text = "🛒 Nueva Venta", Dock = DockStyle.Top, Height = 70, Font = font };
            var btnHistorial = new Button { Text = "📋 Historial",   Dock = DockStyle.Top, Height = 70, Font = font };

            btnClientes .Click += (s, e) => new frmClientes().Show();
            btnArticulos.Click += (s, e) => new frmArticulos().Show();
            btnVentas   .Click += (s, e) => new frmNuevaVenta().Show();
            btnHistorial.Click += (s, e) => new frmHistorialVentas().Show();

            Controls.AddRange(new Control[] {
                btnHistorial, btnVentas, btnArticulos, btnClientes
            });
        }
    }
}