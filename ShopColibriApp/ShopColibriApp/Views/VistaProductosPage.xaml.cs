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
    public partial class VistaProductosPage : ContentPage
    {
        ProductoViewModel pvm { get; set; }
        ViewModelBitacora vmb { get; set; }
        private string? Filtre { get; set; }
        public VistaProductosPage()
        {
            InitializeComponent();
            vmb = new ViewModelBitacora();
            BindingContext = pvm = new ProductoViewModel();
            CargarListaProducto();
        }

        private async void CargarListaProducto()
        {
            LvlListarProducto.ItemsSource = await pvm.GetBuscarProducto(Filtre);
        }

        private void SbBuscarPro_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtre = SbBuscarPro.Text;
            CargarListaProducto();
        }

        private async void LvlListarProducto_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Producto ItemSelect = e.SelectedItem as Producto;
            if (ItemSelect != null)
            {
                GlobalObject.GloProd = ItemSelect;
                Navigation.PushAsync(new ProductoPage());
            }
        }

        private void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloProd = null;
            Navigation.PushAsync( new ProductoPage());
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Confirmación", "¿Esta seguro de eliminar este Producto?","Si","No"))
            {
                var item = (sender as MenuItem).CommandParameter;
                int id = int.Parse(item.ToString());
                Producto producto = new Producto();
                producto = await pvm.GetProductoId(id);
                bool R = await pvm.deleteProducto(id);
                if (R)
                {
                    CargarListaProducto();
                    await DisplayAlert("Verificación","Se elimino el producto con éxito","OK");
                    await vmb.PostBitacora(DateTime.Now, GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2 +
                          " Elimino un Producto. Producto: " + producto.Nombre);
                }
                else
                {
                    await DisplayAlert("Error de verificación", "No se logro elimino el producto", "OK");
                }
            }
        }

        private void LvlListarProducto_Refreshing(object sender, EventArgs e)
        {
            CargarListaProducto();
            LvlListarProducto.IsRefreshing = false;
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}