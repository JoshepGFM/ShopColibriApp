using Android.Provider;
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
				"Teléfono: " + GlobalObject.GloUsu.Telefono + "" +
				"Me gustaría hacer una(s) consulta(s) por el Producto: " + LblNombre.Text + ".";

			//Chat.Open(Whatsapp, Mensaje);
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