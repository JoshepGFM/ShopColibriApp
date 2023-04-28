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

        private async void LvlListaInventario_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            InventarioDTO inventario = e.SelectedItem as InventarioDTO;

            if (inventario != null)
            {
                if (GlobalObject.GloUsu.TusuarioId == 3 || GlobalObject.GloUsu == null)
                {
                    GlobalObject.GloInven_DTO = inventario;
                    await Navigation.PushAsync(new VistaDetail());
                }
                if (GlobalObject.GloUsu.TusuarioId == 1 || GlobalObject.GloUsu.TusuarioId == 2)
                {
                    GlobalObject.GloInven_DTO = inventario;
                    FmModificar.IsVisible = true;
                    FmIElimnar.IsVisible = true;
                }
            }
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
            FmModificar.IsVisible = false;
            FmIElimnar.IsVisible = false;
            LvlListaInventario.IsRefreshing = false;
        }

        private async void BtnAgregar_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloInven_DTO = new InventarioDTO();
            await Navigation.PushAsync(new InventarioPage());
        }

        private void FuncionesUsuario()
        {
            if(GlobalObject.GloUsu.TusuarioId == 1 || GlobalObject.GloUsu.TusuarioId == 2)
            {
                FmAgregar.IsVisible = true;
                LblActivo.IsVisible = true;
                SwStock.IsVisible = true;
            }
            else
            {
                FmAgregar.IsVisible = false;
                FmIElimnar.IsVisible = false;
                FmModificar.IsVisible = false;
                LblActivo.IsVisible = false;
                SwStock.IsVisible = false;
            }
        }

        private async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (LvlListaInventario.SelectedItem != null)
            {
                if (await DisplayAlert("Confirmación", "Esta seguro de eliminar " + GlobalObject.GloInven_DTO.NombrePro + " de " + GlobalObject.GloInven_DTO.NombreEmp + "?", "Si", "No"))
                {
                    bool R = await ivm.DeleteInventario(GlobalObject.GloInven_DTO.Id);

                    if (R)
                    {
                        await DisplayAlert("Validación exitosa", "Se elimino con éxito del Inventario", "OK");
                        LvlListaInventario.BeginRefresh();
                        Task.Delay(2000);
                        LvlListaInventario.EndRefresh();
                    }
                    else
                    {
                        await DisplayAlert("Error de validación", "No se logro eliminar del Inventario", "OK");
                    }
                }
            }
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            if (LvlListaInventario.SelectedItem != null)
            {
                await Navigation.PushAsync(new InventarioPage());
            }
        }
    }
}