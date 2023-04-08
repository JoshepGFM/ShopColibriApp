using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views.ViewCM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detail : ContentPage
    {
        InventarioViewModel ivm { get; set; }

        private string? Filtro {get; set;}
        public Detail()
        {
            InitializeComponent();

            BindingContext = ivm = new InventarioViewModel();

            CargarInventario();
            FuncionesUsuario();
        }

        private async void CargarInventario()
        {
            LvlListaInventario.ItemsSource = await ivm.GetInveBuscar(Filtro, SwStock.IsToggled); 
        }

        private void SbBuscarPro_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtro = SbBuscarPro.Text.Trim();
            CargarInventario();
        }

        private void LvlListaInventario_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void SwStock_Toggled(object sender, ToggledEventArgs e)
        {
            if (SwStock.IsToggled)
            {
                LblActivo.Text = "En stock";
            }
            else
            {
                LblActivo.Text = "No stock";
            }
            LvlListaInventario.BeginRefresh();
            Task.Delay(2000);
            LvlListaInventario.EndRefresh();
        }

        private void LvlListaInventario_Refreshing(object sender, EventArgs e)
        {
            CargarInventario();
            LvlListaInventario.IsRefreshing = false;
        }

        private void BtnDelete_Clicked(object sender, EventArgs e)
        {

        }

        private async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GLoInventario = null;
            await Navigation.PushAsync(new InventarioPage());
        }

        private void FuncionesUsuario()
        {
            if(GlobalObject.GloUsu.TusuarioId == 1 || GlobalObject.GloUsu.TusuarioId == 2)
            {
                BtnAgregar.IsVisible = true;
                LblActivo.IsVisible = true;
                SwStock.IsVisible = true;
            }
            else
            {
                BtnAgregar.IsVisible = false;
                LblActivo.IsVisible = false;
                SwStock.IsVisible = false;
            }
        }
    }
}