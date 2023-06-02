using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaSalidas : ContentPage
    {
        ViewModelBitacoraSalida vmb { get; set; }
        ProductoViewModel pvm { get; set; }
        public VistaSalidas()
        {
            InitializeComponent();

            BindingContext = vmb = new ViewModelBitacoraSalida();
            BindingContext = pvm = new ProductoViewModel();

            DateTime fechain = DateTime.Now;
            DtpInicio.Date = fechain.Date.AddMonths(-1);
            CargarProductos();
            CargarListaBitacora();
            SumaTotal();
        }

        private async void CargarProductos()
        {
            PckProducto.ItemsSource = await pvm.GetProducto();
        }

        private async void CargarListaBitacora()
        {
            ObservableCollection<BitacoraSalida> list = new ObservableCollection<BitacoraSalida>();
            Producto producto = PckProducto.SelectedItem as Producto;
            if (producto != null)
            {
                list = await vmb.GetBitacoraSalidas(DtpInicio.Date, DtpFinal.Date, producto.Nombre, CkbTodo.IsToggled);
            }
            else
            {
                list = await vmb.GetBitacoraSalidas(DtpInicio.Date, DtpFinal.Date, null, CkbTodo.IsToggled);
            }
            LvlListaBitacora.ItemsSource = list;
        }

        private void LvlListaBitacora_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void LvlListaBitacora_Refreshing(object sender, EventArgs e)
        {
            CargarListaBitacora();
            LvlListaBitacora.IsRefreshing = false;
            SumaTotal();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            LvlListaBitacora.BeginRefresh();
            Task.Delay(5000);
            LvlListaBitacora.EndRefresh();
        }

        private async void SumaTotal()
        {
            ObservableCollection<BitacoraSalida> list = new ObservableCollection<BitacoraSalida>();
            Producto producto = PckProducto.SelectedItem as Producto;
            if (producto != null)
            {
                list = await vmb.GetBitacoraSalidas(DtpInicio.Date, DtpFinal.Date, producto.Nombre, CkbTodo.IsToggled);
            }
            else
            {
                list = await vmb.GetBitacoraSalidas(DtpInicio.Date, DtpFinal.Date, "", CkbTodo.IsToggled);
            }

            int suma = 0;
            foreach(var item in list)
            {
                suma += item.Salida;
            }

            LblTotal.Text = suma.ToString();
        }

        private void CkbTodo_Toggled(object sender, ToggledEventArgs e)
        {
            CargarProductos();
            CargarListaBitacora();
            SumaTotal();
        }
    }
}