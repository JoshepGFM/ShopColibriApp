using ShopColibriApp.ViewModels;
using ShopColibriApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaInicio : ContentPage
    {
        UsuarioViewModel vm {  get; set; }
        bool acceso = true;
        public VistaInicio()
        {
            InitializeComponent();

            vm = new UsuarioViewModel();
            Task.Delay(200);
            CargarInicio();
        }

        private async void Usuario()
        {
            bool R = false;
            if (Application.Current.Properties.ContainsKey("Usuario") && Application.Current.Properties.ContainsKey("Pass"))
            {
                try
                {
                    string u = Application.Current.Properties["Usuario"].ToString();
                    string p = Application.Current.Properties["Pass"].ToString();
                    
                    GlobalObject.GloUsu = await vm.GetUsuario(u);
                    R = await vm.ValidarAccesoUsuario(u, p);
                    if (R)
                    {
                        acceso = false;
                        await Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        acceso = false;
                        await Navigation.PushAsync(new Login());
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                await Navigation.PushAsync(new Login());
            }
        }

        private void CargarInicio()
        {
            var timer = TimeSpan.FromSeconds(3);
            Device.StartTimer(timer, () =>
            {
                if (acceso)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Usuario();
                    });
                }
                return true;
            });
        }
    }
}