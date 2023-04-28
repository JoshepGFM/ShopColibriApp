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
    public partial class PedidosPage : ContentPage
    {
        public Usuario MiUsuario { get; set; }
        private PedidosCalcu pedidosCalcu { get; set; }
        PedidosViewModel pvm { get; set; }
        public PedidosPage()
        {
            InitializeComponent();

            BindingContext = pvm = new PedidosViewModel();

            MiUsuario = new Usuario();

            CargarInfo();
            CargarListaElementos();
            CalculoTotal();
        }

        private void CargarInfo()
        {
            if(GlobalObject.GloPedidosDTO.Codigo > 0)
            {
                PckFecha.Date = GlobalObject.GloPedidosDTO.Fecha;
                PckFechaEn.Date = GlobalObject.GloPedidosDTO.FechaEn;
                ObservableCollection<PedidosCalcu> inventario = new ObservableCollection<PedidosCalcu>();
                inventario = GlobalObject.GloPedidosDTO.inventarios;
                if (inventario.Count > 0)
                {
                    if (GlobalObject.GloListInven.Count == 0)
                    {
                        GlobalObject.GloListInven = inventario;
                    }
                }
                if (GlobalObject.GloUsuPedi.IdUsuario > 0)
                {
                    MiUsuario = GlobalObject.GloUsuPedi;
                    BtnQuitarUsu.IsVisible = true;
                    BtnAgregarUsu.IsEnabled = false;
                }
                else
                {
                    MiUsuario = GlobalObject.GloPedidosDTO.Usuario;
                }
                BtnGuardar.IsVisible = false;
                BtnModificar.IsVisible = true;
            }
            else
            {
                if (GlobalObject.GloUsuPedi.IdUsuario > 0)
                {
                    MiUsuario = GlobalObject.GloUsuPedi;
                    BtnQuitarUsu.IsVisible = true;
                    BtnAgregarUsu.IsEnabled = false;
                }
                BtnGuardar.IsVisible = true;
                BtnModificar.IsVisible = false;
            }
        }

        private void CalculoTotal()
        {
            if (GlobalObject.GloListInven.Count > 0)
            {
                decimal Suma = 0;
                for(int i = 0;i < GlobalObject.GloListInven.Count;i++)
                {
                    Suma += GlobalObject.GloListInven[i].Total;
                }
                LblTotal.Text = Suma.ToString();
            }
        }

        private void CargarListaElementos()
        {
            LvlListaInventario.ItemsSource = GlobalObject.GloListInven;
            if (MiUsuario != null && MiUsuario.IdUsuario > 0)
            {
                LblUsuario.Text = MiUsuario.Nombre + " " +
                MiUsuario.Apellido1 + " " +
                MiUsuario.Apellido2;
                BtnQuitarUsu.IsVisible = true;
                BtnAgregarUsu.IsEnabled = false;
                BtnAgregarUsu.BackgroundColor = Color.Gray;
            }
        }

        private void LvlListaInventario_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PedidosCalcu select = e.SelectedItem as PedidosCalcu;
            if (select != null)
            {
                pedidosCalcu = select;
                FmQuitar.IsVisible = true;
            }
        }

        private void LvlListaInventario_Refreshing(object sender, EventArgs e)
        {
            CargarListaElementos();
            FmQuitar.IsVisible = false;
            LvlListaInventario.IsRefreshing = false;
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            if (ValidarEntradas())
            {
                decimal Total = decimal.Parse(LblTotal.Text);
                bool R = await pvm.PostPedidos(PckFecha.Date, PckFechaEn.Date, GlobalObject.GloListInven,
                    MiUsuario, Total);

                if (R)
                {
                    GlobalObject.GloListInven.Clear();
                    GlobalObject.GloPedidos = new Models.Pedidos();
                    GlobalObject.GloPedidos_Cont = new Models.Pedidos();
                    GlobalObject.GloPedidosDTO = new Models.PedidosDTO();
                    GlobalObject.GloUsuPedi = new Usuario();
                    MiUsuario = new Usuario();
                    await DisplayAlert("Validación exitosa", "Se a ingresado el pedido con éxito", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error de validación", "No se a podido realizar el guardado del pedido","OK");
                }
            }
        }

        private async void BtnagregarIventa_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloPedidosDTO.Fecha = PckFecha.Date;
            GlobalObject.GloPedidosDTO.FechaEn = PckFechaEn.Date;
            GlobalObject.GloPedidosDTO.inventarios = GlobalObject.GloListInven;
            GlobalObject.GloPedidosDTO.Usuario = MiUsuario;
            await Navigation.PushAsync(new SeleccionInventario());
        }

        private async void BtnAgregarUsu_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloPedidosDTO.Fecha = PckFecha.Date;
            GlobalObject.GloPedidosDTO.FechaEn = PckFechaEn.Date;
            GlobalObject.GloPedidosDTO.inventarios = GlobalObject.GloListInven;
            GlobalObject.GloPedidosDTO.Usuario = MiUsuario;
            await Navigation.PushAsync(new SeleccionUsuarioPedido());
        }

        private void BtnQuitar_Clicked(object sender, EventArgs e)
        {
            if (pedidosCalcu.Id > 0)
            {
                GlobalObject.GloListInven.Remove(pedidosCalcu);
                CargarListaElementos();
                pedidosCalcu = new Models.PedidosCalcu();
                FmQuitar.IsVisible = false;
                CalculoTotal();
            }
        }

        private void BtnQuitarUsu_Clicked(object sender, EventArgs e)
        {
            BtnAgregarUsu.BackgroundColor = Color.Green;
            BtnAgregarUsu.IsEnabled = true;
            LblUsuario.Text = "Seleccione un Cliente";
            MiUsuario = new Usuario();
            BtnQuitarUsu.IsVisible = false;
        }

        private bool ValidarEntradas()
        {
            bool R = false;
            if (PckFecha.Date != null && PckFechaEn != null &&
                GlobalObject.GloListInven.Count > 0 && MiUsuario != null && 
                MiUsuario.IdUsuario > 0 && decimal.Parse(LblTotal.Text.Trim()) > 0)
            {
                R = true;
            }
            else
            {
                if(PckFecha.Date == null)
                {
                    PckFecha.Focus();
                    DisplayAlert("Error de validación", "Se requiere una fecha de cuando se creo el pedido", "OK");
                    return false;
                }
                if (PckFechaEn.Date == null)
                {
                    PckFechaEn.Focus();
                    DisplayAlert("Error de validación", "Se requiere una fecha para la entrega", "OK");
                    return false;
                }
                if (GlobalObject.GloListInven.Count == 0)
                {
                    DisplayAlert("Error de validación", "Se requiere el ingreso de un producto para el pedido", "OK");
                    BtnagregarIventa.Focus();
                    return false;
                }
                if (MiUsuario == null || MiUsuario.IdUsuario == 0)
                {
                    DisplayAlert("Error de validación", "Se requiere de un usuario para el pedido", "OK");
                    BtnAgregarUsu.Focus();
                    return false;
                }
            }
            return R;
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            if (ValidarEntradas())
            {
                decimal Total = decimal.Parse(LblTotal.Text);
                bool R = await pvm.PutPedidos(GlobalObject.GloPedidosDTO.Codigo, PckFecha.Date, PckFechaEn.Date, GlobalObject.GloListInven,
                    MiUsuario, Total);

                if (R)
                {
                    GlobalObject.GloListInven.Clear();
                    GlobalObject.GloPedidos = new Models.Pedidos();
                    GlobalObject.GloPedidos_Cont = new Models.Pedidos();
                    GlobalObject.GloPedidosDTO = new Models.PedidosDTO();
                    GlobalObject.GloUsuPedi = new Usuario();
                    MiUsuario = new Usuario();
                    await DisplayAlert("Validación exitosa", "Se a modificado el pedido con éxito", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error de validación", "No se a podido realizar el modificado del pedido", "OK");
                }
            }
        }
    }
}