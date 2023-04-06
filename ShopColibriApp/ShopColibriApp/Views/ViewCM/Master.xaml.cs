using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShopColibriApp.Views;
using Newtonsoft.Json.Schema;

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
                GlobalObject.GloUsu = new Models.Usuario();

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
            if (GlobalObject.GloUsu.TusuarioId == 1 ||
                    GlobalObject.GloUsu.TusuarioId == 2)
            {
                if (GlobalObject.GloUsu.TusuarioId == 1)
                {
                    BtnRegisUsu.IsVisible = true;
                }
                else
                {
                    BtnRegisUsu.IsVisible = false;
                }
                FmUsuariosAd.IsVisible = true;
            }
            else
            {
                FmUsuariosAd.IsVisible = false;
            }
        }

        private async void BtnInventario_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented = false;
            await App.MasterDet.Detail.Navigation.PushAsync(new InventarioPage());
        }

        private async void BtnInicio_Clicked(object sender, EventArgs e)
        {
            App.MasterDet.IsPresented=false;
            await Navigation.PushAsync(new MainPage());
        }
    }
}