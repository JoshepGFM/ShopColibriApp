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
using ShopColibriApp.Models;
using ShopColibriApp.Servicios;
using Google.Apis.Drive.v3.Data;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagenPage : ContentPage
    {
        UsuarioViewModel vmu;
        FotoViewModel foto;
        Servicios.GoogleDriveService drive;

        ObservableCollection<FileImageSource> imagen { get; set; }
        public ImagenPage()
        {
            InitializeComponent();
            BindingContext = foto = new FotoViewModel();
            drive = new Servicios.GoogleDriveService();
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            drive.UploadImageToDrive();
            if (ImgProductos.Images != null)
            {
                ObservableCollection<FileImageSource> list = new ObservableCollection<FileImageSource>();
                list = ImgProductos.Images;
                foreach (var image in list)
                {
                    Imagen NewItem = new Imagen();

                    NewItem.Imagen1 = image;

                    GlobalObject.GloImagenes.Add(NewItem);
                }
                await Navigation.PushAsync(new InventarioPage());
            }
            else
            {
                await DisplayAlert("Error de validación", "Se tiene seleccionar una imagen para guardar", "OK");
            }
        }

        private async void BtnGaleria_Clicked(object sender, EventArgs e)
        {
            ObservableCollection<FileImageSource> imagen = new ObservableCollection<FileImageSource>();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No camera", "Este dispositivo no permite el uso de la galería", "OK");
                return;
            }
            imagen = await foto.SelectImage();
            ImgProductos.Images = imagen;
        }

        private async void BtnCamara_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camera", "Este dispositivo no permite el uso de la Camara", "OK"); ;
            }
            ImgProductos.Images = await foto.TakePhoto();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new InventarioPage());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}