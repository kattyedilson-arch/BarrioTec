using System;
using System.Collections.Generic;

namespace TiendaApp.Models {
    public class Venta {
        public int      Id            { get; set; }
        public DateTime Fecha         { get; set; }
        public int      ClienteId     { get; set; }
        public string?  ClienteNombre { get; set; }
        public decimal  Total         { get; set; }
        public List<DetalleVenta> Detalles { get; set; }
        public Venta() { Detalles = new List<DetalleVenta>(); }
    }
}