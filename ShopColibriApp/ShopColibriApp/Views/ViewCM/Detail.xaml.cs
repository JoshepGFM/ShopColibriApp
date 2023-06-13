using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        ImagenViewModel imgvm { get; set; }
        ViewModelBitacora vmb { get; set; }
        private string? Filtro {get; set;}
        public Detail()
        {
            InitializeComponent();

            vmb = new ViewModelBitacora();
            BindingContext = ivm = new InventarioViewModel();
            BindingContext = imgvm = new ImagenViewModel();

            CargarInventario();
            FuncionesUsuario();
        }

        private async void CargarInventario()
        {
            ObservableCollection<InventarioDTO> list = await ivm.GetInveBuscar(Filtro, SwStock.IsToggled);
            for (int i = 0; i < list.Count; ++i)
            {
                list[i].priImagen = "https://drive.google.com/uc?id=" + list[i].priImagen;
                for (int j = 0; j < list[i].imagenes.Count; ++j)
                {
                    list[i].imagenes[j].Imagen1 = "https://drive.google.com/uc?id=" + list[i].imagenes[j].Imagen1;
                }
            }
            LvlListaInventario.ItemsSource = list;
        }

        private void SbBuscarPro_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filtro = SbBuscarPro.Text.Trim();
            CargarInventario();
        }

        private async void LvlListaInventario_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            GlobalObject.GloImagenes.Clear();
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
            GlobalObject.GloImagenes.Clear();
            GlobalObject.GLoInventario = new Inventario();
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
                    bool T = await imgvm.DeleteImagen(GlobalObject.GloInven_DTO.imagenes);
                    bool R = await ivm.DeleteInventario(GlobalObject.GloInven_DTO.Id);

                    if (R)
                    {
                        await DisplayAlert("Validación exitosa", "Se elimino con éxito del Inventario", "OK");
                        await vmb.PostBitacora(DateTime.Now, GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2 +
                          " Elimino un Inventario. Inventario: " + GlobalObject.GloInven_DTO.NombrePro + " " + GlobalObject.GloInven_DTO.NombreEmp);
                        if (!T)
                        {
                            await DisplayAlert("Error de validación", "No se pudo eliminar las imágenes vinculadas (puede que este vinculado con otro elemento)", "OK");
                        }
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
        //Permite que al presionar la tecla back del teléfono la aplicación realice la funcionalidad de dentro de la función.
        protected override bool OnBackButtonPressed()
        {

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await DisplayAlert("Mensaje", "Quiere cerrar la aplicación", "SI", "NO"))
                    {
                        Process.GetCurrentProcess().Kill();
                    }
                });

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}