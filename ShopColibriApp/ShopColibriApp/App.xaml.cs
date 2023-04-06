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
    }
}
