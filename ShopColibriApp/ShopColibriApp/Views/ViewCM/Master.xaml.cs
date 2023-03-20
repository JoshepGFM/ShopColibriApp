using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShopColibriApp.Views;

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
            await App.MasterDet.Detail.Navigation.PushAsync(new ImagenPage());
        }

        private async void BtnCerrarS_Clicked(object sender, EventArgs e)
        {
            var resp = await DisplayAlert("Cierre de Sesion", "Quiere cerrar Sesion", "Si","No");

            if(resp)
            {
                Application.Current.Properties["Usuario"] = "";
                Application.Current.Properties["Pass"] = "";
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
            }
        }

        private async void BtnRegisUsu_Clicked(object sender, EventArgs e)
        {
            await App.MasterDet.Detail.Navigation.PushAsync(new VistaUsuarios());
        }

        private void BtnConf_Clicked(object sender, EventArgs e)
        {
            //TODO:
        }
    }
}