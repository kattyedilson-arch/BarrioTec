namespace TiendaApp.Models {
    public class DetalleVenta {
        public int     Id             { get; set; }
        public int     VentaId        { get; set; }
        public int     ArticuloId     { get; set; }
        public string? ArticuloNombre { get; set; }
        public int     Cantidad       { get; set; }
        public decimal Subtotal       { get; set; }
    }
}

   
