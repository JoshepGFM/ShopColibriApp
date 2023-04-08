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
    public class InventarioDTO
    {
        public RestRequest request { get; set; }

        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public int Stock { get; set; }

        public decimal PrecioUn { get; set; }

        public string Origen { get; set; } = null!;

        public int ProductoCodigo { get; set; }

        public int EmpaqueId { get; set; }

        public string NombreEmp { get; set; }

        public string NombrePro { get; set; }

        public async Task<ObservableCollection<InventarioDTO>> GetBuscarInventario(string? Filtro, bool? estado)
        {
            try
            {
                string Route = string.Format("Inventarios/BusquedaIventario?buscar={0}&estado={1}", Filtro, estado);
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
                    var list = JsonConvert.DeserializeObject<ObservableCollection<InventarioDTO>>(response.Content);

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
    }
}
