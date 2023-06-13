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
    public class Pedidos
    {
        public RestRequest request { get; set; }

        public int Codigo { get; set; }

        public DateTime Fecha { get; set; }

        public DateTime FechaEn { get; set; }

        public decimal Total { get; set; }

        public int UsuarioIdUsuario { get; set; }

        //public virtual Usuario UsuarioIdUsuarioNavigation { get; set; } = null!;

        //public virtual ICollection<Inventario> Inventarios { get; } = new List<Inventario>();

        public async Task<List<Pedidos>> GetPedidos()
        {
            try
            {
                string Route = string.Format("Pedidoes");

                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Get);

                //info de seguridad del api
                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

                RestResponse response = await client.ExecuteAsync(request);

                HttpStatusCode statusCode = response.StatusCode;

                //carga de la info en un json

                if (statusCode == HttpStatusCode.OK)
                {
                    var list = JsonConvert.DeserializeObject<List<Pedidos>>(response.Content);

                    return list;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                // TO DO: Guardar estos errores en una bitácora para su posterior análisis
                throw;
            }
        }

        public async Task<Pedidos> GetPedidosId(int id)
        {
            try
            {
                string Route = string.Format("Pedidoes/{0}", id);

                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Get);

                //info de seguridad del api
                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

                RestResponse response = await client.ExecuteAsync(request);

                HttpStatusCode statusCode = response.StatusCode;

                //carga de la info en un json

                if (statusCode == HttpStatusCode.OK)
                {
                    var list = JsonConvert.DeserializeObject<Pedidos>(response.Content);

                    return list;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                // TO DO: Guardar estos errores en una bitácora para su posterior análisis
                throw;
            }
        }

        public async Task<bool> PostPedidos()
        {
            try
            {
                string Route = string.Format("Pedidoes");
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

        public async Task<bool> PutPedidos()
        {
            try
            {
                string Route = string.Format("Pedidoes/{0}", this.Codigo);
                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Put);

                //info de seguridad del api
                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

                //Se transforma a Json para la api
                string SerialClass = JsonConvert.SerializeObject(this);

                request.AddBody(SerialClass, Servicios.CnnToShopColibri.mimetype);

                RestResponse response = await client.ExecuteAsync(request);

                HttpStatusCode statusCode = response.StatusCode;

                //carga de la info en un json

                if (statusCode == HttpStatusCode.NoContent)
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

        public async Task<bool> DeletePedido(int id)
        {
            try
            {
                string Route = string.Format("Pedidoes/{0}", id);

                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Delete);

                //info de seguridad del api
                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

                RestResponse response = await client.ExecuteAsync(request);

                HttpStatusCode statusCode = response.StatusCode;

                //carga de la info en un json

                if (statusCode == HttpStatusCode.NoContent)
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
