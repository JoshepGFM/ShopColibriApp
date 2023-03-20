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
        private void LvlListaUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

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
            GlobalObject.GloUsu_Registro = new Usuario();
            await Navigation.PushAsync(new Registro());
        }
    }
}