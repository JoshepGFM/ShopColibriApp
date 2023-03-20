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
                PckTipo.IsVisible = true;
            }
            else
            {
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

        private async void BtnRegistrar_Clicked(object sender, EventArgs e)
        {
            if (ValidarElementos())
            {
                Tusuario tus = PckTipo.SelectedItem as Tusuario;
                int IdTus;
                if (GlobalObject.GloUsu.IdUsuario == 1)
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
                var answer = await DisplayAlert("Requiere confirmacion", "¿Esta seguro de registrar?", "Si","No");
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
                        await DisplayAlert("Validacion exitoza", "Se agrego con exito el Usuario", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error de Validacion", "Se ocaciono un error al ingresar el Usuario", "OK");
                    }
                }
            }
        }

        private bool ValidarElementos()
        {
            bool R = false;
            if (GlobalObject.GloUsu.IdUsuario == 1)
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
                    R = true;
                }
                else
                {
                    if (TxtNombre.Text == null || string.IsNullOrEmpty(TxtNombre.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el Nombre", "OK");
                        TxtNombre.Focus();
                        return false;
                    }
                    if (TxtApellido1.Text == null || string.IsNullOrEmpty(TxtApellido1.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el primer apellido", "OK");
                        TxtApellido1.Focus();
                        return false;
                    }

                    if (TxtApellido2.Text == null || string.IsNullOrEmpty(TxtApellido2.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el segundo apellido", "OK");
                        TxtApellido2.Focus();
                        return false;
                    }

                    if (TxtEmail.Text == null || string.IsNullOrEmpty(TxtEmail.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el Email", "OK");
                        TxtEmail.Focus();
                        return false;
                    }

                    if (TxtContrasennia1.Text == null || string.IsNullOrEmpty(TxtContrasennia1.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere una Contraseña", "OK");
                        TxtContrasennia1.Focus();
                        return false;
                    }

                    if (TxtContrasennia2.Text == null || string.IsNullOrEmpty(TxtContrasennia2.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere confirmar la Contraseña", "OK");
                        TxtContrasennia2.Focus();
                        return false;
                    }

                    if (TxtTelefono.Text == null || string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el Telefono", "OK");
                        TxtTelefono.Focus();
                        return false;
                    }

                    if (PckTipo.SelectedIndex == -1)
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el Tipo de Usuario", "OK");
                        PckTipo.Focus();
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
                    R = true;
                }
                else
                {
                    if (TxtNombre.Text == null || string.IsNullOrEmpty(TxtNombre.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el Nombre", "OK");
                        TxtNombre.Focus();
                        return false;
                    }
                    if (TxtApellido1.Text == null || string.IsNullOrEmpty(TxtApellido1.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el primer apellido", "OK");
                        TxtApellido1.Focus();
                        return false;
                    }

                    if (TxtApellido2.Text == null || string.IsNullOrEmpty(TxtApellido2.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el segundo apellido", "OK");
                        TxtApellido2.Focus();
                        return false;
                    }

                    if (TxtEmail.Text == null || string.IsNullOrEmpty(TxtEmail.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el Email", "OK");
                        TxtEmail.Focus();
                        return false;
                    }

                    if (TxtContrasennia1.Text == null || string.IsNullOrEmpty(TxtContrasennia1.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere una Contraseña", "OK");
                        TxtContrasennia1.Focus();
                        return false;
                    }

                    if (TxtContrasennia2.Text == null || string.IsNullOrEmpty(TxtContrasennia2.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere confirmar la Contraseña", "OK");
                        TxtContrasennia2.Focus();
                        return false;
                    }

                    if (TxtTelefono.Text == null || string.IsNullOrEmpty(TxtTelefono.Text.Trim()))
                    {
                        DisplayAlert("Error de Validacion", "Se requiere el Telefono", "OK");
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
                    DisplayAlert("Error de Validacion", "Las contraseñas no coinciden", "OK");
                    return false;
                }
            }else { return false; }
        }
    }
}