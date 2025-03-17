using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ShopColibriApp.Models
{
    public class Imagen
    {
        public RestRequest request { get; set; }
        public int Id { get; set; }

        public string Imagen1 { get; set; } = null!;

        public int InventarioId { get; set; }

        //public virtual Inventario Inventario { get; set; } = null!;

        public async Task<bool> PostImagen(IFormFile image)
        {
            try
            {
                string imagen = IFormFileToBase64(image);
                var fileName = image.FileName;
                var contentType = "image/jpeg";
                string Route = string.Format("Imagens/GuarImagen?fileName={0}&contentType={1}&idInve={2}", fileName, contentType, this.InventarioId);
                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Post);

                //info de seguridad del api
                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

                request.AddParameter(Servicios.CnnToShopColibri.mimetype, imagen, ParameterType.RequestBody);

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

        public async Task<bool> DeleteImagen(List<int> ids)
        {
            try
            {
                string Route = string.Format("Imagens/EliminarMasivo?imagenes={0}", ids);
                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Delete);

                //info de seguridad del api
                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

                //Se transforma a Json para la api
                string SerialClass = JsonConvert.SerializeObject(ids);
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
        #region Herraminta
        private static string IFormFileToBase64(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }
        #endregion
    }
}
