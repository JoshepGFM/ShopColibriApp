using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShopColibriApp.Views;
using Newtonsoft.Json.Schema;
using ShopColibriApp.Models;
using System.Collections.ObjectModel;

namespace ShopColibriApp.Views.ViewCM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Master : ContentPage
    {
        public Master()
        {
            InitializeComponent();
            DataUsuario();
        }

        private async void BtnProductos_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            App.MasterDet.Detail.Navigation.PushAsync(new VistaProductosPage());
        }

        private async void BtnCerrarS_Clicked(object sender, EventArgs e)
        {
            var resp = await DisplayAlert("Cierre de Sesión", "Quiere cerrar Sesión", "Si","No");

            if(resp)
            {
                Application.Current.Properties.Clear();
                GlobalObject.GloUsu = new Usuario();

                await Navigation.PushAsync(new Login());
            }
        }

        private void DataUsuario()
        {
            if(GlobalObject.GloUsu == null)
            {
                return;
            }
            else
            {
                if (GlobalObject.GloUsu.TusuarioId == 1) { 
                    LblNombre.Text = GlobalObject.GloUsu.Nombre.ToString();
                }
                else
                {
                    LblNombre.Text = GlobalObject.GloUsu.Apellido1.ToString() + " " + 
                        GlobalObject.GloUsu.Apellido2.ToString() + " " + 
                        GlobalObject.GloUsu.Nombre.ToString();
                }
                
                LblCorreo.Text = GlobalObject.GloUsu.Email.ToString();
                LblTipo.Text = GlobalObject.GloUsu.Tipo.ToString();
                validarVisiBotones();
            }
        }

        private async void BtnRegisUsu_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new VistaUsuarios());
        }

        private async void BtnConf_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new ConfiPerfil());
        }

        private void validarVisiBotones()
        {
            if (GlobalObject.GloUsu != null)
            {
                if (GlobalObject.GloUsu.TusuarioId == 1 ||
                        GlobalObject.GloUsu.TusuarioId == 2)
                {
                    if (GlobalObject.GloUsu.TusuarioId == 2)
                    {
                        BtnProductos.IsVisible = false;
                        BtnRegisUsu.IsVisible = false;
                        BtnRegistro.IsVisible = false;
                        BtnEntradas.IsVisible = false;
                        BtnVerSalidas.IsVisible = false;
                        BtnVerBitacora.IsVisible = false;
                    }
                }
                else
                {
                    BtnProductos.IsVisible = false;
                    BtnRegisUsu.IsVisible = false;
                    BtnInventario.IsVisible = false;
                    BtnEmpaque.IsVisible = false;
                    BtnPedidos.IsVisible = false;
                    BtnControlMar.IsVisible = false;
                    BtnRegistro.IsVisible = false;
                    BtnEntradas.IsVisible = false;
                    BtnVerSalidas.IsVisible = false;
                    BtnVerBitacora.IsVisible = false;
                    ScrollMenu.HeightRequest = 80;
                }
            }
            else
            {
                BtnCerrarS.IsVisible = false;
                BtnConf.IsVisible = false;
            }
        }

        private async void BtnInventario_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloImagenes.Clear();
            GlobalObject.GLoInventario = new Inventario();
            GlobalObject.GloInven_DTO = new InventarioDTO();
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new InventarioPage());
        }

        private async void BtnInicio_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented=false;
            await Navigation.PushAsync(new MainPage());
        }

        private async void BtnEmpaque_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new VistaEmpaque());
        }

        private async void BtnPedidos_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            GlobalObject.GloListInven.Clear();
            GlobalObject.GloPedidos = new Models.Pedidos();
            GlobalObject.GloPedidos_Cont = new Models.Pedidos();
            GlobalObject.GloPedidosDTO = new Models.PedidosDTO();
            GlobalObject.GloUsuPedi = new Usuario();
            await App.MasterDet.Detail.Navigation.PushAsync(new VistaPedidoPage());
        }

        private void BtnPedido_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloPedidosDTO = new Models.PedidosDTO();
            GlobalObject.GloPedidos = new Models.Pedidos();
            GlobalObject.GloPedidos_Cont = new Models.Pedidos();
            Navigation.PushAsync(new PedidosPage());
        }

        private async void BtnRegistro_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            GlobalObject.GloRegistro = new Models.Registro();
            await App.MasterDet.Detail.Navigation.PushAsync(new RegistroPage());
        }

        private async void BtnVerRegistros_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new VistaRegistro());
        }

        private void BtnNuevosPro_Clicked(object sender, EventArgs e)
        {

        }

        private async void BtnVerControlMar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloControlMarDTO = new Models.ControlMarmitaDTO();
            await Navigation.PushAsync(new VistaControlMarmita());
        }

        private async void BtnVerPedidos_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloPedidosDTO = new Models.PedidosDTO();
            GlobalObject.GloPedidos = new Models.Pedidos();
            GlobalObject.GloPedidos_Cont = new Models.Pedidos();
            await Navigation.PushAsync(new VistaPedidoPage());
        }

        private async void BtnVerSalidas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VistaSalidas());
        }

        private async void BtnVerBitacora_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VistaBitacora());
        }

        private async void BtnEntradas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntradasPage());
        }
    }
}