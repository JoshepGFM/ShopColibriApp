using Google.Apis.Drive.v3.Data;
using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventarioPage : ContentPage
    {
        ProductoViewModel pvm { get; set; }

        InventarioViewModel ivm { get; set; }
        Drive Dv { get; set; }
        public InventarioPage()
        {
            InitializeComponent();
            BindingContext = pvm = new ProductoViewModel();
            ivm = new InventarioViewModel();
            ivm.VerificarAccesoDrive();
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
            PckEmpaque.ItemsSource = await pvm.GetProducto();
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
                    //Producto producto = PckProducto.SelectedItem as Producto;
                    int idE = 0;
                    string Origen;
                    if(TxtOrigen.Text == null || string.IsNullOrEmpty(TxtOrigen.Text.Trim()))
                    {
                        Origen = null;
                    }
                    else
                    {
                        Origen = TxtOrigen.Text.Trim();
                    }
                    if (ImgProductos.Images == null)
                    {
                        R = await ivm.PostInventario(DpckFecha.Date,int.Parse(TxtStock.Text.Trim()),
                                                     decimal.Parse(TxtPrecioUni.Text.Trim()),Origen,idP,idE);
                    }
                    else
                    {
                        R = await ivm.PostInventario(DpckFecha.Date, int.Parse(TxtStock.Text.Trim()),
                                                     decimal.Parse(TxtPrecioUni.Text.Trim()), Origen, idP, idE);
                        T = true;
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
                GlobalObject.GLoInventario.EmpaqueId = -1;
            }

            await Navigation.PushAsync(new ImagenPage());
        }

        private void ValidarLlenado()
        {
            if(GlobalObject.GLoInventario != null)
            {
                DpckFecha.Date = GlobalObject.GLoInventario.Fecha;
                TxtStock.Text = GlobalObject.GLoInventario.Stock.ToString();
                TxtPrecioUni.Text = GlobalObject.GLoInventario.PrecioUn.ToString();
                TxtOrigen.Text = GlobalObject.GLoInventario.Origen;
                SeleccionarProducto(GlobalObject.GLoInventario.ProductoCodigo);
                PckEmpaque.SelectedIndex = -1;
                if(GlobalObject.GloImagenes != null)
                {
                    ImgProductos.Images = GlobalObject.GloImagenes;
                }
            }
        }

        private async void SeleccionarProducto(int codigo)
        {
            var list = await pvm.GetProducto();
            for(int i = 0; i > list.Count; ++i)
            {
                if (list[i].Codigo == codigo)
                {
                    PckProducto.SelectedIndex = i;
                }
            }
        }

        private bool ValidarCampos()
        {
            bool R = false;
            if(DpckFecha.Date != null &&
               TxtStock.Text != null && !string.IsNullOrEmpty(TxtStock.Text.Trim()) &&
               TxtPrecioUni.Text != null && !string.IsNullOrEmpty(TxtPrecioUni.Text.Trim()) &&
               PckProducto.SelectedIndex != -1 && PckEmpaque.SelectedIndex != -1)
            {
                R = true;
            }
            else
            {
                if (string.IsNullOrEmpty(TxtStock.Text.Trim()))
                {
                    DisplayAlert("", "","Ok");
                    TxtStock.Focus();
                    return false;
                }
                if (string.IsNullOrEmpty(TxtPrecioUni.Text.Trim()))
                {
                    DisplayAlert("", "", "Ok");
                    TxtPrecioUni.Focus();
                    return false;
                }
                if (PckProducto.SelectedIndex == -1)
                {
                    TxtStock.Focus();
                    DisplayAlert("", "", "Ok");
                    return false;
                }
                if (PckEmpaque.SelectedIndex == -1)
                {
                    TxtStock.Focus();
                    DisplayAlert("", "", "Ok");
                    return false;
                }
            }
            return R;
        }
    }
}