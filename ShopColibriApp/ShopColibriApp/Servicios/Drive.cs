
using Android.Content;
using Android.Net.Wifi.Hotspot2.Pps;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ShopColibriApp.Servicios
{
    public class Drive
    {
        string[] Scopes = { DriveService.Scope.DriveFile };
        string jsonFilePath = GetJsonFilePath();

        private static string GetJsonFilePath()
        {
            string fileName = "CredenDri.json";
            return Path.Combine(FileSystem.AppDataDirectory, fileName);
        }

        private string ApplicationName = "ShopColibriApp";
        private UserCredential credential;

        private UserCredential Credential;
        private DriveService Service;

        private string CredentialFileName = "Crede.json";
        private string CredentialFolderName = "ImagIventario";

        private DriveService GetService1()
        {
            try
            {
                string jsonFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Assets", "CredenDri.json");
                string resourceName = Assembly.GetExecutingAssembly().GetName().Name + ".Crede.json";

                using (var stream = new FileStream(jsonFilePath, FileMode.Open, FileAccess.Read))
                {
                    string creadPath = Path.Combine(FileSystem.AppDataDirectory, "token.json");
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

            }catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
                var initializer = new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                };

                var service = new DriveService(initializer);
                return service;
        }

        public UserCredential GetService()
        {
            try
            {
                using (Stream stream = new FileStream(jsonFilePath, FileMode.Open, FileAccess.Read))
                {
                    string credPath = Path.Combine(FileSystem.CacheDirectory, ".credentials");
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "usuario",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)
                    ).Result;
                }

                return credential;
            }catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Authorize()
        {
            try
            {
                using (var stream = await FileSystem.OpenAppPackageFileAsync(CredentialFileName))
                {
                    Credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(CredentialFolderName, true));
                }

                var initializer = new BaseClientService.Initializer()
                {
                    HttpClientInitializer = Credential,
                    ApplicationName = "YourApplicationName",
                };
                Service = new DriveService(initializer);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}