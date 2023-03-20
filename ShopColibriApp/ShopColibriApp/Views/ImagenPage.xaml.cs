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

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagenPage : ContentPage
    {
        UsuarioViewModel vmu;
        public ImagenPage()
        {
            InitializeComponent();
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            
        }
        //Funcion para tomar una foto de la galeria
        private async void ObtenerImagen()
        {
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });
            if (file == null) return;

            //Imagen1.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    return stream;
            //});


        }
        //Funcion para tomar una foto con la camara
        async void TomarFoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            //Imagen1.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    file.Dispose();
            //    return stream;
            //});
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
                    await DisplayAlert("No camera", "No camera available", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickPhotosAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 30,
                    SaveMetaData = true
                });

                foreach (var item in file)
                {
                    images.Add(new FileImageSource() { File = item.Path });
                }

                ImgProductos.Images = images;

            }catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "OK");
            }
        }
        //Funcion para tomar varias fotos por la camara
        async Task<List<string>> TakePhoto()
        {
            var photos = new List<string>();
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camera", "No camera available", "OK");
                return photos;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 30,
                SaveMetaData = true
            });

            if (file == null)
                return photos;

            photos.Add(file.Path);
            return photos;

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
                MediaFile file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                    Directory = "ShopColibri",
                    Name = "ShopColibri" + DateTime.Now.ToString(),
                    SaveToAlbum = true,
                }); ;

                if (file == null)
                    return;
                images.Add(new FileImageSource { File = file.Path });
                ImgProductos.Images = images;
                await DisplayAlert("File Location", file.Path, "OK");

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "OK");
            }
            
        }

        private void BtnGaleria_Clicked(object sender, EventArgs e)
        {
            SelectMultipleImage();
        }

        private void BtnCamara_Clicked(object sender, EventArgs e)
        {
            TakeMultiplePhoto();
        }
    }
}