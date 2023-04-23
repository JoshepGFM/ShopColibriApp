using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopColibriApp.Models
{
    public class Pedidos
    {
        public RestRequest request { get; set; }

        public int Codigo { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime FechaEn { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Total { get; set; }

        public int UsuarioIdUsuario { get; set; }

        //public virtual Usuario UsuarioIdUsuarioNavigation { get; set; } = null!;

        //public virtual ICollection<Inventario> Inventarios { get; } = new List<Inventario>();
    }
}
