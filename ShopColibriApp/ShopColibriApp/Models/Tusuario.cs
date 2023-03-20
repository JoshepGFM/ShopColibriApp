using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.Models
{
    public class Tusuario
    {
        public RestRequest request { get; set; }
        public Tusuario()
        {
            //Usuarios = new HashSet<Usuario>();
        }
        public int Id { get; set; }

        public string Tipo { get; set; } = null!;

        //public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();

        public async Task<List<Tusuario>> GetTUsuario()
        {

            try
            {
                string Route = string.Format("Tusuarios");
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
                    var list = JsonConvert.DeserializeObject<List<Tusuario>>(response.Content);
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
    }
}
