using ShopColibriApp.Models;
using ShopColibriApp.Servicios;
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
	public partial class VistaControlMarmita : ContentPage
	{
        ControlMarmitaViewModel cvm { get; set; }
        private string? Filter { get; set; }
		public VistaControlMarmita ()
		{
			InitializeComponent ();

            BindingContext = cvm = new ControlMarmitaViewModel();

            CargarListaControlMarmita();
		}

        private async void CargarListaControlMarmita()
        {
            ObservableCollection<ControlMarmita> lista = new ObservableCollection<ControlMarmita>();
            lista = await cvm.GetControlMarmi(Filter);
            LvlListaControlesMarmita.ItemsSource = lista;
        }

        private void LvlListaControlesMarmita_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void LvlListaControlesMarmita_Refreshing(object sender, EventArgs e)
        {
            CargarListaControlMarmita();
            LvlListaControlesMarmita.IsRefreshing = false;
        }

        private void BtnDelete_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnInvalidar_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnModificar_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnAgregar_Clicked(object sender, EventArgs e)
        {

        }

        private void SbBuscarControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter = SbBuscarControl.Text.Trim();
            CargarListaControlMarmita();
        }
    }
}