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
    public partial class ControlMarmitaPage : ContentPage
    {
        UsuarioViewModel uvm { get; set; }
        public ControlMarmitaPage()
        {
            InitializeComponent();

            BindingContext = uvm = new UsuarioViewModel();

            CargarListaUsuarios();
        }

        private async void CargarListaUsuarios()
        {
            ObservableCollection<Usuario> list = new ObservableCollection<Usuario>();
            if (GlobalObject.GloListUsu != null)
            {
                list = GlobalObject.GloListUsu;
            }
            LvlListaUsuarios.ItemsSource = list;
        }

        private void LvlListaUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void LvlListaUsuarios_Refreshing(object sender, EventArgs e)
        {
            CargarListaUsuarios();
            LvlListaUsuarios.IsRefreshing = false;
        }

        private void BtnGuardar_Clicked(object sender, EventArgs e)
        {

        }

        private async void BtnagregarUsu_Clicked(object sender, EventArgs e)
        {
            
            GlobalObject.GloControMarmi_Cont.Fecha = PckFecha.Date;
            GlobalObject.GloControMarmi_Cont.HoraEn = TmHoraEn.Time;
            GlobalObject.GloControMarmi_Cont.HoraAp = TmHoraAp.Time;
            if (TxtTemperatura.Text == null)
            {
                GlobalObject.GloControMarmi_Cont.Temperatura = 0;
            }
            else
            {
                GlobalObject.GloControMarmi_Cont.Temperatura = int.Parse(TxtTemperatura.Text.Trim());
            }
            if (TxtIntensidaMov.Text == null)
            {
                GlobalObject.GloControMarmi_Cont.IntensidadMov = null;
            }
            else
            {
                GlobalObject.GloControMarmi_Cont.IntensidadMov = TxtIntensidaMov.Text;
            }
            if (TxtLote.Text == null)
            {
                GlobalObject.GloControMarmi_Cont.Lote = null;
            }
            else
            {
                GlobalObject.GloControMarmi_Cont.Lote = TxtLote.Text;
            }

            await Navigation.PushAsync(new SeleccionUsuario());
        }

        private async void BtnVerControles_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VistaControlMarmita());
        }
    }
}