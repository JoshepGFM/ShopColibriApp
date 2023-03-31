using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro : ContentPage
    {
        UsuarioViewModel vm;
        public Registro()
        {
            InitializeComponent();
            BindingContext = vm = new UsuarioViewModel();
            CargarTusuario();
            ValidarNivel();
        }

        private async void CargarTusuario()
        {
            PckTipo.ItemsSource = await vm.GetTUsuario();
        }

        private void ValidarNivel()
        {
            if (GlobalObject.GloUsu != null && GlobalObject.GloUsu.IdUsuario == 1)
            {
                if (GlobalObject.GloUsu_Registro != null)
                {
                    TxtNombre.Text = GlobalObject.GloUsu_Registro.Nombre;
                    TxtApellido1.Text = GlobalObject.GloUsu_Registro.Apellido1;
                    TxtApellido2.Text = GlobalObject.GloUsu_Registro.Apellido2;
                    TxtEmail.Text = GlobalObject.GloUsu_Registro.Email;
                    TxtEmailResp.Text = GlobalObject.GloUsu_Registro.EmailResp;
                    TxtTelefono.Text = GlobalObject.GloUsu_Registro.Telefono;
                    SelectItem(GlobalObject.GloUsu_Registro.TusuarioId);

                    BtnRegistrar.IsVisible = false;
                    BtnModificar.IsVisible = true;
                }
                else if(GlobalObject.GloUsu != null && !GlobalObject.AgregadoUsuSis)
                {
                    LlenarUsuario();
                }
                else if(GlobalObject.AgregadoUsuSis)
                {
                    BtnRegistrar.IsVisible = true;
                    BtnModificar.IsVisible = false;
                }
                PckTipo.IsVisible = true;
            }
            else
            {
                if (!GlobalObject.AgregadoUsuSis)
                {
                    if (GlobalObject.GloUsu != null) { LlenarUsuario(); }
                }
                else
                {
                    BtnRegistrar.IsVisible = true;
                    BtnModificar.IsVisible = false;
                }
                PckTipo.IsVisible = false;
            }
        }
        private void SwVer1_Toggled(object sender, ToggledEventArgs e)
        {
            if(SwVer1.IsToggled == false)
            {
                TxtContrasennia1.IsPassword = true;
            }
            else
            {
                TxtContrasennia1.IsPassword = false;
            }
        }

        private void SwVer2_Toggled(object sender, ToggledEventArgs e)
        {
            if (SwVer2.IsToggled == false)
            {
                TxtContrasennia2.IsPassword = true;
            }
            else
            {
                TxtContrasennia2.IsPassword = false;
            }
        }


        private bool ValidarElementos()
        {
            bool R = false;
            if (PckTipo.IsEnabled == false)
            {
                if (TxtNombre.Text != null && !string.IsNullOrEmpty(TxtNombre.Text.Trim()) &&
                    TxtApellido1.Text != null && !string.IsNullOrEmpty(TxtApellido1.Text.Trim()) &&
                    TxtApellido2.Text != null && !string.IsNullOrEmpty(TxtApellido2.Text.Trim()) &&
                    TxtEmail.Text != null && !string.IsNullOrEmpty(TxtEmail.Text.Trim()) &&
                    ValidarContrasennia() &&
                    TxtTelefono.Text != null && !string.IsNullOrEmpty(TxtTelefono.Text.Trim()) &&
                    PckTipo.SelectedIndex > -1
                    )
                {
                    if (vm.IsValidEmail(TxtEmail.Text.Trim()))
                    {
                         R= true;
                    }
                    else
                    {
                        DisplayAlert("Error validación", TxtEmail.Text + ": No corresponde a un formato de correo", "OK");
                    }
                }
                else
                {
                    if (TxtNombre.Text == null || string.IsNullOrEmpty(TxtNombre.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el Nombre", "OK");
                        TxtNombre.Focus();
                        return false;
                    }
                    if (TxtApellido1.Text == null || string.IsNullOrEmpty(TxtApellido1.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el primer apellido", "OK");
                        TxtApellido1.Focus();
                        return false;
                    }

                    if (TxtApellido2.Text == null || string.IsNullOrEmpty(TxtApellido2.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el segundo apellido", "OK");
                        TxtApellido2.Focus();
                        return false;
                    }

                    if (TxtEmail.Text == null || string.IsNullOrEmpty(TxtEmail.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el Email", "OK");
                        TxtEmail.Focus();
                        return false;
                    }

                    if (TxtContrasennia1.Text == null || string.IsNullOrEmpty(TxtContrasennia1.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere una Contraseña", "OK");
                        TxtContrasennia1.Focus();
                        return false;
                    }

                    if (TxtContrasennia2.Text == null || string.IsNullOrEmpty(TxtContrasennia2.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere confirmar la Contraseña", "OK");
                        TxtContrasennia2.Focus();
                        return false;
                    }

                    if (TxtTelefono.Text == null || string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el Teléfono", "OK");
                        TxtTelefono.Focus();
                        return false;
                    }

                    if (PckTipo.SelectedIndex == -1)
                    {
                        PckTipo.Focus();
                        DisplayAlert("Error de Validación", "Se requiere el Tipo de Usuario", "OK");
                        return false;
                    }
                }
            }
            else
            {
                if (TxtNombre.Text != null && !string.IsNullOrEmpty(TxtNombre.Text.Trim()) &&
                    TxtApellido1.Text != null && !string.IsNullOrEmpty(TxtApellido1.Text.Trim()) &&
                    TxtApellido2.Text != null && !string.IsNullOrEmpty(TxtApellido2.Text.Trim()) &&
                    TxtEmail.Text != null && !string.IsNullOrEmpty(TxtEmail.Text.Trim()) &&
                    ValidarContrasennia() &&
                    TxtTelefono.Text != null && !string.IsNullOrEmpty(TxtTelefono.Text.Trim())
                    )
                {
                    if (vm.IsValidEmail(TxtEmail.Text.Trim()))
                    {
                        R = true;
                    }
                    else
                    {
                        DisplayAlert("Error validación", TxtEmail.Text + ": No corresponde a un formato de correo", "OK");
                    }
                }
                else
                {
                    if (TxtNombre.Text == null || string.IsNullOrEmpty(TxtNombre.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el Nombre", "OK");
                        TxtNombre.Focus();
                        return false;
                    }
                    if (TxtApellido1.Text == null || string.IsNullOrEmpty(TxtApellido1.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el primer apellido", "OK");
                        TxtApellido1.Focus();
                        return false;
                    }

                    if (TxtApellido2.Text == null || string.IsNullOrEmpty(TxtApellido2.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el segundo apellido", "OK");
                        TxtApellido2.Focus();
                        return false;
                    }

                    if (TxtEmail.Text == null || string.IsNullOrEmpty(TxtEmail.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el Email", "OK");
                        TxtEmail.Focus();
                        return false;
                    }

                    if (TxtContrasennia1.Text == null || string.IsNullOrEmpty(TxtContrasennia1.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere una Contraseña", "OK");
                        TxtContrasennia1.Focus();
                        return false;
                    }

                    if (TxtContrasennia2.Text == null || string.IsNullOrEmpty(TxtContrasennia2.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere confirmar la Contraseña", "OK");
                        TxtContrasennia2.Focus();
                        return false;
                    }

                    if (TxtTelefono.Text == null || string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                    {
                        DisplayAlert("Error de Validación", "Se requiere el Teléfono", "OK");
                        TxtTelefono.Focus();
                        return false;
                    }
                }
            }

            return R;
        }

        private bool ValidarContrasennia()
        {
            if (TxtContrasennia1.Text != null && !string.IsNullOrEmpty(TxtContrasennia1.Text.Trim()) &&
                TxtContrasennia2.Text != null && !string.IsNullOrEmpty(TxtContrasennia2.Text.Trim()))
            {
                if (TxtContrasennia1.Text.Trim() == TxtContrasennia2.Text.Trim())
                {
                    return true;
                }
                else
                {
                    DisplayAlert("Error de Validación", "Las contraseñas no coinciden", "OK");
                    return false;
                }
            }else { return false; }
        }

        //Metodo para seleccionar un tipo de usuario en el picker según el id
        private async void SelectItem(int index)
        {
            var list = await vm.GetTUsuario();
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i].Id.ToString() == index.ToString())
                {
                    PckTipo.SelectedIndex = i;
                }
            }
        }

        private void LlenarUsuario()
        {
            TxtNombre.Text = GlobalObject.GloUsu.Nombre;
            TxtApellido1.Text = GlobalObject.GloUsu.Apellido1;
            TxtApellido2.Text = GlobalObject.GloUsu.Apellido2;
            TxtEmail.Text = GlobalObject.GloUsu.Email;
            TxtEmailResp.Text = GlobalObject.GloUsu.EmailResp;
            TxtTelefono.Text = GlobalObject.GloUsu.Telefono;
            SelectItem(GlobalObject.GloUsu.TusuarioId);

            BtnRegistrar.IsVisible = false;
            BtnModificar.IsVisible = true;
        }

        private async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ValidarElementos())
                {
                    Tusuario tus = PckTipo.SelectedItem as Tusuario;
                    int IdTus;
                    if (PckTipo.IsEnabled == false)
                    {
                        IdTus = tus.Id;
                    }
                    else
                    {
                        IdTus = 3;
                    }
                    string emailRes = "";
                    if (TxtEmailResp.Text == null || string.IsNullOrEmpty(TxtEmailResp.Text.Trim()))
                    {
                        emailRes = null;
                    }
                    else
                    {
                        emailRes = TxtEmailResp.Text.Trim();
                    }
                    var answer = await DisplayAlert("Confirmación", "¿Quiere registrarse?", "Si", "No");
                    if (answer)
                    {
                        bool R = await vm.PostUsuario(TxtNombre.Text.Trim(),
                                                      TxtApellido1.Text.Trim(),
                                                      TxtApellido2.Text.Trim(),
                                                      TxtEmail.Text.Trim(),
                                                      TxtContrasennia2.Text.Trim(),
                                                      emailRes,
                                                      TxtTelefono.Text.Trim(),
                                                      IdTus);

                        if (R)
                        {

                            await DisplayAlert("Validación exitosa", "Se agrego con éxito el Usuario", "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Error de Validación", "Se ocasiono un error al ingresar el Usuario", "OK");
                        }
                    }
                }
            }catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "OK");
            }
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            if (ValidarElementos())
            {
                Usuario usuario = new Usuario();
                Tusuario tus = PckTipo.SelectedItem as Tusuario;
                if (GlobalObject.GloUsu_Registro != null)
                {
                    usuario.IdUsuario = GlobalObject.GloUsu_Registro.IdUsuario;
                }
                else if (GlobalObject.GloUsu != null)
                {
                    usuario.IdUsuario = GlobalObject.GloUsu.IdUsuario;
                }
                usuario.Nombre = TxtNombre.Text.Trim();
                usuario.Apellido1 = TxtApellido1.Text.Trim();
                usuario.Apellido2 = TxtApellido2.Text.Trim();
                usuario.Email = TxtEmail.Text.Trim();
                usuario.Contrasennia = TxtContrasennia1.Text.Trim();
                string emailRes = "";
                if (TxtEmailResp.Text == null || string.IsNullOrEmpty(TxtEmailResp.Text.Trim()))
                {
                    emailRes = null;
                }
                else
                {
                    emailRes = TxtEmailResp.Text.Trim();
                }
                usuario.EmailResp = emailRes;
                usuario.Telefono = TxtTelefono.Text.Trim();
                usuario.TusuarioId = tus.Id;
                bool R = await vm.PatchUsuario(usuario,usuario.Contrasennia);
                if (R)
                {
                    GlobalObject.GloUsu_Registro = null;
                    await DisplayAlert("Validación exitosa", "Se Modifico con éxito el Usuario", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error de Validación", "Se ocasiono un error al modificar el Usuario", "OK");
                }
            }
        }
    }
}