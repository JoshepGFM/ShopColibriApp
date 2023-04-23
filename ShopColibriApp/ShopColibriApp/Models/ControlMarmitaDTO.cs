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
    public class ControlMarmitaDTO
    {
        public RestRequest request { get; set; }

        public int Codigo { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraEn { get; set; }

        public TimeSpan HoraAp { get; set; }

        public int Temperatura { get; set; }

        public string IntensidadMov { get; set; } = null!;

        public string Lote { get; set; } = null!;

        public List<Usuario> Usuarios { get; set; }

        public async Task<ObservableCollection<ControlMarmitaDTO>> GetControlMarmitumsBuscar(string? Filtro)
        {
            try
            {
                string Route = string.Format("ControlMarmitums/ListaControl?buscar={0}", Filtro);

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
                    var list = JsonConvert.DeserializeObject<ObservableCollection<ControlMarmitaDTO>>(response.Content);

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
