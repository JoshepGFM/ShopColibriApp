using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.Models
{
    public class Empaque
    {
        RestRequest request { get; set; }
        public Empaque()
        {
            //FechaIngres = new HashSet<FechaIngre>();
            //Inventarios = new HashSet<Inventario>();
        }
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Tamannio { get; set; } = null!;
        public int Stock { get; set; }
        //public virtual ICollection<FechaIngre> FechaIngres { get; } = new List<FechaIngre>();
        //public virtual ICollection<Inventario> Inventarios { get; } = new List<Inventario>();

        public async Task<bool> PostEmpaque()
        {
            try
            {
                string Route = string.Format("Empaques");
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
