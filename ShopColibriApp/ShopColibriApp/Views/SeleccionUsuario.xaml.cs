using Android.Widget;
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
    public partial class SeleccionUsuario : ContentPage
    {
        UsuarioViewModel uvm { get; set; }
        private string? Filtro { get; set; }
        public SeleccionUsuario()
        {
            InitializeComponent();

            BindingContext = uvm = new UsuarioViewModel();

            CargarListaUsuarios();
        }

        private async void CargarListaUsuarios()
        {
            ObservableCollection<Usuario> lista = new ObservableCollection<Usuario>();
            lista = await uvm.GetUsuBuscar(Filtro, true, false);
            if (GlobalObject.GloListUsu.Count > 0)
            {
                for(int i = 0; i < GlobalObject.GloListUsu.Count; i++)
                {
                    for(int j = 0; j < lista.Count; j++)
                    {
                        if (GlobalObject.GloListUsu[i].IdUsuario == lista[j].IdUsuario)
                        {
                            lista.Remove(lista[j]);
                        }
                    }
                }
            }
            LvlListaUsuarios.ItemsSource = lista;
        }

        private async void LvlListaUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Usuario usuario = e.SelectedItem as Usuario;
            if (usuario != null)
            {
                GlobalObject.GloListUsu.Add(usuario);
                await Navigation.PushAsync(new ControlMarmitaPage());
            }
        }

        private void LvlListaUsuarios_Refreshing(object sender, EventArgs e)
        {
            CargarListaUsuarios();
            LvlListaUsuarios.IsRefreshing = false;
        }

        private void SbBuscarUsu_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtro = SbBuscarUsu.Text.Trim();
            CargarListaUsuarios();
        }
    }
}