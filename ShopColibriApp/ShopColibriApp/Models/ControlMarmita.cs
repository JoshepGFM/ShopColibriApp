using Android.Text.Format;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.Models
{
    public class ControlMarmita
    {
        public RestRequest request { get; set; }
        public int Codigo { get; set; }

        public DateTime Fecha { get; set; }

        public TimeSpan HoraEn { get; set; }

        public TimeSpan HoraAp { get; set; }

        public int Temperatura { get; set; }

        public string IntensidadMov { get; set; } = null!;

        public string Lote { get; set; } = null!;

        public List<int> idUsuario { get; set; }

        //public virtual ICollection<Usuario> UsuarioIdUsuarios { get; } = new List<Usuario>();

        public async Task<bool> PostControlMar()
        {
            try
            {
                string Route = string.Format("ControlMarmitums");
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
