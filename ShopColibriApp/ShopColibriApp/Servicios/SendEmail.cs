using RestSharp;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.Servicios
{
    public class SendEmail
    {
        public RestRequest request { get; set; }
        public async Task<bool> EnviarEmail(string receptor, string asunto, string contenido = "", bool esUrl = false, string urlConfirmacion = "")
        {
            try
            {
                string Route = string.Format("SendEmail/SendEmail?receptor={0}&asunto={1}&contenido={2}&esUrl={3}&urlConfirmacion={4}", receptor, asunto, contenido, esUrl, urlConfirmacion);

                string FinalUrl = CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalUrl);

                request = new RestRequest(FinalUrl, Method.Post);

                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

                RestResponse response = await client.ExecuteAsync(request);

                HttpStatusCode statusCode = response.StatusCode;

                if (statusCode == HttpStatusCode.OK)
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
