using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VistaBitacora : ContentPage
	{
        ViewModelBitacora vmb { get; set; }
		public VistaBitacora ()
		{
			InitializeComponent ();

            BindingContext = vmb = new ViewModelBitacora ();

            DateTime fechain = DateTime.Now;
            DtpInicio.Date = fechain.Date.AddMonths(-1);
            CargarAccion();
            CargarBitacora();
		}

        public async void CargarBitacora()
        {
            string busqueda = PckProducto.SelectedItem as string;
            LvlListaBitacora.ItemsSource = await vmb.GetBitacora(DtpInicio.Date, DtpFinal.Date, busqueda, CkbTodo.IsToggled);
        }

		public void CargarAccion()
		{
			ObservableCollection<string> item = new ObservableCollection<string> ();

            item.Add("Guardo un Usuario");
            item.Add("Modifico un Usuario");
            item.Add("Elimino un Usuario");
            item.Add("Inhabilito un Usuario");
            item.Add("Valido un Usuario");
            item.Add("Guardo un Producto");
            item.Add("Modifico un Producto");
            item.Add("Elimino un Producto");
			item.Add("Guardo un Inventario");
			item.Add("Modifico un Inventario");
			item.Add("Elimino un Inventario");
            item.Add("Guardo un Empaque");
            item.Add("Modifico un Empaque");
            item.Add("Elimino un Empaque");
            item.Add("Guardo un Pedido");
            item.Add("Modifico un Pedido");
            item.Add("Elimino un Pedido");
            item.Add("Guardo un Control de Marmita");
            item.Add("Modifico un Control de Marmita");
            item.Add("Elimino un Control de Marmita");
            item.Add("Guardo un Registro");
            item.Add("Modifico un Registro");
            item.Add("Elimino un Registro");

            PckProducto.ItemsSource = item;
		}

        private void CkbTodo_Toggled(object sender, ToggledEventArgs e)
        {
            CargarAccion();
            CargarBitacora();
        }

        private void LvlListaBitacora_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void LvlListaBitacora_Refreshing(object sender, EventArgs e)
        {
            CargarBitacora();
            LvlListaBitacora.IsRefreshing = false;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            LvlListaBitacora.BeginRefresh();
            Task.Delay(5000);
            LvlListaBitacora.EndRefresh();
        }
    }
}