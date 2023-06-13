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
		private int campo = 0;
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
				if (GlobalObject.GloInven_DTO.imagenes.Count > 0)
				{
					if (GlobalObject.GloInven_DTO.imagenes.Count > 1)
					{
						FmAdelante.IsVisible = true;
					}
                    ImgDetail.Source = GlobalObject.GloInven_DTO.imagenes[campo].Imagen1.ToString();
                }
				LblDescripcion.Text = GlobalObject.GloInven_DTO.DescripcionPro;
				LblTamannio.Text = GlobalObject.GloInven_DTO.NombreEmp;
				LblPrecio.Text = GlobalObject.GloInven_DTO.PrecioUn.ToString();
			}
		}

        private void btnAnterior_Clicked(object sender, EventArgs e)
        {
			if (campo > -1)
			{
				campo -= 1;
			}
			ValidarTransicion(campo, GlobalObject.GloInven_DTO.imagenes.Count);
        }

        private void btnSiguiente_Clicked(object sender, EventArgs e)
        {
            if (campo < GlobalObject.GloInven_DTO.imagenes.Count-1)
            {
                campo += 1;
            }
            ValidarTransicion(campo, GlobalObject.GloInven_DTO.imagenes.Count);
        }

		private void ValidarTransicion(int n, int cantidad)
		{
			if (n > 0)
			{
				FmAtras.IsVisible = true;
			}
			if (n < cantidad)
			{
				FmAdelante.IsVisible = true;
			}
			if(n == cantidad - 1)
			{
				FmAdelante.IsVisible = false;
            }
			if (n == 0)
			{
				FmAtras.IsVisible = false;
			}
			if (n < GlobalObject.GloInven_DTO.imagenes.Count)
			{
				ImgDetail.Source = GlobalObject.GloInven_DTO.imagenes[n].Imagen1.ToString();
			}
        }

        protected override bool OnBackButtonPressed()
        {

			Navigation.PushAsync(new MainPage());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }

    }
}