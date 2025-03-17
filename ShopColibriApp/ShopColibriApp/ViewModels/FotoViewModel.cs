using Plugin.Media;
using Plugin.Media.Abstractions;
using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ShopColibriApp.ViewModels
{
    public class FotoViewModel:FotoModel
    {
        public FotoViewModel() 
        {

        }

        public async Task<ObservableCollection<FileImageSource>> SelectImage()
        {
            try
            {
                ObservableCollection<FileImageSource> images = new ObservableCollection<FileImageSource>();
                await CrossMedia.Current.Initialize();

                var galeria = new PickMediaOptions();
                galeria.PhotoSize = PhotoSize.Full;
                galeria.CompressionQuality = 30;
                galeria.SaveMetaData = true;

                var file = await CrossMedia.Current.PickPhotosAsync(galeria);
                if (file == null)
                    return null;
                foreach (var item in file)
                {
                    images.Add(new FileImageSource() { File = item.Path });
                };
                return images;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        public async Task<ObservableCollection<FileImageSource>> TakePhoto()
        {


            try
            {
                ObservableCollection<FileImageSource> images = new ObservableCollection<FileImageSource>();

                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return null;
                }

                var camara = new StoreCameraMediaOptions();
                camara.DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear;
                camara.PhotoSize = PhotoSize.Full;
                camara.Directory = "ShopColibri";
                camara.Name = "ShopColibri" + DateTime.Now.ToString();

                camara.SaveToAlbum = true;
                MediaFile foto = await CrossMedia.Current.TakePhotoAsync(camara);

                if (foto == null)
                    return null;
                images.Add(new FileImageSource() { File = foto.Path });
                return images;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }


    }
}
