using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TiendaApp.Data;
using TiendaApp.Models;

namespace TiendaApp.Controllers {
    public class ArticuloController {

        public List<Articulo> GetAll() {
            var lista = new List<Articulo>();
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand("SELECT Id, Nombre, Precio, Stock FROM Articulos", conn);
                var dr  = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(new Articulo {
                        Id     = (int)dr["Id"],
                        Nombre = dr["Nombre"].ToString(),
                        Precio = (decimal)dr["Precio"],
                        Stock  = (int)dr["Stock"]
                    });
            }
            return lista;
        }

        public void Insert(Articulo a) {
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Articulos (Nombre, Precio, Stock) VALUES (@n, @p, @s)", conn);
                cmd.Parameters.AddWithValue("@n", a.Nombre);
                cmd.Parameters.AddWithValue("@p", a.Precio);
                cmd.Parameters.AddWithValue("@s", a.Stock);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Articulo a) {
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Articulos SET Nombre=@n, Precio=@p, Stock=@s WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@n",  a.Nombre);
                cmd.Parameters.AddWithValue("@p",  a.Precio);
                cmd.Parameters.AddWithValue("@s",  a.Stock);
                cmd.Parameters.AddWithValue("@id", a.Id);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id) {
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Articulos WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Articulo> Search(string filtro) {
            var lista = new List<Articulo>();
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var cmd = new SqlCommand(
                    "SELECT Id, Nombre, Precio, Stock FROM Articulos WHERE Nombre LIKE @f", conn);
                cmd.Parameters.AddWithValue("@f", $"%{filtro}%");
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(new Articulo {
                        Id     = (int)dr["Id"],
                        Nombre = dr["Nombre"].ToString(),
                        Precio = (decimal)dr["Precio"],
                        Stock  = (int)dr["Stock"]
                    });
            }
            return lista;
        }
    }
}