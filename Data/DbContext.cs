using Microsoft.Data.SqlClient;

namespace TiendaApp.Data {
    public class DbContext {
        private static DbContext? _instance;
       private readonly string _connStr =
    "Server=LAPTOP-4IUHDDEA;Database=TiendaDb;User Id=sa;Password=Admin123;TrustServerCertificate=True;";

        private DbContext() { }

        public static DbContext Instance {
            get {
                if (_instance == null) _instance = new DbContext();
                return _instance;
            }
        }

        public SqlConnection GetConnection() {
            return new SqlConnection(_connStr);
        }
    }
}