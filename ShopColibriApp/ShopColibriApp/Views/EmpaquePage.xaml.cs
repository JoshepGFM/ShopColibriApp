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
    public partial class EmpaquePage : ContentPage
    {
        EmpaqueViewModel evm { get; set; }
        public EmpaquePage()
        {
            InitializeComponent();
            BindingContext = evm = new EmpaqueViewModel();
            ValidarInicio();
        }

        private void ValidarInicio()
        {
            if (GlobalObject.GloEmpaque != null)
            {
                TxtNombre.Text = GlobalObject.GloEmpaque.Nombre;
                TxtTamannio.Text = GlobalObject.GloEmpaque.Tamannio;
                LblStock.Text = GlobalObject.GloEmpaque.Stock.ToString();
                BtnGuardar.IsVisible = false;
                LblTituloEmpaque.Text = "Modificar Empaque";
                BtnModificar.IsVisible = true;
            }
            else
            {
                BtnGuardar.IsVisible = true;
                BtnModificar.IsVisible = false;
            }
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            if (ValidarEntradas())
            {
                bool R = await evm.PostEmpaque(TxtNombre.Text.Trim(), TxtTamannio.Text.Trim(), int.Parse(TxtStock.Text.Trim()));
                if (R)
                {
                    await DisplayAlert("Validación exitosa", "Se agrego exitosamente el Empaque", "Ok");
                    await Navigation.PushAsync(new VistaEmpaque());
                }
                else
                {
                    await DisplayAlert("Error de validación", "No se logro agregar el Empaque", "Ok");
                }
            }
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            if (ValidarEntradas())
            {
                int id = GlobalObject.GloEmpaque.Id;
                int stock = int.Parse(TxtStock.Text.Trim()) + int.Parse(LblStock.Text.Trim());
                bool R = await evm.PutEmpaque(id, TxtNombre.Text.Trim(), TxtTamannio.Text.Trim(), stock, int.Parse(TxtStock.Text.Trim()));
                if (R)
                {
                    await DisplayAlert("Validación exitosa", "Se a modificado el Empaque exitosamente", "Ok");
                    await Navigation.PushAsync(new VistaEmpaque());
                }
                else
                {
                    await DisplayAlert("Error de validación", "No se logro modificar el Empaque", "Ok");
                }
            }
        }

        public bool ValidarEntradas()
        {
            bool R = false;
            if(TxtNombre.Text != null && !string.IsNullOrEmpty(TxtNombre.Text.Trim()) &&
                TxtTamannio.Text != null && !string.IsNullOrEmpty(TxtTamannio.Text.Trim()) &&
                TxtStock.Text != null && !string.IsNullOrEmpty(TxtStock.Text.Trim())) 
            { 
                R = true;
            }
            else
            {
                if(TxtNombre.Text == null || string.IsNullOrEmpty(TxtNombre.Text))
                {
                    DisplayAlert("Error de Validación", "Se requiere un Nombre para el Empaque", "Ok");
                    TxtNombre.Focus();
                    return false;
                }
                if (TxtTamannio.Text == null || string.IsNullOrEmpty(TxtTamannio.Text))
                {
                    DisplayAlert("Error de Validación", "Se requiere un Tamaño para el Empaque", "Ok");
                    TxtTamannio.Focus();
                    return false;
                }
                if (TxtStock.Text == null || string.IsNullOrEmpty(TxtStock.Text))
                {
                    DisplayAlert("Error de Validación", "Se requiere una cantidad en el stock para el Empaque", "Ok");
                    TxtStock.Focus();
                    return false;
                }
            }
            return R;
        }

        private void BtnMenos_Clicked(object sender, EventArgs e)
        {
            int n;
            if (TxtStock.Text == null || TxtStock.Text == "")
            {
                n = 0;
            }
            else
            {
                n = int.Parse(TxtStock.Text);
            }
            if (n < 0)
            {
                n = 0;
            }
            else if (n >= 1)
            {
                n -= 1;
            }
            TxtStock.Text = n.ToString();
            ValidarBotones();
        }

        private void BtnMas_Clicked(object sender, EventArgs e)
        {
            int n;
            if (TxtStock.Text == null || TxtStock.Text == "")
            {
                n = 0;
            }
            else
            {
                n = int.Parse(TxtStock.Text);
            };
            if (n >= 0 && n < 10000)
            {
                n += 1;
            }
            TxtStock.Text = n.ToString();
            ValidarBotones();
        }

        private void ValidarBotones()
        {
            int n;
            if (TxtStock.Text == null || TxtStock.Text == "")
            {
                n = 0;
            }
            else
            {
                n = int.Parse(TxtStock.Text);
            }
            if (n == 0)
            {
                BtnMenos.IsEnabled = false;
            }
            else if (n > 0)
            {
                BtnMenos.IsEnabled = true;
            }
            if (n >= 9999)
            {
                BtnMas.IsEnabled = false;
            }
            else if (n < 10000)
            {
                BtnMas.IsEnabled = true;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new VistaEmpaque());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}