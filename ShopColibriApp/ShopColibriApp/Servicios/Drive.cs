
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ShopColibriApp.Servicios
{
    public class Drive
    {
        string[] Scopes = { DriveService.Scope.Drive };
        string AplicationName = "ShopColibriApp";

        public void VerificarAcceso()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("CredenDri.json", FileMode.Open, FileAccess.Read))
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
            if(files != null && files.Count > 0)
            {
                foreach(var file in files)
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