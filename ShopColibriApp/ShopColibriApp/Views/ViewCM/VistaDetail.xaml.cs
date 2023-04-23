using Android.Provider;
using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.OpenWhatsApp;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views.ViewCM
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VistaDetail : ContentPage
	{
		UsuarioViewModel uvm { get; set; }
		public VistaDetail ()
		{
			InitializeComponent ();

			BindingContext = uvm = new UsuarioViewModel ();

			RellenarElementos();
		}

        private async void BtnContactar_Clicked(object sender, EventArgs e)
        {
			Usuario usuario = new Usuario ();

			usuario = await uvm.GetUsuario("verificacionesshopcolibri@gmail.com");

			string Whatsapp = "+506" + usuario.Telefono.ToString();
			string Mensaje = "Cliente: " + GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2 + "" +
				" Teléfono: " + GlobalObject.GloUsu.Telefono + ", " +
				"Me gustaría hacer una(s) consulta(s) por el Producto: " + LblNombre.Text + " " + LblTamannio.Text + ".";
			try
			{
			Chat.Open(Whatsapp, Mensaje);
			}
			catch (Exception ex)
			{
				if (ex.Message == "No Activity found to handle Intent { act=android.intent.action.VIEW dat=whatsapp://send/... flg=0x10000000 }")
				{
					await DisplayAlert("Error de ingreso a App", "Se requiere de whatsapp instalado en el celular para usar esta función", "OK");
				}
				else
				{
                    await DisplayAlert("Error de ingreso a App", ex.Message, "OK");
                }
			}
         }

		private void RellenarElementos()
		{
			if (GlobalObject.GLoInventario != null)
			{
				LblNombre.Text = GlobalObject.GloInven_DTO.NombrePro;
				//ImgProducto.Images = ;
				LblDescripcion.Text = GlobalObject.GloInven_DTO.DescripcionPro;
				LblTamannio.Text = GlobalObject.GloInven_DTO.NombreEmp;
			}
		}
    }
}