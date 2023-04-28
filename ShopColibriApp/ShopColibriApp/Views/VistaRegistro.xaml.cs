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
    public partial class VistaRegistro : ContentPage
    {
        RegistroViewModel rvm { get; set; }
        private string? Filtro { get; set; }
        public VistaRegistro()
        {
            InitializeComponent();

            BindingContext = rvm = new RegistroViewModel();

            CargarListaRegistros();
        }

        private async void CargarListaRegistros()
        {
            ObservableCollection<RegistroDTO> list = new ObservableCollection<RegistroDTO>();
            list = await rvm.GetRegistroBuscar(Filtro);
            LvlListaRegistro.ItemsSource = list;
        }

        private void LvlListaRegistro_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            RegistroDTO registro = e.SelectedItem as RegistroDTO;
            if (registro != null)
            {
                GlobalObject.GloRegistro.Id = registro.Id;
                GlobalObject.GloRegistro.Fecha = registro.Fecha;
                GlobalObject.GloRegistro.HorasL = registro.HorasL;
                GlobalObject.GloRegistro.HorasX = registro.HorasX;
                GlobalObject.GloRegistro.HorasM = registro.HorasM;
                GlobalObject.GloRegistro.HorasJ = registro.HorasJ;
                GlobalObject.GloRegistro.HorasV = registro.HorasV;
                GlobalObject.GloRegistro.HorasS = registro.HorasS;
                GlobalObject.GloRegistro.TotalHoras = registro.TotalHoras;
                GlobalObject.GloRegistro.CostoHora = registro.CostoHora;
                GlobalObject.GloRegistro.Total = registro.Total;
                GlobalObject.GloRegistro.UsuarioIdUsuario = registro.UsuarioIdUsuario;
            }
            FmModificar.IsVisible = true;
        }

        private void LvlListaRegistro_Refreshing(object sender, EventArgs e)
        {
            CargarListaRegistros();
            FmModificar.IsVisible = false;
            LvlListaRegistro.IsRefreshing = false;
        }

        private void SbBuscarReg_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtro = SbBuscarReg.Text.Trim();
            CargarListaRegistros();
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            FmModificar.IsVisible = false;
            if (await DisplayAlert("Confirmación", "¿Esta seguro de eliminar este Usuario?", "Si", "No"))
            {
                var item = (sender as MenuItem).CommandParameter;
                int id = int.Parse(item.ToString());
                bool R = await rvm.DeleteRegistro(id);
                if (R)
                {
                    CargarListaRegistros();
                    await DisplayAlert("Verificación", "Se elimino el registro con éxito", "OK");
                }
                else
                {
                    await DisplayAlert("Error de verificación", "No se logro elimino el registro", "OK");
                }
            }
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new RegistroPage());
        }

        private async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloRegistro = new Models.Registro();
            await Navigation.PushAsync(new RegistroPage());
        }
    }
}