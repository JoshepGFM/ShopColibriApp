
using Android.Net.Wifi.Hotspot2.Pps;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Java.Security.Cert;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShopColibriApp.Servicios
{
    public class Drive
    {
        private string[] Scopes = { DriveService.Scope.Drive };
        private string AplicationName = "ShopColibriApp";

        public void VerificarAcceso()
        {
            UserCredential credential;

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Drive)).Assembly;
            Stream Json = assembly.GetManifestResourceStream("ShopColibriApp.CredenDri.json");
            string StreamReader = new StreamReader(Json).ReadToEnd();

            using (var stream = new FileStream("CredenDri.json", FileMode.Open, FileAccess.Read))
            {
                string creadPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(creadPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + creadPath);
            }

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = AplicationName,
            });

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.Read();
        }
    }
}