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
		VerificacionEmail VEmail { get; set; }
		UsuarioViewModel vmu { get; set; }
		public Recuperacion ()
		{
			InitializeComponent ();
			VEmail = new VerificacionEmail ();
			vmu = new UsuarioViewModel ();
		}

        private async void BtnEnviarCorreo_Clicked(object sender, EventArgs e)
        {
			string g = TxtEmail.Text.Trim();
			Usuario usuario = await vmu.GetUsuario(g);
			if (usuario != null)
			{
				Random rnd = new Random();
				int Pin = rnd.Next(10000,999999);
				GlobalObject.NumeroRecuperacion = Pin;
				string Mensaje = "Utilice este código: " + "<h1>"+Pin+"</h1>" + " en la App para verificar que es usted";
				bool R = VEmail.Index(g, "Verificación de cuenta", Mensaje);
				if (R)
				{
					await DisplayAlert("Verificación de envío", "Se le a enviado el correo de verificación con éxito" + Pin, "OK");
					await Navigation.PushAsync(new NewPassword());
				}
				else
				{
					await DisplayAlert("Error de envío", "No se pudo  realizar el envío, verifique el correo, o póngase en contacto con el administrador", "OK");
				}
			}
			else
			{
				await DisplayAlert("Error de validación",g + " No se encuentra registrado en este sitio","OK");
			}
        }
    }
}