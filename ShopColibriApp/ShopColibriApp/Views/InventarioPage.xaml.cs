using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Http;
using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Provider.Telephony.Mms;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventarioPage : ContentPage
    {
        ProductoViewModel pvm { get; set; }
        EmpaqueViewModel evm { get; set; }
        InventarioViewModel ivm { get; set; }
        ImagenViewModel Imvm { get; set; }
        Drive Dv { get; set; }
        public InventarioPage()
        {
            InitializeComponent();
            BindingContext = pvm = new ProductoViewModel();
            BindingContext = evm = new EmpaqueViewModel();
            BindingContext = ivm = new InventarioViewModel();
            BindingContext = Imvm = new ImagenViewModel();
            
            CargarProductos();
            CargarEmpaques();
            ValidarLlenado();
            ValidarBotones();
        }

        private async void CargarProductos()
        {
            PckProducto.ItemsSource = await pvm.GetProducto();
        }

        private async void CargarEmpaques()
        {
            List<Empaque> list = new List<Empaque>();
            list = await evm.GetEmpaque();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Nombre += " " + list[i].Tamannio;
            }
            PckEmpaque.ItemsSource = list;
        }

        private void BtnMenos_Clicked(object sender, EventArgs e)
        {
            int n;
            if (TxtStock.Text == null || TxtStock.Text == "")
            {
                n = 0;
            }
            else
            {
                n = int.Parse(TxtStock.Text);
            }
            if (n < 0)
            {
                n = 0;
            }else if (n >= 1)
            {
                n -= 1;
            }
            TxtStock.Text = n.ToString();
            ValidarBotones();
        }

        private void BtnMas_Clicked(object sender, EventArgs e)
        {
            int n;
            if (TxtStock.Text == null || TxtStock.Text == "")
            {
                n = 0;
            }
            else
            {
                n = int.Parse(TxtStock.Text);
            };
            if (n >= 0 && n < 10000)
            {
                n += 1;
            }
            TxtStock.Text = n.ToString();
            ValidarBotones();
        }

        private void ValidarBotones()
        {
            int n;
            if (TxtStock.Text == null || TxtStock.Text == "")
            {
                n = 0;
            }
            else
            {
                n = int.Parse(TxtStock.Text);
            }
            if ( n == 0)
            {
                BtnMenos.IsEnabled = false;
            }
            else if (n > 0)
            {
                BtnMenos.IsEnabled = true;
            }
            if (n >= 9999)
            {
                BtnMas.IsEnabled = false;
            }
            else if (n < 10000)
            {
                BtnMas.IsEnabled = true;
            }
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            bool R = false;
            bool T = false;
            if (ValidarCampos())
            {
                if (await DisplayAlert("Validación", "Esta seguro de no agregar una imagen?", "Si", "No"))
                {
                    Producto producto = PckProducto.SelectedItem as Producto;
                    int idP = producto.Codigo;
                    Empaque empaque = PckEmpaque.SelectedItem as Empaque;
                    int idE = empaque.Id;
                    if (ImgProductos.Images == null)
                    {
                        R = await ivm.PostInventario(DpckFecha.Date,int.Parse(TxtStock.Text.Trim()),
                                                     decimal.Parse(TxtPrecioUni.Text.Trim()), TxtOrigen.Text.Trim(), idP,idE);
                    }
                    else
                    {
                        //R = await ivm.PostInventario(DpckFecha.Date, int.Parse(TxtStock.Text.Trim()),
                        //                             decimal.Parse(TxtPrecioUni.Text.Trim()), TxtOrigen.Text.Trim(), idP, idE);
                        List<Image> images = new List<Image>();
                        images = TransformaImagen(ImgProductos.Images);
                        List<Android.Media.Image> Lista = new List<Android.Media.Image>();
                        int idIn = await ivm.GetUltimoID();
                        T = await Imvm.PostImagen(images,idIn);
                        //TODO: Metodo para guardar imagenes
                    }
                    if (R)
                    {
                        await DisplayAlert("Validación exitosa", "Se guardo el Inventario correctamente","OK");
                        if (!T)
                        {
                            await DisplayAlert("Error Guardado", "Se tubo problemas al guardar la imagen", "OK");
                        }
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error de validación", "No se a podido guardar el inventario", "OK");
                    }
                }
            }
            
        }

        private async void BtnImagen_Clicked(object sender, EventArgs e)
        {
            Producto producto = PckProducto.SelectedItem as Producto;
            Empaque empaque = PckEmpaque.SelectedItem as Empaque;

            GlobalObject.GloImagenes.Clear();
            GlobalObject.GLoInventario.Fecha = DpckFecha.Date;
            if (TxtStock.Text == null)
            {
                GlobalObject.GLoInventario.Stock = 0;
            }
            else
            {
                GlobalObject.GLoInventario.Stock = int.Parse(TxtStock.Text);
            }
            if (TxtPrecioUni.Text == null)
            {
                GlobalObject.GLoInventario.PrecioUn = 0;
            }
            else
            {
                GlobalObject.GLoInventario.PrecioUn = int.Parse(TxtPrecioUni.Text);
            }
            if (TxtOrigen.Text == null)
            {
                GlobalObject.GLoInventario.Origen = null;
            }
            else
            {
                GlobalObject.GLoInventario.Origen = TxtOrigen.Text;
            }
            if (PckProducto.SelectedIndex == -1)
            {
                GlobalObject.GLoInventario.ProductoCodigo = -1;
            }
            else
            {
                GlobalObject.GLoInventario.ProductoCodigo = producto.Codigo;
            }
            if (PckEmpaque.SelectedIndex == -1)
            {
                GlobalObject.GLoInventario.EmpaqueId = -1;
            }
            else
            {
                GlobalObject.GLoInventario.EmpaqueId = empaque.Id;
            }

            await Navigation.PushAsync(new ImagenPage());
        }

        private void ValidarLlenado()
        {
            if (GlobalObject.GloInven_DTO.Id != 0)
            {
                GlobalObject.GLoInventario.Id = GlobalObject.GloInven_DTO.Id;
                GlobalObject.GLoInventario.Fecha = GlobalObject.GloInven_DTO.Fecha;
                GlobalObject.GLoInventario.Stock = GlobalObject.GloInven_DTO.Stock;
                GlobalObject.GLoInventario.PrecioUn = GlobalObject.GloInven_DTO.PrecioUn;
                GlobalObject.GLoInventario.Origen = GlobalObject.GloInven_DTO.Origen;
                GlobalObject.GLoInventario.ProductoCodigo = GlobalObject.GloInven_DTO.ProductoCodigo;
                GlobalObject.GLoInventario.EmpaqueId = GlobalObject.GloInven_DTO.EmpaqueId;
                BtnGuardar.IsVisible = false;
                BtnModificar.IsVisible = true;
            }
            if(GlobalObject.GLoInventario != null)
            {
                DpckFecha.Date = GlobalObject.GLoInventario.Fecha;
                LblStock.Text = GlobalObject.GLoInventario.Stock.ToString();
                TxtPrecioUni.Text = GlobalObject.GLoInventario.PrecioUn.ToString();
                TxtOrigen.Text = GlobalObject.GLoInventario.Origen;
                int idP = GlobalObject.GLoInventario.ProductoCodigo;
                int idE = GlobalObject.GLoInventario.EmpaqueId;
                SeleccionarProducto(idP);
                SeleccionarEmpaque(idE);
                if(GlobalObject.GloImagenes.Count > 0)
                {
                    FmImagen.IsVisible = true;
                    ImgProductos.Images = GlobalObject.GloImagenes;
                }
                else
                {
                    FmImagen.IsVisible = false;
                }
            }
        }

        private async void SeleccionarProducto(int codigo)
        {
            var list = await pvm.GetProducto();
            for(int i = 0; i < list.Count; ++i)
            {
                if (list[i].Codigo.ToString() == codigo.ToString())
                {
                    PckProducto.SelectedIndex = i;
                }
            }
        }

        private async void SeleccionarEmpaque(int id)
        {
            var list = await evm.GetEmpaque();
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].Id == id)
                {
                    PckEmpaque.SelectedIndex = i;
                }
            }
        }

        private bool ValidarCampos()
        {
            bool R = false;
            if(DpckFecha.Date != null &&
               TxtStock.Text != null && !string.IsNullOrEmpty(TxtStock.Text.Trim()) &&
               TxtPrecioUni.Text != null && !string.IsNullOrEmpty(TxtPrecioUni.Text.Trim()) &&
               TxtOrigen.Text != null && !string.IsNullOrEmpty(TxtOrigen.Text.Trim()) &&
               PckProducto.SelectedIndex != -1 && PckEmpaque.SelectedIndex != -1)
            {
                R = true;
            }
            else
            {
                if (PckProducto.SelectedIndex == -1)
                {
                    TxtStock.Focus();
                    DisplayAlert("Error de validación", "Se requiere seleccionar un producto", "Ok");
                    return false;
                }
                if (string.IsNullOrEmpty(TxtStock.Text.Trim()))
                {
                    DisplayAlert("Error de validación", "Se requiere de un numero del 0 al 1 en el stock","Ok");
                    TxtStock.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(TxtPrecioUni.Text.Trim()))
                {
                    DisplayAlert("Error de validación", "Se requiere un precio para ingresar el inventario", "Ok");
                    TxtPrecioUni.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(TxtOrigen.Text.Trim()))
                {
                    DisplayAlert("Error de validación", "Se requiere de un Origen del producto a inventariar", "Ok");
                    TxtOrigen.Focus();
                    return false;
                }
                if (PckEmpaque.SelectedIndex == -1)
                {
                    TxtStock.Focus();
                    DisplayAlert("Error de validación", "Se requiere seleccionar un producto", "Ok");
                    return false;
                }
            }
            return R;
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            bool R = false;
            bool T = false;
            if (ValidarCampos())
            {
                if (await DisplayAlert("Validación", "Esta seguro de Modificar el Inventario?", "Si", "No"))
                {
                    Producto producto = PckProducto.SelectedItem as Producto;
                    int idP = producto.Codigo;
                    Empaque empaque = PckEmpaque.SelectedItem as Empaque;
                    int idE = empaque.Id;
                    if (ImgProductos.Images == null)
                    {
                        //R = await ivm.PutInventario(DpckFecha.Date, int.Parse(TxtStock.Text.Trim()),
                        //                             decimal.Parse(TxtPrecioUni.Text.Trim()), TxtOrigen.Text.Trim(), idP, idE);
                    }
                    else
                    {
                        //R = await ivm.PutInventario(DpckFecha.Date, int.Parse(TxtStock.Text.Trim()),
                        //                             decimal.Parse(TxtPrecioUni.Text.Trim()), TxtOrigen.Text.Trim(), idP, idE);
                        T = true;
                        //TODO: Metodo para guardar imagenes
                    }
                    if (R)
                    {
                        await DisplayAlert("Validación exitosa", "Se modifico el Inventario correctamente", "OK");
                        if (!T)
                        {
                            await DisplayAlert("Error Guardado", "Se tubo problemas al guardar la imagen", "OK");
                        }
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error de validación", "No se a podido modificar el Inventario", "OK");
                    }
                }
            }
        }

        private List<Image> TransformaImagen(ObservableCollection<FileImageSource> Lista)
        {
            List<Image> list = new List<Image>();
            foreach (FileImageSource item in Lista)
            {
                list.Add(new Image { Source = item });
            }
            return list;
        }

        
    }
}