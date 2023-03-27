using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Collections.ObjectModel;
using ShopColibriApp.ViewModels;
using static Xamarin.Essentials.Permissions;
using System.Security.Permissions;
using Plugin.Permissions;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagenPage : ContentPage
    {
        UsuarioViewModel vmu;
        FotoViewModel foto;
        public ImagenPage()
        {
            InitializeComponent();
            BindingContext = new FotoViewModel();
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            
        }
        //Funciones para tomar varias fotos de la galeria
        public async void SelectMultipleImage()
        {
            try
            {
                ObservableCollection<FileImageSource> images = new ObservableCollection<FileImageSource>();
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("No camera", "Camara no habilitada", "OK");
                    return;
                }


                var galeria = new PickMediaOptions();
                galeria.PhotoSize = PhotoSize.Full;
                galeria.CompressionQuality = 30;
                galeria.SaveMetaData = true;

                var file = await CrossMedia.Current.PickPhotosAsync(galeria);
                if (file == null)
                    return;
                foreach (var item in file)
                {
                    images.Add(new FileImageSource() { File = item.Path });
                    ImgProductos.Images = images;
                }


            }catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "OK");
            }
        }
        async void TakeMultiplePhoto()
        {
            

            try
            {
                ObservableCollection<FileImageSource> images = new ObservableCollection<FileImageSource>();

                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return;
                }

                var camara = new StoreCameraMediaOptions();
                camara.DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear;
                camara.PhotoSize = PhotoSize.Full;
                camara.Directory = "ShopColibri";
                camara.Name = "ShopColibri" + DateTime.Now.ToString();
                
                camara.SaveToAlbum = true;
                MediaFile foto = await CrossMedia.Current.TakePhotoAsync(camara);

                if (foto == null)
                    return;
                images.Add(new FileImageSource() { File = foto.Path });
                ImgProductos.Images = images;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "OK");
                Console.WriteLine(ex.ToString());
            }
        }

        private void BtnGaleria_Clicked(object sender, EventArgs e)
        {
            SelectMultipleImage();
        }

        private async void BtnCamara_Clicked(object sender, EventArgs e)
        {
            TakeMultiplePhoto();
        }
    }
}