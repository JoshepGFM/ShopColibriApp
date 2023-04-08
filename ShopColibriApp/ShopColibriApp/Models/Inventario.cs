using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.Models
{
    public class Inventario
    {
        RestRequest request { get; set; }
        //public Inventario()
        //{
        //    Imagens = new HashSet<Imagen>();
        //    PedidosCodigos = new HashSet<Pedido>();
        //}
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public int Stock { get; set; }

        public decimal PrecioUn { get; set; }

        public string Origen { get; set; } = null!;

        public int ProductoCodigo { get; set; }

        public int EmpaqueId { get; set; }

        //public virtual Empaque Empaque { get; set; } = null!;
        //public virtual ICollection<Imagen> Imagens { get; } = new List<Imagen>();
        //public virtual Producto ProductoCodigoNavigation { get; set; } = null!;
        //public virtual ICollection<Pedido> PedidosCodigos { get; } = new List<Pedido>();

        public async Task<bool> PostInventario()
        {
            try
            {
                string Route = string.Format("Inventarios");
                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Post);

                //info de seguridad del api
                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

                //Se transforma a Json para la api
                string SerialClass = JsonConvert.SerializeObject(this);

                request.AddBody(SerialClass, Servicios.CnnToShopColibri.mimetype);

                RestResponse response = await client.ExecuteAsync(request);

                HttpStatusCode statusCode = response.StatusCode;

                //carga de la info en un json

                if (statusCode == HttpStatusCode.Created)
                {

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }


    }
}
