using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TiendaApp.Data;
using TiendaApp.Models;

namespace TiendaApp.Controllers {
    public class ClienteController {

        public List<Cliente> GetAll() {
            var lista = new List<Cliente>();
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand("SELECT Id, Nombre, NIT FROM Clientes", conn);
                var dr  = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(new Cliente {
                        Id     = (int)dr["Id"],
                        Nombre = dr["Nombre"].ToString(),
                        NIT    = dr["NIT"].ToString()
                    });
            }
            return lista;
        }

        public void Insert(Cliente c) {
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Clientes (Nombre, NIT) VALUES (@n, @nit)", conn);
                cmd.Parameters.AddWithValue("@n",   c.Nombre);
                cmd.Parameters.AddWithValue("@nit", c.NIT);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Cliente c) {
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Clientes SET Nombre=@n, NIT=@nit WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@n",   c.Nombre);
                cmd.Parameters.AddWithValue("@nit", c.NIT);
                cmd.Parameters.AddWithValue("@id",  c.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id) {
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Clientes WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Cliente> Search(string filtro) {
            var lista = new List<Cliente>();
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand(
                    "SELECT Id, Nombre, NIT FROM Clientes WHERE Nombre LIKE @f", conn);
                cmd.Parameters.AddWithValue("@f", $"%{filtro}%");
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(new Cliente {
                        Id     = (int)dr["Id"],
                        Nombre = dr["Nombre"].ToString(),
                        NIT    = dr["NIT"].ToString()
                    });
            }
            return lista;
        }
    }
}