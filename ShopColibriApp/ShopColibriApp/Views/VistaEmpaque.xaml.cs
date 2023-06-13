using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaEmpaque : ContentPage
    {
        EmpaqueViewModel evm { get; set; }
        ViewModelBitacora vmb { get; set; }
        private string? Filtro { get; set; }
        public VistaEmpaque()
        {
            InitializeComponent();
            vmb = new ViewModelBitacora();
            BindingContext = evm = new EmpaqueViewModel();
            CargarListaEmpaques();
        }

        private async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloEmpaque = null;
            await Navigation.PushAsync(new EmpaquePage());
        }

        private async void CargarListaEmpaques()
        {
            ObservableCollection<Empaque> list = new ObservableCollection<Empaque>();
            list = await evm.GetBuscarEmpaque(Filtro, SwAvtivos.IsToggled);
            LvlListaEmpaque.ItemsSource = list;
        }

        private async void LvlListaEmpaque_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Empaque itemSelect = e.SelectedItem as Empaque;

            if (itemSelect != null)
            {
                GlobalObject.GloEmpaque = itemSelect;
                await Navigation.PushAsync(new EmpaquePage());
            }
        }

        private void LvlListaEmpaque_Refreshing(object sender, EventArgs e)
        {
            CargarListaEmpaques();
            LvlListaEmpaque.IsRefreshing = false;
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Confirmación", "¿Esta seguro de eliminar este Empaque?", "Si", "No"))
            {
                var item = (sender as MenuItem).CommandParameter;
                int id = int.Parse(item.ToString());
                Empaque empaque = new Empaque();
                empaque = await evm.GetEmpaqueId(id);
                bool R = await evm.DeleteEmpaque(id);
                if (R)
                {
                    CargarListaEmpaques();
                    await DisplayAlert("Verificación", "Se elimino el empaque con éxito", "OK");
                    await vmb.PostBitacora(DateTime.Now, GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2 +
                            " Elimino un Empaque. Empaque: " + empaque.Nombre + " " + empaque.Tamannio + " con " + empaque.Stock.ToString() + " de Stock");
                }
                else
                {
                    await DisplayAlert("Error de verificación", "No se logro eliminar el empaque", "OK");
                }
            }
        }

        private void SwAvtivos_Toggled(object sender, ToggledEventArgs e)
        {
            if (SwAvtivos.IsToggled == true)
            {
                LblStock.Text = "En stock";
            }
            else
            {
                LblStock.Text = "No stock";
            }
            LvlListaEmpaque.BeginRefresh();
            Task.Delay(2000);
            LvlListaEmpaque.EndRefresh();
        }

        private void SbBuscarUsu_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtro = SbBuscarUsu.Text;
            CargarListaEmpaques();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}