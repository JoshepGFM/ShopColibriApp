using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using System;
using System.IO;
using Android.App;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Xamarin.Forms;
using Google.Apis.Util.Store;
using Xamarin.Auth;
using System.Net;

namespace ShopColibriApp.Servicios
{
    public class GoogleDriveService
    {
        private const string ClientId = "419242195891-o4dn41rln3ms5tnqdiu4ui42mh767f08.apps.googleusercontent.com";
        private const string ClientSecret = "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";
        private static string RedirectUri = "https://colibrishop.github.io/RedirectApp/";
        private const string Scope = "https://www.googleapis.com/auth/drive";

        private TaskCompletionSource<string> _authCodeTcs;

        public async Task<string> UploadImageToDrive()
        {
            try
            {
                var fileResult = await MediaPicker.PickPhotoAsync();

                if (fileResult != null)
                {
                    var driveService = await GetDriveService();
                    var fileId = await UploadImage(driveService, fileResult);
                    return fileId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al seleccionar la imagen: {ex.Message}");
            }

            return null;
        }

        private async Task<DriveService> GetDriveService()
        {
            _authCodeTcs = new TaskCompletionSource<string>();

            var authFlowInitializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                },
                Scopes = new[] { Scope }
            };

            Device.BeginInvokeOnMainThread(async () =>
            {
                await OpenExternalBrowserToGetAuthCode(authFlowInitializer);
            });

            var authCode = await _authCodeTcs.Task;

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                },
                new[] { Scope },
                "user",
                System.Threading.CancellationToken.None,
                new FileDataStore("Tokens", true),
                new LocalServerCodeReceiver()
            );

            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "TuAplicacion"
            });

            return service;
        }

        private async Task OpenExternalBrowserToGetAuthCode(GoogleAuthorizationCodeFlow.Initializer authFlowInitializer)
        {
            var authFlow = new GoogleAuthorizationCodeFlow(authFlowInitializer);
            var authRequest = authFlow.CreateAuthorizationCodeRequest(RedirectUri);

            var authUrl = authRequest.Build().AbsoluteUri;
            await Browser.OpenAsync(authUrl, BrowserLaunchMode.SystemPreferred);

            // Esperar a que se complete la autorización en el navegador externo
            while (string.IsNullOrEmpty(_authCodeTcs.Task.Result))
            {
                await Task.Delay(100);
            }
        }

        private async Task<string> UploadImage(DriveService driveService, FileResult fileResult)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = Path.GetFileName(fileResult.FullPath)
            };

            using (var stream = await fileResult.OpenReadAsync())
            {
                var uploadStream = new MemoryStream();
                await stream.CopyToAsync(uploadStream);

                var request = driveService.Files.Create(fileMetadata, uploadStream, fileResult.ContentType);
                request.Fields = "id";
                var uploadProgress = await request.UploadAsync();
                if (uploadProgress.Status == UploadStatus.Completed)
                {
                    var file = request.ResponseBody;
                    return file.Id;
                }
            }

            return null;
        }
    }
}
