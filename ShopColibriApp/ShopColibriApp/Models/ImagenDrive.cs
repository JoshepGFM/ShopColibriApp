//using Android.Media;
using Android.Graphics;
using Android.Runtime;
using Java.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShopColibriApp.Models
{
    public class ImagenDrive
    {
        public class Base64IFormFileConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                // Indica que este convertidor solo se aplica a propiedades de tipo IFormFile.
                return objectType == typeof(IFormFile);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.Value == null)
                {
                    return null;
                }

                // Lee el valor de la propiedad del JSON.
                var base64String = reader.Value.ToString();

                // Convierte la cadena Base64 a bytes.
                var fileBytes = Convert.FromBase64String(base64String);

                // Crea una nueva instancia de IFormFile a partir de los bytes.
                var fileName = "file_" + Guid.NewGuid().ToString("N");
                var fileStream = new MemoryStream(fileBytes);
                var formFile = new FormFile(fileStream, 0, fileBytes.Length, fileName, fileName);

                return formFile;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                // Obtiene el archivo enviado desde el formulario.
                var formFile = (IFormFile)value;

                // Carga los bytes del archivo en un MemoryStream.
                byte[] fileBytes;
                using (var stream = new MemoryStream())
                {
                    formFile.CopyTo(stream);
                    fileBytes = stream.ToArray();
                }

                // Convierte los bytes del archivo a Base64 y escribe el valor en el JSON.
                var base64String = Convert.ToBase64String(fileBytes);
                writer.WriteValue(base64String);
            }
        }
        public RestRequest request { get; set; }
        [JsonConverter(typeof(Base64IFormFileConverter))]
        public IFormFile Archivo { get; set; }

        public async Task<bool> GuardarImagen()
        {
            try
            {
                string Route = string.Format("Imagens/GuardarImagen");
                string FinalURL = Servicios.CnnToShopColibri.UrlProduction + Route;

                RestClient client = new RestClient(FinalURL);

                request = new RestRequest(FinalURL, Method.Post);

                //info de seguridad del api
                request.AddHeader(Servicios.CnnToShopColibri.ApiKeyName, Servicios.CnnToShopColibri.ApiValue);
                request.AddHeader(Servicios.CnnToShopColibri.contentType, Servicios.CnnToShopColibri.mimetype);

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

        public string SerializeObject(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public string ConvertToJson(IFormFile file)
        {
            string jsonString;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                jsonString = reader.ReadToEnd();
            }
            return jsonString;
        }

        
    }
}
