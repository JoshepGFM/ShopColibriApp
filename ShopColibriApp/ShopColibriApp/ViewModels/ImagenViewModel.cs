using Android.Media;
using Android.Telephony.Mbms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ImagenViewModel()
        {
            ValidarConexionInternet();
            MiImagen = new Imagen();
        }

        public async Task<bool> PostImagen(List<IFormFile> images,int IdInve)
        {
            if(IsBusy) return false;
            IsBusy = true;
            try
            {
                bool R = false;
                foreach (var proces in images)
                {
                    MiImagen.InventarioId = IdInve;
                    R = await MiImagen.PostImagen(proces);
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

        public async Task<bool> DeleteImagen(ObservableCollection<Imagen> imagens)
        {
            try
            {
                bool R = false;
                List<int> list = new List<int>();
                foreach (var imagen in imagens)
                {

                    list.Add(imagen.Id);
                }
                R = await MiImagen.DeleteImagen(list);
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
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
