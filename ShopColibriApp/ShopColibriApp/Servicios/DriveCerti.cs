using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.IO;
using Android.Content;
using Android.Net.Wifi;
using Xamarin.Forms;

namespace ShopColibriApp.Servicios
{
    public class DriveCerti 
    {
        public async Task<string> SelectImageFromDrive1()
        {
            var accessToken = await CustomAuthenticator.AuthenticateAsync();

            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("No se pudo obtener el token de acceso.");
                return null;
            }

            // Resto de la lógica para seleccionar la imagen desde Google Drive

            return null;
        }

        private const string ApiBaseUrl = "https://www.googleapis.com/drive/v3";
        private const string UploadEndpoint = "https://www.googleapis.com/upload/drive/v3/files";
        private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private static string IPActual = IPAddressHelper.GetLocalIPAddress();
        private static string RedirectUri = "http://localhost";

        public async Task<string> SelectImageFromDrive()
        {
            try
            {
                var accessToken = await GetAccessToken();

                if (string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine("No se pudo obtener el token de acceso.");
                    return null;
                }

                var file = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Seleccionar imagen desde Google Drive",
                    FileTypes = FilePickerFileType.Images
                });

                if (file != null)
                {
                    var fileMetadata = new Dictionary<string, string>
            {
                { "name", file.FileName },
                { "parents", "root" }
            };

                    var uploadUrl = $"{UploadEndpoint}?uploadType=multipart";

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        using (var content = new MultipartFormDataContent())
                        {
                            var fileBytes = await GetFileBytes(file.FullPath);
                            var fileContent = new ByteArrayContent(fileBytes);
                            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

                            foreach (var entry in fileMetadata)
                            {
                                content.Add(new StringContent(entry.Value), $"\"{entry.Key}\"");
                            }

                            content.Add(fileContent, "\"file\"", $"\"{file.FileName}\"");

                            var response = await httpClient.PostAsync(uploadUrl, content);

                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                Console.WriteLine(responseContent);

                                // Extrae el ID del archivo de la respuesta
                                var fileId = ExtractFileId(responseContent);

                                return fileId;
                            }
                            else
                            {
                                Console.WriteLine($"Error de carga: {response.StatusCode}");
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

            return null;
        }

        private static async Task<string> GetAccessToken()
        {
            try
            {
                var authUri = new Uri("https://accounts.google.com/o/oauth2/v2/auth/oauthchooseaccount?access_type=offline&response_type=code&client_id=419242195891-ea1mlscshis75f1hor5c1ts73jv76g42.apps.googleusercontent.com&redirect_uri=http%3A%2F%2F127.0.0.1%3A50669%2Fauthorize%2F&scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fdrive&service=lso&o2v=2&flowName=GeneralOAuthFlow");

                var authResult = await WebAuthenticator.AuthenticateAsync(
                    authUri,
                    new Uri(RedirectUri)
                );

                if (authResult != null && !string.IsNullOrEmpty(authResult.AccessToken))
                {
                    return authResult.AccessToken;
                }
            }catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
            return null;
        }

        private static string GetAuthorizationUri()
        {
            var parameters = new Dictionary<string, string>
        {
            {"access_type", "offline"},
            { "response_type", "code" },
            { "client_id", "419242195891-ea1mlscshis75f1hor5c1ts73jv76g42.apps.googleusercontent.com" },
            { "redirect_uri", RedirectUri },
            { "scope", "https://www.googleapis.com/auth/drive.file" },
        };

            var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));

            return $"{AuthorizationEndpoint}?{queryString}";
        }

        private static string ExtractFileId(string responseContent)
        {
            // Analiza la respuesta JSON para extraer el ID del archivo
            // Puedes usar una biblioteca de serialización JSON como Newtonsoft.Json para esto
            // En este ejemplo, se realiza un análisis simple de la cadena JSON
            var startIndex = responseContent.IndexOf("\"id\": \"") + 7;
            var endIndex = responseContent.IndexOf("\"", startIndex);

            return responseContent.Substring(startIndex, endIndex - startIndex);
        }

        private static async Task<byte[]> GetFileBytes(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }

    public class CustomAuthenticator
    {
        public static async Task<string> AuthenticateAsync()
        {
            try
            {
                var authUri = new Uri("https://accounts.google.com/o/oauth2/auth");
                var redirectUri = new Uri("http://localhost");

                var authResult = await WebAuthenticator.AuthenticateAsync(authUri, redirectUri);

                if (authResult != null && !string.IsNullOrEmpty(authResult.AccessToken))
                {
                    // Autenticación exitosa, puedes obtener el token de acceso aquí
                    var accessToken = authResult.AccessToken;
                    return accessToken;
                }

                // Autenticación fallida
                return null;
            }catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }

    public class IPAddressHelper
    {
        public static string GetLocalIPAddress()
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                return null;
            }

            var profiles = Connectivity.ConnectionProfiles;

            if (profiles.Contains(ConnectionProfile.WiFi))
            {
                var wifiIpInfo = GetWifiIpInfo();
                return wifiIpInfo?.IpAddress;
            }
            else if (profiles.Contains(ConnectionProfile.Cellular))
            {
                var cellIpInfo = GetCellularIpInfo();
                return cellIpInfo?.IpAddress;
            }

            return null;
        }

        private static IpAddressInfo GetWifiIpInfo()
        {
            var wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
            var wifiInfo = wifiManager.ConnectionInfo;
            var ipAddress = wifiInfo.IpAddress;

            return new IpAddressInfo(ipAddress);
        }

        private static IpAddressInfo GetCellularIpInfo()
        {
            // Puedes utilizar otras APIs para obtener la dirección IP en la red celular (por ejemplo, TelephonyManager)

            return null;
        }
    }

    public class IpAddressInfo
    {
        public string IpAddress { get; }

        public IpAddressInfo(int ipAddress)
        {
            var byteAddress = BitConverter.GetBytes(ipAddress);
            IpAddress = $"{byteAddress[0]}.{byteAddress[1]}.{byteAddress[2]}.{byteAddress[3]}";
        }
    }
}

