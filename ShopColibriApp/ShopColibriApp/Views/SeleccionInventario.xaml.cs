using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeleccionInventario : ContentPage
    {
        InventarioViewModel ivm { get; set; }
        private string? Filtro;
        InventarioDTO inventarioDTO { get; set; }
        public SeleccionInventario()
        {
            InitializeComponent();

            BindingContext = ivm = new InventarioViewModel();

            CargarListaInventario();
        }

        private async void CargarListaInventario()
        {
            LvlListaInventario.ItemsSource = await ivm.GetInveBuscar(Filtro, true);
        }

        private void LvlListaInventario_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            InventarioDTO inventario = e.SelectedItem as InventarioDTO;
            if (inventario != null)
            {
                inventarioDTO = inventario;
                TxtCantidad.IsEnabled = true;
            }
        }

        private void LvlListaInventario_Refreshing(object sender, EventArgs e)
        {
            CargarListaInventario();
            TxtCantidad.IsEnabled = false;
            LvlListaInventario.IsRefreshing = false;
        }

        private void SbBuscarPro_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtro = SbBuscarPro.Text.Trim();
            CargarListaInventario();
        }

        private async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            if(inventarioDTO != null)
            {
                PedidosCalcu pedidosCalcu = new PedidosCalcu();
                pedidosCalcu.Id = inventarioDTO.Id;
                pedidosCalcu.Fecha = inventarioDTO.Fecha;
                pedidosCalcu.Stock = inventarioDTO.Stock;
                pedidosCalcu.PrecioUn = inventarioDTO.PrecioUn;
                pedidosCalcu.Origen = inventarioDTO.Origen;
                pedidosCalcu.ProductoCodigo = inventarioDTO.ProductoCodigo;
                pedidosCalcu.EmpaqueId = inventarioDTO.EmpaqueId;
                pedidosCalcu.NombreEmp = inventarioDTO.NombreEmp;
                pedidosCalcu.NombrePro = inventarioDTO.NombrePro;
                pedidosCalcu.Cantidad = 0;
                pedidosCalcu.Precio = 0;
                pedidosCalcu.Total = 0;
                int entrada = 0;
                if (TxtCantidad.Text != null)
                {
                    entrada = int.Parse(TxtCantidad.Text.Trim());
                }
                if (entrada > 0 && 
                    entrada <= pedidosCalcu.Stock)
                {
                    pedidosCalcu.Cantidad = entrada;
                    pedidosCalcu.Precio = inventarioDTO.PrecioUn;
                    pedidosCalcu.Total = pedidosCalcu.Cantidad * pedidosCalcu.PrecioUn;
                }
                else
                {
                    await DisplayAlert("Error de selección", "La cantidad debe ser superior a 0 e inferior al stock", "OK");
                    return;
                }
                GlobalObject.GloListInven.Add(pedidosCalcu);
                await Navigation.PushAsync(new PedidosPage());
            }
            else
            {
                await DisplayAlert("Error de selección", "Seleccione un elemento para agregar", "OK");
            }
        }
    }
}