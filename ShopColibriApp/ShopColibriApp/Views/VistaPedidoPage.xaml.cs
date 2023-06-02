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
    public partial class VistaPedidoPage : ContentPage
    {
        PedidosViewModel pvm;
        private string? Filtro;
        public VistaPedidoPage()
        {
            InitializeComponent();

            BindingContext = pvm = new PedidosViewModel();

            CargarListaPedidos();
        }

        private async void CargarListaPedidos()
        {
            ObservableCollection<PedidosDTO> list = new ObservableCollection<PedidosDTO>();
            list = await pvm.GetPedidosBusqueda(Filtro);
            for(int i = 0; i < list.Count; i++)
            {
                list[i].Usuario.Nombre = list[i].Usuario.Nombre + " " +
                    list[i].Usuario.Apellido1 + " " + list[i].Usuario.Apellido2;
            }
            LvlListaPedido.ItemsSource = list;
        }

        private void LvlListaPedido_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PedidosDTO list = e.SelectedItem as PedidosDTO;
            if (list != null)
            {
                for(int i = 0; i < list.inventarios.Count; ++i)
                {
                    list.inventarios[i].priImagen = "https://drive.google.com/uc?id=" + list.inventarios[i].priImagen;
                }
                GlobalObject.GloPedidosDTO = list;
                FmModificar.IsVisible = true;
            }
        }

        private void LvlListaPedido_Refreshing(object sender, EventArgs e)
        {
            CargarListaPedidos();
            FmModificar.IsVisible = false;
            LvlListaPedido.IsRefreshing = false;
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Confirmación", "¿Esta seguro de eliminar este Pedido?", "Si", "No"))
            {
                var item = (sender as MenuItem).CommandParameter;
                int id = int.Parse(item.ToString());
                bool R = await pvm.DeletePedido(id);
                if (R)
                {
                    CargarListaPedidos();
                    await DisplayAlert("Verificación", "Se elimino el pedido con éxito", "OK");
                }
                else
                {
                    await DisplayAlert("Error de verificación", "No se logro eliminar el Pedido", "OK");
                }
            }
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PedidosPage());
        }

        private void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloListInven.Clear();
            GlobalObject.GloPedidos = new Models.Pedidos();
            GlobalObject.GloPedidos_Cont = new Models.Pedidos();
            GlobalObject.GloPedidosDTO = new Models.PedidosDTO();
            GlobalObject.GloUsuPedi = new Usuario();
            Navigation.PushAsync(new PedidosPage());
        }

        private void SbBuscarPed_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtro = SbBuscarPed.Text.Trim();
            CargarListaPedidos();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}