using System;
using System.Windows.Forms;
using TiendaApp.Views;

namespace TiendaApp {
    internal static class Program {
        [STAThread]
        static void Main() {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmMenu());
        }
    }
}