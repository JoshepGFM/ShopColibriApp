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
        public EmpaquePage()
        {
            InitializeComponent();
        }

        private void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            if (ValidarEntradas())
            {

            }
        }

        private void BtnModificar_Clicked(object sender, EventArgs e)
        {

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
                if(TxtNombre.Text != null || string.IsNullOrEmpty(TxtNombre.Text.Trim()))
                {
                    DisplayAlert("Error de Validación", "Se requiere un Nombre para el Empaque", "Ok");
                    TxtNombre.Focus();
                    return false;
                }
                if (TxtTamannio.Text != null || string.IsNullOrEmpty(TxtTamannio.Text.Trim()))
                {
                    DisplayAlert("Error de Validación", "Se requiere un Tamaño para el Empaque", "Ok");
                    TxtTamannio.Focus();
                    return false;
                }
                if (TxtStock.Text != null || string.IsNullOrEmpty(TxtStock.Text.Trim()))
                {
                    DisplayAlert("Error de Validación", "Se requiere una cantidad en el stock para el Empaque", "Ok");
                    TxtStock.Focus();
                    return false;
                }
            }
            return R;
        }
    }
}