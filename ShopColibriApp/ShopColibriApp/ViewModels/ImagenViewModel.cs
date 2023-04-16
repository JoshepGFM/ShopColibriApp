using Android.Media;
using Android.Telephony.Mbms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShopColibriApp.ViewModels
{
    public class ImagenViewModel : BaseViewModel
    {
        Imagen MiImagen { get; set; }
        ImagenDrive MiImag { get; set; }
        public ImagenViewModel()
        {
            ValidarConexionInternet();
            MiImagen = new Imagen();
            MiImag = new ImagenDrive();
        }

        public async Task<bool> PostImagen(List<Xamarin.Forms.Image> images,int IdInve)
        {
            if(IsBusy) return false;
            IsBusy = true;
            try
            {
                bool R = false;
                for (int i = 0; i < images.Count; i++)
                {
                    MiImag.Archivo = images[i];
                    R =await MiImag.GuardarImagen();
                }
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }

        public static IFormFile GetIFormFileFromPath(string path)
        {
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(path);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(path);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            return fileMock.Object;
        }
    }
}
