using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TiendaApp.Data;
using TiendaApp.Models;

namespace TiendaApp.Controllers {
    public class VentaController {

        public void RegistrarVenta(Venta venta) {
            using (SqlConnection conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try {
                    string sqlVenta = @"INSERT INTO Ventas (ClienteId, Total)
                                       OUTPUT INSERTED.Id VALUES (@c, @t)";
                    SqlCommand cmdV = new SqlCommand(sqlVenta, conn, tran);
                    cmdV.Parameters.AddWithValue("@c", venta.ClienteId);
                    cmdV.Parameters.AddWithValue("@t", venta.Total);
                    int ventaId = (int)cmdV.ExecuteScalar();

                    foreach (var d in venta.Detalles) {
                        string sqlDetalle = @"INSERT INTO DetalleVentas 
                            (VentaId, ArticuloId, Cantidad, Subtotal) 
                            VALUES (@v, @a, @can, @s)";
                        SqlCommand cmdD = new SqlCommand(sqlDetalle, conn, tran);
                        cmdD.Parameters.AddWithValue("@v",   ventaId);
                        cmdD.Parameters.AddWithValue("@a",   d.ArticuloId);
                        cmdD.Parameters.AddWithValue("@can", d.Cantidad);
                        cmdD.Parameters.AddWithValue("@s",   d.Subtotal);
                        cmdD.ExecuteNonQuery();

                        string sqlStock = "UPDATE Articulos SET Stock = Stock - @can WHERE Id = @a";
                        SqlCommand cmdS = new SqlCommand(sqlStock, conn, tran);
                        cmdS.Parameters.AddWithValue("@can", d.Cantidad);
                        cmdS.Parameters.AddWithValue("@a",   d.ArticuloId);
                        cmdS.ExecuteNonQuery();
                    }
                    tran.Commit();
                } catch (Exception) {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public List<Venta> GetHistorial() {
            var lista = new List<Venta>();
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var sql = @"SELECT v.Id, v.Fecha, v.Total, c.Nombre AS ClienteNombre
                            FROM Ventas v
                            INNER JOIN Clientes c ON v.ClienteId = c.Id
                            ORDER BY v.Fecha DESC";
                var dr = new SqlCommand(sql, conn).ExecuteReader();
                while (dr.Read())
                    lista.Add(new Venta {
                        Id            = (int)dr["Id"],
                        Fecha         = (DateTime)dr["Fecha"],
                        Total         = (decimal)dr["Total"],
                        ClienteNombre = dr["ClienteNombre"].ToString()
                    });
            }
            return lista;
        }

        public List<DetalleVenta> GetDetalle(int ventaId) {
            var lista = new List<DetalleVenta>();
            using (var conn = DbContext.Instance.GetConnection()) {
                conn.Open();
                var sql = @"SELECT dv.Cantidad, dv.Subtotal, a.Nombre AS ArticuloNombre
                            FROM DetalleVentas dv
                            INNER JOIN Articulos a ON dv.ArticuloId = a.Id
                            WHERE dv.VentaId = @id";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", ventaId);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                    lista.Add(new DetalleVenta {
                        ArticuloNombre = dr["ArticuloNombre"].ToString(),
                        Cantidad       = (int)dr["Cantidad"],
                        Subtotal       = (decimal)dr["Subtotal"]
                    });
            }
            return lista;
        }
    }
}