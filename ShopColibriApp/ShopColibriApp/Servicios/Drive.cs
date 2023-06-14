
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
        private string ApplicationName = "ShopColibriApp";
        private UserCredential credential;

        private DriveService GetService()
        {

            using (var stream = new FileStream("Properties/CredenDri.json", FileMode.Open, FileAccess.Read))
            {
                string creadPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(creadPath, true)).Result;
            }

            if (credential.Token.IsExpired(credential.Flow.Clock))
            {
                if (credential.RefreshTokenAsync(CancellationToken.None).Result)
                {
                    Console.WriteLine("Token de acceso actualizado.");
                }
                else
                {
                    Console.WriteLine("Error al actualizar el token de acceso.");
                }
            }
            else
            {
                Console.WriteLine("El token de acceso aún es válido.");
            }

            //var service = new DriveService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = ApplicationName,
            //});
            //Crear el servicio de Google Drive usando el token actualizado
            var service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }

        public DriveService Credencial()
        {
            DriveService service = new DriveService();
            service = GetService();
            return service;
        }
    }
}