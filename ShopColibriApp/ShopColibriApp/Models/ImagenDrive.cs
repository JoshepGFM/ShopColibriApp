//using Android.Media;
using Android.Graphics;
using Android.Runtime;
using Microsoft.AspNetCore.Http;
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
        [Preserve(AllMembers = true)]
        internal class Base64ImageConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(byte[]);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JToken token = JToken.Load(reader);
                return Convert.FromBase64String(token.ToString());
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(Convert.ToBase64String((byte[])value));
            }
        }
        //public class Base64IFormFileConverter : JsonConverter<IFormFile>
        //{
        //    public override IFormFile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public override void Write(Utf8JsonWriter writer, IFormFile value, JsonSerializerOptions options)
        //    {
        //        using (var ms = new MemoryStream())
        //        {
        //            value.CopyTo(ms);
        //            var fileBytes = ms.ToArray();
        //            writer.WriteStringValue(Convert.ToBase64String(fileBytes));
        //        }
        //    }
        //}
        //public class ImageConverter : JsonConverter
        //{
        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        var base64 = (string)reader.Value;
        //        // convert base64 to byte array, put that into memory stream and feed to image
        //        return Image.FromStream(new MemoryStream(Convert.FromBase64String(base64)));
        //    }

        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        var image = (Image)value;
        //        var ms = new MemoryStream();
        //        image.Save(ms, image.RawFormat);
        //        byte[] imageBytes = ms.ToArray();
        //        writer.WriteValue(imageBytes);
        //    }

        //    public override bool CanConvert(Type objectType)
        //    {
        //        return objectType == typeof(Image);
        //    }
        //}
        public RestRequest request { get; set; }
        [JsonConverter(typeof(Base64ImageConverter))]
        public Image Archivo { get; set; }

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
