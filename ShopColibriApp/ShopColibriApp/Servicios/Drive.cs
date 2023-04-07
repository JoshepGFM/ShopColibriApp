
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


        public async Task<UserCredential> GetCredentials()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("CredenDri.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            return credential;
        }
    }
}