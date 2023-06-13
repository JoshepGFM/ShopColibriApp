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
    public class Producto
    {
        public RestRequest request { get; set; }
        //public Producto()
        //{
        //    Inventarios = new HashSet<Inventario>();
        //}
        public int Codigo { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        //public virtual ICollection<Inventario> Inventarios { get; } = new List<Inventario>();

        public async Task<List<Producto>> GetProducto()
        {

            try
            {
                string Route = string.Format("Productoes");
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
                    var list = JsonConvert.DeserializeObject<List<Producto>>(response.Content);
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

                throw;
            }
        }

        public async Task<Producto> GetProductoId(int id)
        {

            try
            {
                string Route = string.Format("Productoes/{0}", id);
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
                    var list = JsonConvert.DeserializeObject<Producto>(response.Content);
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

                throw;
            }
        }

        public async Task<bool> PostProducto()
        {
            try
            {
                string Route = string.Format("Productoes");
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

        public async Task<ObservableCollection<Producto>> GetBuscarProducto(string? Filtro)
        {
            try
            {
                string Route = string.Format("Productoes/BuscarProducto?Buscar={0}", Filtro);
                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Get);

                //agregar la info de seguridad del api, en este caso Apikey
                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

                RestResponse response = await client.ExecuteAsync(request);

                HttpStatusCode statusCode = response.StatusCode;

                //carga de la info en un json

                if (statusCode == HttpStatusCode.OK)
                {
                    var list = JsonConvert.DeserializeObject<ObservableCollection<Producto>>(response.Content);

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

        public async Task<bool> PutProducto()
        {
            try
            {
                string Route = string.Format("Productoes/{0}", this.Codigo);
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

        public async Task<bool> DeleteProducto()
        {
            try
            {
                string Route = string.Format("Productoes/{0}", this.Codigo);
                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Delete);

                //agregar la info de seguridad del api, en este caso Apikey
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
                // TO DO: Guardar estos errores en una bitácora para su posterior análisis
                throw;
            }
        }
    }
}
