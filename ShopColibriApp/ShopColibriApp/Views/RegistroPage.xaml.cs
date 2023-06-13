using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
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
    public partial class RegistroPage : ContentPage
    {
        UsuarioViewModel uvm { get; set; }
        RegistroViewModel rvm { get; set; }
        ViewModelBitacora vmb { get; set; }
        public RegistroPage()
        {
            InitializeComponent();

            vmb = new ViewModelBitacora();
            BindingContext = uvm = new UsuarioViewModel();
            BindingContext = rvm = new RegistroViewModel();

            CargarListaUsuarios();
            ValidarBotones();
        }

        private async void CargarListaUsuarios()
        {
            List<Usuario> list = new List<Usuario>();
            list = await uvm.GetUser();
            for(int i = 0; i < list.Count; i++)
            {
                list[i].Nombre = list[i].Nombre + " " + list[i].Apellido1 + " " + list[i].Apellido2;
            }
            PckUsuario.ItemsSource = list;
        }

        private bool ValidarEntradas()
        {
            bool R = false;
            if (PckUsuario.SelectedIndex != -1 &&
                PckFecha.Date != null &&
                TxtLunes.Text != null && !string.IsNullOrWhiteSpace(TxtLunes.Text.Trim()) &&
                TxtMartes.Text != null && !string.IsNullOrWhiteSpace(TxtLunes.Text.Trim()) &&
                TxtMiercoles.Text != null && !string.IsNullOrWhiteSpace(TxtLunes.Text.Trim()) &&
                TxtJueves.Text != null && !string.IsNullOrWhiteSpace(TxtLunes.Text.Trim()) &&
                TxtViernes.Text != null && !string.IsNullOrWhiteSpace(TxtLunes.Text.Trim()) &&
                TxtSabado.Text != null && !string.IsNullOrWhiteSpace(TxtLunes.Text.Trim()) &&
                int.Parse(LblTotalHoras.Text) > 0 &&
                TxtCostoHora.Text != null && int.Parse(TxtCostoHora.Text.Trim()) > 0)
            {
                R = true;
            }
            else
            {
                if (PckUsuario.SelectedIndex == -1)
                {
                    PckUsuario.Focus();
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un Usuario", "Ok");
                    return false;
                }
                if (PckFecha.Date == null)
                {
                    PckFecha.Focus();
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un fecha", "Ok");
                    return false;
                }
                if (TxtLunes.Text == null)
                {
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un numero en Lunes", "Ok");
                    TxtLunes.Focus();
                    return false;
                }
                if (TxtMartes.Text == null)
                {
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un numero en Martes", "Ok");
                    TxtMartes.Focus();
                    return false;
                }
                if (TxtMiercoles.Text == null)
                {
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un numero en Miércoles", "Ok");
                    TxtMiercoles.Focus();
                    return false;
                }
                if (TxtJueves.Text == null)
                {
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un numero en Jueves", "Ok");
                    TxtJueves.Focus();
                    return false;
                }
                if (TxtViernes.Text == null)
                {
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un numero en Viernes", "Ok");
                    TxtViernes.Focus();
                    return false;
                }
                if (TxtSabado.Text == null)
                {
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un numero en Sabado", "Ok");
                    TxtSabado.Focus();
                    return false;
                }
                if (LblTotalHoras.Text == null || int.Parse(LblTotalHoras.Text) < 1)
                {
                    DisplayAlert("Error de Validación", "Se requiere que el total de horas sea mayor a 0", "Ok");
                    return false;
                }
                if (TxtCostoHora.Text == null || int.Parse(TxtCostoHora.Text) == 0)
                {
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un precio para las Horas", "Ok");
                    TxtCostoHora.Focus();
                    return false;
                }
            }
            return R;
        }

        private void ValidarBotones()
        {
            if (GlobalObject.GloRegistro.Id == 0)
            {
                BtnGuardar.IsVisible = true;
                BtnModificar.IsVisible = false;
            }
            else
            {
                BtnGuardar.IsVisible = false;
                LblTituloRegistro.Text = "Modificar Registro";
                BtnModificar.IsVisible = true;

                SeleccionarUsuario(GlobalObject.GloRegistro.UsuarioIdUsuario);
                PckFecha.Date = GlobalObject.GloRegistro.Fecha;
                TxtLunes.Text = GlobalObject.GloRegistro.HorasL.ToString();
                TxtMartes.Text = GlobalObject.GloRegistro.HorasX.ToString();
                TxtMiercoles.Text = GlobalObject.GloRegistro.HorasM.ToString();
                TxtJueves.Text = GlobalObject.GloRegistro.HorasJ.ToString();
                TxtViernes.Text = GlobalObject.GloRegistro.HorasV.ToString();
                TxtSabado.Text = GlobalObject.GloRegistro.HorasS.ToString();
                LblTotalHoras.Text = GlobalObject.GloRegistro.TotalHoras.ToString();
                TxtCostoHora.Text = GlobalObject.GloRegistro.CostoHora.ToString();
                LblTotal.Text = GlobalObject.GloRegistro.Total.ToString();
            }
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            if (ValidarEntradas())
            {
                Usuario usuario = PckUsuario.SelectedItem as Usuario;
                bool R = await rvm.PostRegistro(usuario.IdUsuario, PckFecha.Date, int.Parse(TxtLunes.Text.Trim()), int.Parse(TxtMartes.Text.Trim()),
                     int.Parse(TxtMiercoles.Text.Trim()), int.Parse(TxtJueves.Text.Trim()), int.Parse(TxtViernes.Text.Trim()), 
                     int.Parse(TxtViernes.Text.Trim()), int.Parse(LblTotalHoras.Text), int.Parse(TxtCostoHora.Text.Trim()),
                     int.Parse(LblTotal.Text.Trim()));
                if (R)
                {
                    await DisplayAlert("Validación exitosa", "Se a ingresado el registro con éxito", "OK");
                    await vmb.PostBitacora(DateTime.Now, GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2 +
                            " Guardo un Registro. Registro: Del usuario, " + usuario.Nombre + " del " + PckFecha.Date.ToString());
                    await Navigation.PushAsync(new VistaRegistro());
                }
                else
                {
                    await DisplayAlert("Error validación", "No se logro ingresar el registro", "OK");
                }
            }
        }

        private void CalculoTotales()
        {
            int l = 0;
            int x = 0;
            int m = 0;
            int j = 0;
            int v = 0;
            int s = 0;
            int cosHora = 0;
            int toHo = 0;
            int total =0;
            if (TxtLunes.Text != null && !string.IsNullOrEmpty(TxtLunes.Text))
            {
                l = int.Parse(TxtLunes.Text);
            }
            if (TxtMartes.Text != null && !string.IsNullOrEmpty(TxtMartes.Text))
            {
                x = int.Parse(TxtMartes.Text);
            }
            if (TxtMiercoles.Text != null && !string.IsNullOrEmpty(TxtMiercoles.Text))
            {
                m = int.Parse(TxtMiercoles.Text);
            }
            if (TxtJueves.Text != null && !string.IsNullOrEmpty(TxtJueves.Text))
            {
                j = int.Parse(TxtJueves.Text);
            }
            if (TxtViernes.Text != null && !string.IsNullOrEmpty(TxtViernes.Text))
            {
                v = int.Parse(TxtViernes.Text);
            }
            if (TxtSabado.Text != null && !string.IsNullOrEmpty(TxtSabado.Text))
            {
                s = int.Parse(TxtSabado.Text);
            }
            if (TxtCostoHora.Text != null && !string.IsNullOrEmpty(TxtCostoHora.Text))
            {
                cosHora = int.Parse(TxtCostoHora.Text);
            }

            toHo = l+x+m+j+v+s;

            total = toHo * cosHora;

            LblTotalHoras.Text = toHo.ToString();
            LblTotal.Text = total.ToString();
        }

        private void TxtLunes_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculoTotales();
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            if (ValidarEntradas())
            {
                Usuario usuario = PckUsuario.SelectedItem as Usuario;
                int idRegistro = GlobalObject.GloRegistro.Id;
                bool R = await rvm.PutRegistro(idRegistro, usuario.IdUsuario, PckFecha.Date, int.Parse(TxtLunes.Text.Trim()), int.Parse(TxtMartes.Text.Trim()),
                     int.Parse(TxtMiercoles.Text.Trim()), int.Parse(TxtJueves.Text.Trim()), int.Parse(TxtViernes.Text.Trim()),
                     int.Parse(TxtViernes.Text.Trim()), int.Parse(LblTotalHoras.Text), int.Parse(TxtCostoHora.Text.Trim()),
                     int.Parse(LblTotal.Text.Trim()));
                if (R)
                {
                    await DisplayAlert("Validación exitosa", "Se a modificado el registro con éxito", "OK");
                    await vmb.PostBitacora(DateTime.Now, GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2 +
                            " Modifico un Registro. Registro: Del usuario, " + usuario.Nombre + " del " + PckFecha.Date.ToString());
                    await Navigation.PushAsync(new VistaRegistro());
                }
                else
                {
                    await DisplayAlert("Error validación", "No se logro modificado el registro", "OK");
                }
            }
        }

        private async void SeleccionarUsuario(int id)
        {
            List<Usuario> list = new List<Usuario>();
            list = await uvm.GetUser();
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].IdUsuario == id)
                {
                    PckUsuario.SelectedIndex = i;
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new VistaRegistro());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}