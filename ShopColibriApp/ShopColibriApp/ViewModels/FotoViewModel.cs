using Plugin.Media.Abstractions;
using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ShopColibriApp.ViewModels
{
    public class FotoViewModel:FotoModel
    {
        public Command CapturarComando { get; set; }

        public FotoViewModel() 
        {
            CapturarComando = new Command(tomarFoto);
        }
        private async void tomarFoto()
        {
            try
            {
                var camara = new StoreCameraMediaOptions();
                camara.DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front;
                camara.PhotoSize = PhotoSize.Full;
                camara.Directory = "ShopColibri";
                camara.Name = "ShopColibri" + DateTime.Now.ToString();
                camara.SaveToAlbum = true;
                var foto = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(camara);

                if (foto != null)
                {
                    Fotico = ImageSource.FromStream(() =>
                    {
                        return foto.GetStream();
                    });
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
