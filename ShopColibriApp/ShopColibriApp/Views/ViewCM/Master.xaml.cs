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
                GlobalObject.GloUsu = null;

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
                    if (GlobalObject.GloUsu.TusuarioId == 1)
                    {
                        BtnProductos.IsVisible = true;
                        BtnRegisUsu.IsVisible = true;
                        BtnInventario.IsVisible = true;
                        BtnEmpaque.IsVisible = true;
                        BtnPedidos.IsVisible = true;
                        BtnControlMar.IsVisible = true;
                        BtnRegistro.IsVisible = true;
                        BtnVerRegistros.IsVisible = true;
                    }
                    else
                    {
                        BtnProductos.IsVisible = false;
                        BtnRegisUsu.IsVisible = false;
                        BtnInventario.IsVisible = true;
                        BtnEmpaque.IsVisible = true;
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
                    BtnVerRegistros.IsVisible = false;
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
            GlobalObject.GloInven_DTO = new Models.InventarioDTO();
            GlobalObject.GloImagenes = new List<Imagen>();
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
            await App.MasterDet.Detail.Navigation.PushAsync(new PedidosPage());
        }

        private async void BtnControlMar_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            GlobalObject.GloListUsu.Clear();
            GlobalObject.GloControMarmi = new Models.ControlMarmita();
            GlobalObject.GloControMarmi_Cont = new Models.ControlMarmita();
            GlobalObject.GloControlMarDTO = new Models.ControlMarmitaDTO();
            await App.MasterDet.Detail.Navigation.PushAsync(new ControlMarmitaPage());
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
            GlobalObject.GloRegistro = null;
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

        private void BtnVerBitacora_Clicked(object sender, EventArgs e)
        {

        }

        private void BtnInicio_Pressed(object sender, EventArgs e)
        {
            BtnInicio.BackgroundColor = Color.Gray;
        }

        private void BtnRegisUsu_Pressed(object sender, EventArgs e)
        {
            BtnRegisUsu.BackgroundColor = Color.Gray;
        }

        private void BtnProductos_Pressed(object sender, EventArgs e)
        {
            BtnProductos.BackgroundColor = Color.Gray;
        }

        private void BtnNuevosPro_Pressed(object sender, EventArgs e)
        {
            BtnNuevosPro.BackgroundColor = Color.Gray;
        }

        private void BtnInventario_Pressed(object sender, EventArgs e)
        {
            BtnInventario.BackgroundColor = Color.Gray;
        }

        private void BtnEmpaque_Pressed(object sender, EventArgs e)
        {
            BtnEmpaque.BackgroundColor = Color.Gray;
        }

        private void BtnPedidos_Pressed(object sender, EventArgs e)
        {
            BtnPedidos.BackgroundColor = Color.Gray;
        }

        private void BtnVerPedidos_Pressed(object sender, EventArgs e)
        {
            BtnVerPedidos.BackgroundColor = Color.Gray;
        }

        private void BtnControlMar_Pressed(object sender, EventArgs e)
        {
            BtnControlMar.BackgroundColor = Color.Gray;
        }

        private void BtnVerControlMar_Pressed(object sender, EventArgs e)
        {
            BtnVerControlMar.BackgroundColor = Color.Gray;
        }

        private void BtnRegistro_Pressed(object sender, EventArgs e)
        {
            BtnRegistro.BackgroundColor = Color.Gray;
        }

        private void BtnVerRegistros_Pressed(object sender, EventArgs e)
        {
            BtnVerRegistros.BackgroundColor = Color.Gray;
        }

        private void BtnVerSalidas_Pressed(object sender, EventArgs e)
        {
            BtnVerSalidas.BackgroundColor = Color.Gray;
        }

        private void BtnVerBitacora_Pressed(object sender, EventArgs e)
        {
            BtnVerBitacora.BackgroundColor = Color.Gray;
        }
    }
}