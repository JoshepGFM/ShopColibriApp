using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace ShopColibriApp.Models
{
    public class FechaIngreDTO
    {
        RestRequest request { get; set; }
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public int Entrada { get; set; }

        public int? EmpaqueId { get; set; }

        public int? InventarioId { get; set; }

        public string? EmpaqueNombre { get; set; }

        public string? InventarioNombre { get; set; }

        public async Task<ObservableRangeCollection<FechaIngreDTO>> GetFechaIngreRequisitos(DateTime inicio, DateTime final, bool? seleccion, bool todo)
        {
            try
            {
                string fechIni = Uri.EscapeDataString(inicio.Date.ToString("MM/dd/yyyy").Split(' ')[0]);
                string fechFin = Uri.EscapeDataString(final.Date.ToString("MM/dd/yyyy").Split(' ')[0]);
                string Route = string.Format("FechaIngres/Consulta?inicio={0}&final={1}&seleccion={2}&todo={3}", fechIni, fechFin, seleccion, todo);

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
                    var list = JsonConvert.DeserializeObject<ObservableRangeCollection<FechaIngreDTO>>(response.Content);

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
