using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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
    public partial class EntradasPage : ContentPage
    {
        FechaIngreViewModel fivm { get; set; }
        private bool? seleccion = null;
        public EntradasPage()
        {
            InitializeComponent();

            BindingContext = fivm = new FechaIngreViewModel();

            DateTime fechain = DateTime.Now;
            DtpInicio.Date = fechain.Date.AddMonths(-1);
            CargarEntradas();
            CargarPckEntradas();
            SumaTotal();
        }

        public async void CargarEntradas()
        {
            ObservableCollection<FechaIngreDTO> list = new ObservableCollection<FechaIngreDTO>();
            list = await fivm.GetFechaIngres(DtpInicio.Date, DtpFinal.Date, seleccion, CkbTodo.IsToggled);
            LvlListaEntrada.ItemsSource = list.Reverse();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            ValidarTipoEntrada();
            CargarEntradas();
            SumaTotal();
        }

        private void LvlListaEntrada_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void LvlListaEntrada_Refreshing(object sender, EventArgs e)
        {
            ValidarTipoEntrada();
            CargarEntradas();
            SumaTotal();
            LvlListaEntrada.IsRefreshing = false;
        }

        private void CkbTodo_Toggled(object sender, ToggledEventArgs e)
        {
            PckEntradas.SelectedIndex = -1;
            seleccion = null;
            ValidarTipoEntrada();
            CargarEntradas();
            SumaTotal();
        }

        private void CargarPckEntradas()
        {
            List<string> entradas = new List<string>();

            entradas.Add("Empaques");
            entradas.Add("Inventarios");

            PckEntradas.ItemsSource = entradas;
        }

        private void ValidarTipoEntrada()
        {
            string R = PckEntradas.SelectedItem as string;
            if(R == "Empaques")
            {
                seleccion = true;
            }
            if(R == "Inventarios")
            {
                seleccion = false;
            }
            if(R == null)
            {
                seleccion = null;
            }
        }

        private async void SumaTotal()
        {
            ObservableCollection<FechaIngreDTO> list = new ObservableCollection<FechaIngreDTO>();
            string producto = PckEntradas.SelectedItem as string;
            if (producto != null)
            {
                list = await fivm.GetFechaIngres(DtpInicio.Date, DtpFinal.Date, seleccion, CkbTodo.IsToggled);
            }
            else
            {
                list = await fivm.GetFechaIngres(DtpInicio.Date, DtpFinal.Date, null, CkbTodo.IsToggled);
            }

            int suma = 0;
            foreach (var item in list)
            {
                suma += item.Entrada;
            }

            LblTotal.Text = suma.ToString();
        }

    }
}