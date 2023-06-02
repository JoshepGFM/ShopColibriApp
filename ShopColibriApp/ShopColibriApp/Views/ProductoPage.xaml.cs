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
    public partial class ProductoPage : ContentPage
    {
        ProductoViewModel pvm { get; set; }
        public ProductoPage()
        {
            InitializeComponent();
            BindingContext = pvm = new ProductoViewModel();
            DataProduct();
        }

        private void DataProduct()
        {
            if (GlobalObject.GloProd != null)
            {
                TxtNombre.Text = GlobalObject.GloProd.Nombre;
                TxtDescripcion.Text = GlobalObject.GloProd.Descripcion;
                BtnModificar.IsVisible = true;
                LblTituloProducto.Text = "Modificar Producto";
                BtnIngresar.IsVisible = false;
            }
            else
            {
                BtnModificar.IsVisible = false;
                BtnIngresar.IsVisible = true;
            }
        }

        private async void BtnIngresar_Clicked(object sender, EventArgs e)
        {
            if (ValidarElemento())
            {
                string des = "";
                if (TxtDescripcion.Text == null || string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
                {
                    des = null;
                }
                else
                {
                    des = TxtDescripcion.Text.Trim();
                }
                bool R = await pvm.PostProducto(TxtNombre.Text.Trim(), des);
                if (R)
                {
                    await DisplayAlert("Validación exitosa", "Se Ingreso el producto con éxito", "OK");
                    await Navigation.PushAsync(new VistaProductosPage());
                }
                else
                {
                    await DisplayAlert("Error de ingreso", "No se pudo ingresar el producto", "OK");
                }
            }
        }

        private bool ValidarElemento()
        {
            bool R = false;
            if (TxtNombre.Text != null && !string.IsNullOrEmpty(TxtNombre.Text.Trim()))
            {
                R = true;
            }
            else
            {
                if (string.IsNullOrEmpty(TxtNombre.Text.Trim()))
                {
                    DisplayAlert("Error de validación", "Se requiere de un nombre para ingresar un producto", "OK");
                    TxtNombre.Focus();
                    return false;
                }
            }
            return R;
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            if (ValidarElemento())
            {
                string des = "";
                if (TxtDescripcion.Text == null || string.IsNullOrEmpty(TxtDescripcion.Text.Trim()))
                {
                    des = null;
                }
                else
                {
                    des = TxtDescripcion.Text.Trim();
                }
                int Codigo = GlobalObject.GloProd.Codigo;
                bool R = await pvm.PutProducto(Codigo, TxtNombre.Text.Trim(), des);
                if (R)
                {
                    await DisplayAlert("Validación exitosa", "Se Modifico el producto con éxito", "OK");
                    await Navigation.PushAsync(new VistaProductosPage());
                }
                else
                {
                    await DisplayAlert("Error de ingreso", "No se pudo Modificar el producto", "OK");
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new VistaProductosPage());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}