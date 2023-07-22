using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ShopColibriApp.Views;
using Prism.Navigation.Xaml;
using ShopColibriApp.ViewModels;
using System.Threading.Tasks;

namespace ShopColibriApp
{
    public partial class App : Application
    {
        public static MasterDetailPage MasterDet { get; set; }
        bool Validado { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new VistaInicio());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            // Procesar la URL de redirección y extraer el código de autorización
            // para completar el flujo de autorización de Google Drive.

            if (uri.Host == "authresponse")
            {
                string authorizationCode = uri.Query.Substring(6);
                // Procesar el código de autorización aquí.
                // Puedes usar "authorizationCode" para completar el flujo de autorización con Google Drive.
            }
        }
    }
}
