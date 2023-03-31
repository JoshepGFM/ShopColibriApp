using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
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

            EstadoSW();
		}

		private async void CargarListaUsuarios(bool estado = true)
		{
			LvlListaUsuarios.ItemsSource = await vm.GetUsuBuscar(Filtro, estado);
		}
        private async void LvlListaUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Usuario itemSelect = e.SelectedItem as Usuario;
            if (itemSelect != null)
            {
                
                //if (SwAvtivos.IsToggled)
                //{
                //    LvlListaUsuarios.BtnInvalidar.ImageSoucer = "Validar.png";
                //}
                //else
                //{
                //    BtnInvalidar.ImageSoucer = "Invalidar.png";
                //}
                GlobalObject.GloUsu_Registro = itemSelect;
                await Navigation.PushAsync(new Registro());
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
			Filtro = SbBuscarUsu.Text;
			CargarListaUsuarios();
        }

        private void SwAvtivos_Toggled(object sender, ToggledEventArgs e)
        {
			if (SwAvtivos.IsToggled)
			{
				LblActivo.Text = "Activos";
			}
			else
            {
                LblActivo.Text = "Inactivos";
			}
        }

        private async void EstadoSW()
        {
            var timer = TimeSpan.FromSeconds(0.5);
            Device.StartTimer(timer, () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CargarListaUsuarios(SwAvtivos.IsToggled);
                });
                return true;
            });
        }

        private async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloUsu_Registro = null;
            GlobalObject.AgregadoUsuSis = true;
            await Navigation.PushAsync(new Registro());
        }

        private void BtnInvalidar_Clicked(object sender, EventArgs e)
        {
            
        }

        private void BtnModificar_Clicked(object sender, EventArgs e)
        {

        }

        private void LvlListaUsuarios_Refreshing(object sender, EventArgs e)
        {
            CargarListaUsuarios();
            LvlListaUsuarios.IsRefreshing = false;
        }
    }
}