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
    public partial class Login : ContentPage
    {
        UsuarioViewModel vm { get; set; }
        public Login()
        {
            InitializeComponent();
            vm = new UsuarioViewModel();
            AccesoDirecto();
            TmrConApi();
        }

        private void CmdVerPass(object sender, ToggledEventArgs e)
        {
            if (SwVerPass.IsToggled == false) {
                TxtPass.IsPassword = true;
            }
            else
            {
                TxtPass.IsPassword = false;
            }
        }

        private async void BtnIngresar_Clicked(object sender, EventArgs e)
        {
            bool R = false;
            if (TxtUserName.Text != null && !string.IsNullOrEmpty(TxtUserName.Text.Trim()) &&
                TxtPass.Text != null && !string.IsNullOrEmpty(TxtPass.Text.Trim()))
            {
                try
                {
                    string u = TxtUserName.Text.Trim();
                    string p = TxtPass.Text.Trim();

                    R = await vm.ValidarAccesoUsuario(u, p);
                    Application.Current.Properties["Usuario"] = u;
                    Application.Current.Properties["Pass"] = p;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                await DisplayAlert("Error de Validación", "Se requiere el Usuario y la contraseña", "OK");
            }

            if (R)
            {
                string u = Application.Current.Properties["Usuario"].ToString();

                GlobalObject.GloUsu = await vm.GetUsuario(u);

                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Validación de acceso", "Usuario y contraseña incorrectos", "OK");
            }
        }

        private async void Registrarse(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registro());
        }

        private async void Recuperar(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Recuperacion());
        }

        private async void TmrConApi()
        {
            var timer = TimeSpan.FromSeconds(1);
            Device.StartTimer(timer, () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ConApi();
                });
                return true;
            });
        }

        private async void ConApi()
        {
            bool R = await vm.ValidarConexion();
            if (R)
            {
                EstadoApi.BackgroundColor = Color.Green;
            }
            else
            {
                EstadoApi.BackgroundColor = Color.Red;
            }
        }

        private async void AccesoDirecto()
        {
            bool R = false;
            if (Application.Current.Properties.ContainsKey("Usuario") && Application.Current.Properties.ContainsKey("Pass"))
            {
                try
                {
                    string u = Application.Current.Properties["Usuario"].ToString();
                    string p = Application.Current.Properties["Pass"].ToString();

                    GlobalObject.GloUsu = await vm.GetUsuario(u);
                    R = await vm.ValidarAccesoUsuario(u,p);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            if (R) { await Navigation.PushAsync(new MainPage()); }
        }
    }
}