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
    public partial class VistaSalidas : ContentPage
    {
        public VistaSalidas()
        {
            InitializeComponent();
        }

        private void LvlListaBitacora_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void LvlListaBitacora_Refreshing(object sender, EventArgs e)
        {

        }
    }

    //protected override bool OnBackButtonPressed()
    //{
    //    Navigation.PushAsync(new MainPage());

    //    // Retornar true para indicar que se ha manejado el evento del botón "Back"
    //    return true;
    //}
}