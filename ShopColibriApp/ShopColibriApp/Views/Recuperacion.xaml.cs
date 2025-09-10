using Android.App;
using ShopColibriApp.Models;
using ShopColibriApp.Servicios;
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
	public partial class Recuperacion : ContentPage
	{
		SendEmail SEmail { get; set; }
		UsuarioViewModel vmu { get; set; }
		public Recuperacion ()
		{
			InitializeComponent ();
			SEmail = new SendEmail ();
			vmu = new UsuarioViewModel ();
			ValidarUsuarioGlobal();
		}

        private async void BtnEnviarCorreo_Clicked(object sender, EventArgs e)
        {
			string g = TxtEmail.Text.Trim();
			if (vmu.IsValidEmail(g)) //Valida que el gmail sea de un formato correcto
			{
				Usuario usuario = await vmu.GetUsuario(g);
				if (usuario != null)
				{
					if (true)
					{
						//Genera un codígo random para enviar al correo y verificar que sea el usuario
						Random rnd = new Random();
						int Pin = rnd.Next(100000, 999999);
						GlobalObject.NumeroRecuperacion = Pin;//Pasa el valor para compararlo
						bool R = await SEmail.EnviarEmail(g, "Verificación de cuenta", Pin.ToString());//Envía el mensaje al correo correspondiente
						if (R)
						{
							GlobalObject.GloUsu = usuario;
							await DisplayAlert("Verificación de envío", "Se le a enviado el correo de verificación con éxito", "OK");
							await Navigation.PushAsync(new NewPassword());
						}
						else
						{
							await DisplayAlert("Error de envío", "No se pudo  realizar el envío, verifique el correo, o póngase en contacto con el administrador", "OK");
						}
					}
				}
				else
				{
					await DisplayAlert("Error de validación",g + ": No se encuentra registrado en este sitio","OK");
				}
			}
			else
			{
				await DisplayAlert("Error validación",g + ": No corresponde a un formato de correo","OK");
			}
        }

		private void ValidarUsuarioGlobal()
		{
			if(GlobalObject.GloUsu.IdUsuario > 0)
			{
				TxtEmail.Text = GlobalObject.GloUsu.Email;
				TxtEmail.IsEnabled = false;
				BtnCambiar.IsVisible = true;

			}
			else
			{
				TxtEmail.IsEnabled=true;
			}
		}

        private void BtnCambiar_Clicked(object sender, EventArgs e)
        {
			TxtEmail.IsEnabled = true;
        }
    }
}