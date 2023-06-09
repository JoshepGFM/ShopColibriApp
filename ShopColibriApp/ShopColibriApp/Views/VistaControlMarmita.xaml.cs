﻿using ShopColibriApp.Models;
using ShopColibriApp.Servicios;
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
	public partial class VistaControlMarmita : ContentPage
	{
        ControlMarmitaViewModel cvm { get; set; }
        ViewModelBitacora vmb { get; set; }
        private string? Filter { get; set; }
		public VistaControlMarmita ()
		{
			InitializeComponent ();

            vmb = new ViewModelBitacora ();
            BindingContext = cvm = new ControlMarmitaViewModel();

            CargarListaControlMarmita();
		}

        private async void CargarListaControlMarmita()
        {
            ObservableCollection<ControlMarmitaDTO> lista = new ObservableCollection<ControlMarmitaDTO>();
            lista = await cvm.GetControlMarmiBuscar(Filter);
            LvlListaControlesMarmita.ItemsSource = lista;
        }

        private void LvlListaControlesMarmita_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ControlMarmitaDTO controlMarmita = e.SelectedItem as ControlMarmitaDTO;
            GlobalObject.GloControlMarDTO = controlMarmita;
            FmModificar.IsVisible = true;
        }

        private void LvlListaControlesMarmita_Refreshing(object sender, EventArgs e)
        {
            CargarListaControlMarmita();
            FmModificar.IsVisible = false;
            LvlListaControlesMarmita.IsRefreshing = false;
        }

        private  async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Confirmación", "¿Esta seguro de eliminar este Empaque?", "Si", "No"))
            {
                var item = (sender as MenuItem).CommandParameter;
                int id = int.Parse(item.ToString());
                ControlMarmita control = new ControlMarmita();
                control = await cvm.GetControlMarmiId(id);
                bool R = await cvm.DeleteControlMarmita(id);
                if (R)
                {
                    await DisplayAlert("Verificación", "Se elimino el control con éxito", "OK");
                    await vmb.PostBitacora(DateTime.Now, GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2 +
                            " Elimino un Control de Marmita. Control: Cod." + control.Codigo + " del lote, " + control.Lote);
                    CargarListaControlMarmita();
                }
                else
                {
                    await DisplayAlert("Error de verificación", "No se logro eliminar el Control", "OK");
                }
            }
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ControlMarmitaPage());
        }
        //Permite recargar la lista al escribir
        private void SbBuscarControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter = SbBuscarControl.Text.Trim();
            CargarListaControlMarmita();
        }

        private async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloControMarmi_Cont = new ControlMarmita();
            GlobalObject.GloControlMarDTO = new Models.ControlMarmitaDTO();
            GlobalObject.GloListUsu.Clear();
            await Navigation.PushAsync(new ControlMarmitaPage());
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}