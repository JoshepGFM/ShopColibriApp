﻿using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views.ViewCM
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VistaUsuarios : ContentPage
	{
		UsuarioViewModel vm;
		private string? Filtro { get; set; }
		public VistaUsuarios ()
		{
			InitializeComponent ();

			BindingContext = vm = new UsuarioViewModel ();

            CargarListaUsuarios();
		}

		public async void CargarListaUsuarios()
		{
            ObservableCollection<Usuario> lista = new ObservableCollection<Usuario> ();
            lista = await vm.GetUsuBuscar(Filtro, SwAvtivos.IsToggled);
            LvlListaUsuarios.ItemsSource = lista;
		}
        private async void LvlListaUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Usuario itemSelect = e.SelectedItem as Usuario;

            if (itemSelect != null)
            {

                if (SwAvtivos.IsToggled)
                {
                    BtnInvalidar.Source = "Invalidar.png";
                    FmIvalidar.IsVisible = true;
                    FmModificar.IsVisible = true;
                }
                else
                {
                    BtnInvalidar.Source = "Validar.png";
                    FmIvalidar.IsVisible = true;
                }
                GlobalObject.GloUsu_Registro = itemSelect;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
			Filtro = SbBuscarUsu.Text;
			CargarListaUsuarios();
        }

        private async void SwAvtivos_Toggled(object sender, ToggledEventArgs e)
        {
			if (SwAvtivos.IsToggled)
			{
				LblActivo.Text = "Activos";
                FmIvalidar.IsVisible = false;
                FmModificar.IsVisible = false;
                FmIvalidar.BackgroundColor = Color.PaleVioletRed;
            }
			else
            {
                LblActivo.Text = "Inactivos";
                FmIvalidar.IsVisible = false;
                FmModificar.IsVisible = false;
                FmIvalidar.BackgroundColor = Color.PaleGreen;
            }
            LvlListaUsuarios.BeginRefresh();
            Task.Delay(2000);
            LvlListaUsuarios.EndRefresh();
        }

        private async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloUsu_Registro = null;
            GlobalObject.AgregadoUsuSis = true;
            await Navigation.PushAsync(new Registro());
        }

        private async void BtnInvalidar_Clicked(object sender, EventArgs e)
        {
            if(LvlListaUsuarios.SelectedItem != null)
            {
                int id = GlobalObject.GloUsu_Registro.IdUsuario;
                if (id != 1)
                {
                    bool R = await vm.ValidarUsuario(id);
                    if (R)
                    {
                        if (FmIvalidar.BackgroundColor == Color.PaleVioletRed)
                        {
                            await DisplayAlert("Validación", "El usuario se Invalido correctamente", "OK");
                            GlobalObject.GloUsu_Registro = null;
                            LvlListaUsuarios.BeginRefresh();
                            Task.Delay(2000);
                            LvlListaUsuarios.EndRefresh();
                            return;
                        }
                        if (FmIvalidar.BackgroundColor == Color.PaleGreen)
                        {
                            await DisplayAlert("Validación", "El usuario se Valido correctamente", "OK");
                            GlobalObject.GloUsu_Registro = null;
                            LvlListaUsuarios.BeginRefresh();
                            Task.Delay(2000);
                            LvlListaUsuarios.EndRefresh();
                            return;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error de validación", "No se tuvo éxito al Validar o invalidar el usuario", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error de validación", "No se puede Invalidar este administrador", "OK");
                }
            }
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registro());
        }

        private void LvlListaUsuarios_Refreshing(object sender, EventArgs e)
        {
            CargarListaUsuarios();
            FmIvalidar.IsVisible = false;
            FmModificar.IsVisible = false;
            LvlListaUsuarios.IsRefreshing = false;
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            FmIvalidar.IsVisible = false;
            FmModificar.IsVisible = false;
            if (await DisplayAlert("Confirmación", "¿Esta seguro de eliminar este Usuario?", "Si", "No"))
            {
                var item = (sender as MenuItem).CommandParameter;
                int id = int.Parse(item.ToString());
                if (id != 1)
                {
                    bool R = await vm.DeleteUsuario(id);
                    if (R)
                    {
                        CargarListaUsuarios();
                        await DisplayAlert("Verificación", "Se elimino el usuario con éxito", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error de verificación", "No se logro elimino el usuario", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error de Eliminación", "No se puede eliminar este usuario administrador", "OK");
                }
            }
        }
    }
}