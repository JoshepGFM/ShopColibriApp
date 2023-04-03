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
    public partial class NewPassword : ContentPage
    {
        UsuarioViewModel vmu { get; set; }
        public NewPassword()
        {
            InitializeComponent();
            vmu = new UsuarioViewModel();
        }

        private async void BtnCambiar_Clicked(object sender, EventArgs e)
        {
            if (VerificarElementos())
            {
                bool R = await vmu.PatchUsuario(GlobalObject.GloUsu, TxtPass1.Text.Trim());
                if (R)
                {
                    await DisplayAlert("Verificación de contraseña", "Se realizo el cambio de contraseña exitosamente", "OK");
                    await Navigation.PushAsync(new Login());
                }
                else
                {
                    await DisplayAlert("Error de modificación","No se pudo realizar el cambio de contraseña","OK");
                }
            }
        }

        private  bool VerificarElementos()
        {
            bool R = false;
            if (TxtPass1.Text !=null && !string.IsNullOrEmpty(TxtPass1.Text.Trim()) &&
                TxtPass2.Text != null && !string.IsNullOrEmpty(TxtPass2.Text.Trim()) &&
                TxtPin.Text != null && !string.IsNullOrEmpty(TxtPin.Text.Trim()))
                {
                    if (TxtPass1.Text == TxtPass2.Text &&
                        vmu.IsPasswordSecure(TxtPass1.Text.Trim())&&
                        GlobalObject.NumeroRecuperacion.ToString() == TxtPin.Text.Trim()
                        )
                    {
                        R = true;
                    }
                    else
                    {
                    if (GlobalObject.NumeroRecuperacion.ToString() != TxtPin.Text.Trim())
                    {
                        DisplayAlert("Error de validación", "La clave o el pin es incorrecto", "OK");
                        TxtPass1.Focus();
                        return false;
                    }
                    if (TxtPass1.Text.Trim() != TxtPass2.Text.Trim())
                        {
                            DisplayAlert("Error de validación", "Las contraseñas no son iguales", "OK");
                            TxtPass1.Focus();
                            return false;
                        }
                        if (!vmu.IsPasswordSecure(TxtPass1.Text.Trim()))
                        {
                            DisplayAlert("Error de contraseña", "La contraseña no cuenta con los parámetros necesario, debe contar con:/n" +
                                "(8 dígitos, " +
                                "que tenga un numero (0-9), " +
                                "que tenga cuente con letras minúsculas y mayúsculas.", "OK");
                            TxtPass1.Focus();
                            return false;
                        }
                    }
            }
            else
            {
                if (TxtPass1.Text == null || string.IsNullOrEmpty(TxtPass1.Text.Trim()))
                {
                    DisplayAlert("Error de validación", "Debe de crear una nueva contraseña", "OK");
                    TxtPass1.Focus();
                    return false;
                }
                if (TxtPass2.Text == null || string.IsNullOrEmpty(TxtPass2.Text.Trim()))
                {
                    DisplayAlert("Error de validación", "Debe de confirmar la contraseña", "OK");
                    TxtPass2.Focus();
                    return false;
                }
                if (TxtPin.Text == null || string.IsNullOrEmpty(TxtPin.Text.Trim()))
                {
                    DisplayAlert("Error de validación", "La clave o pin no es correcta verifique", "OK");
                    TxtPin.Focus();
                    return false;
                }
                
            }
            return R;
        }

        private void SwVer1_Toggled(object sender, ToggledEventArgs e)
        {
            if(SwVer1.IsToggled == true)
            {
                TxtPass1.IsPassword = false;
            }
            else 
            { 
                TxtPass1.IsPassword = true; 
            }
        }

        private void SwVer2_Toggled(object sender, ToggledEventArgs e)
        {
            if (SwVer2.IsToggled == true)
            {
                TxtPass2.IsPassword = false;
            }
            else
            {
                TxtPass2.IsPassword = true;
            }
        }
    }
}