using ShopColibriApp.Models;
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
    public partial class SeleccionUsuarioPedido : ContentPage
    {
        UsuarioViewModel uvm { get; set; }
        private string? Filtro;
        public SeleccionUsuarioPedido()
        {
            InitializeComponent();
            
            BindingContext = uvm = new UsuarioViewModel();

            CargarListaUsuarios();
        }

        private async void CargarListaUsuarios()
        {
            ObservableCollection<Usuario> lista = new ObservableCollection<Usuario>();
            lista = await uvm.GetUsuBuscar(Filtro, true, true);
            LvlListaUsuarios.ItemsSource = lista;
        }

        private void SbBuscarUsu_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtro = SbBuscarUsu.Text.Trim();
            CargarListaUsuarios();
        }

        private async void LvlListaUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Usuario usuario = e.SelectedItem as Usuario;
            if (usuario != null)
            {
                GlobalObject.GloUsuPedi = usuario;
                await Navigation.PushAsync(new PedidosPage());
            }
        }

        private void LvlListaUsuarios_Refreshing(object sender, EventArgs e)
        {
            CargarListaUsuarios();
            GlobalObject.GloUsuPedi = new Models.Usuario();
            LvlListaUsuarios.IsRefreshing = false;
        }
    }
}