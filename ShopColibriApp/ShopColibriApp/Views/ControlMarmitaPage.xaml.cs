using Java.Nio.Channels;
using Org.W3c.Dom.LS;
using ShopColibriApp.Models;
using ShopColibriApp.Servicios;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlMarmitaPage : ContentPage
    {
        UsuarioViewModel uvm { get; set; }
        ControlMarmitaViewModel Cmvm { get; set; }
        UsuarioControlMarmitum ucmvm { get; set; }
        ViewModelBitacora vmb { get; set; }
        private Usuario usuario { get; set; }
        public ControlMarmitaPage()
        {
            InitializeComponent();

            vmb = new ViewModelBitacora();
            BindingContext = uvm = new UsuarioViewModel();
            BindingContext = Cmvm = new ControlMarmitaViewModel();

            CargarInfo();
            CargarListaUsuarios();
        }

        private async void CargarListaUsuarios()
        {
            ObservableCollection<Usuario> list = new ObservableCollection<Usuario>();
            if (GlobalObject.GloListUsu != null)
            {
                list = GlobalObject.GloListUsu;
            }
            LvlListaUsuarios.ItemsSource = list;
        }

        private bool ValidarEntradas()
        {
            bool R = false;
            if (PckFecha.Date != null &&
                TmHoraEn.Time != null &&
                TmHoraAp.Time != null &&
                TxtTemperatura.Text != null && !string.IsNullOrEmpty(TxtTemperatura.Text.Trim()) &&
                TxtIntensidaMov.Text != null && !string.IsNullOrEmpty(TxtIntensidaMov.Text.Trim()) &&
                TxtLote.Text != null && !string.IsNullOrEmpty(TxtLote.Text.Trim()) &&
                GlobalObject.GloListUsu.Count != 0)
            {
                R = true;
            }
            else
            {
                if(PckFecha.Date == null)
                {
                    PckFecha.Focus();
                    DisplayAlert("Error de Validación", "Se requiere de una Fecha", "Ok");
                    return false;
                }
                if (TmHoraEn.Time == null)
                {
                    TmHoraEn.Focus();
                    DisplayAlert("Error de Validación", "Se requiere de una hora de Encendido", "Ok");
                    return false;
                }
                if(TmHoraAp.Time == null)
                {
                    TmHoraAp.Focus();
                    DisplayAlert("Error de Validación", "Se requiere de una hora de Apagado", "Ok");
                    return false;
                }
                if (TxtTemperatura.Text == null)
                {
                    DisplayAlert("Error de Validación", "Se requiere el ingreso de una Temperatura", "Ok");
                    TxtTemperatura.Focus();
                    return false;
                }
                if (TxtIntensidaMov.Text == null)
                {
                    DisplayAlert("Error de Validación", "Se requiere de una intensidad Movimiento", "Ok");
                    TxtIntensidaMov.Focus();
                    return false;
                }
                if (TxtLote.Text == null)
                {
                    DisplayAlert("Error de Validación", "Se requiere de un Lote", "Ok");
                    TxtLote.Focus();
                    return false;
                }
                if (GlobalObject.GloListUsu.Count == 0)
                {
                    DisplayAlert("Error de Validación", "Se requiere del ingreso de un Usuario", "Ok");
                    return false;
                }
            }
            return R;
        }
        //Lista los usuarios seleccionados de la vista de seleccion
        private void LvlListaUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Usuario usuarioSel = e.SelectedItem as Usuario;
            usuario = usuarioSel;
            FmEliminar.IsVisible = true;
        }

        private void CargarInfo()
        {
            if(GlobalObject.GloControlMarDTO.Codigo > 0)
            {
                PckFecha.Date = GlobalObject.GloControlMarDTO.Fecha;
                TmHoraEn.Time = GlobalObject.GloControlMarDTO.HoraEn;
                TmHoraAp.Time = GlobalObject.GloControlMarDTO.HoraAp;
                TxtTemperatura.Text = GlobalObject.GloControlMarDTO.Temperatura.ToString();
                TxtIntensidaMov.Text = GlobalObject.GloControlMarDTO.IntensidadMov;
                TxtLote.Text = GlobalObject.GloControlMarDTO.Lote;
                List<Usuario> list = new List<Usuario>();
                list = GlobalObject.GloControlMarDTO.Usuarios;
                ObservableCollection<Usuario> usuarios = new ObservableCollection<Usuario>();
                foreach (var item in list)
                {
                    Usuario NewItem = new Usuario();

                    NewItem.IdUsuario = item.IdUsuario;
                    NewItem.Nombre = item.Nombre;
                    NewItem.Apellido1 = item.Apellido1;
                    NewItem.Apellido2 = item.Apellido2;
                    NewItem.Email = item.Email;
                    NewItem.EmailResp = item.EmailResp;
                    NewItem.Telefono = item.Telefono;
                    NewItem.TusuarioId = item.TusuarioId;

                    usuarios.Add(NewItem);
                }
                if (usuarios.Count > 0)
                {
                    if (GlobalObject.GloListUsu.Count == 0)
                    {
                        GlobalObject.GloListUsu = usuarios;
                    }
                }
                BtnGuardar.IsVisible = false;
                TituloControlMarmita.Text = "Modificar Control de Marmita";
                BtnModificar.IsVisible = true;
            }
            else
            {
                PckFecha.Date = GlobalObject.GloControMarmi_Cont.Fecha;
                TmHoraEn.Time = GlobalObject.GloControMarmi_Cont.HoraEn;
                TmHoraAp.Time = GlobalObject.GloControMarmi_Cont.HoraAp;
                TxtTemperatura.Text = GlobalObject.GloControMarmi_Cont.Temperatura.ToString();
                TxtIntensidaMov.Text = GlobalObject.GloControMarmi_Cont.IntensidadMov;
                TxtLote.Text = GlobalObject.GloControMarmi_Cont.Lote;
                BtnGuardar.IsVisible = true;
                BtnModificar.IsVisible = false;
            }
        }
        //refresca el listado de los usuarios seleccionados
        private void LvlListaUsuarios_Refreshing(object sender, EventArgs e)
        {
            CargarListaUsuarios();
            LvlListaUsuarios.IsRefreshing = false;
            FmEliminar.IsVisible = false;
        }

        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            if(ValidarEntradas())
            {
                bool R = false;
                List<Usuario> list = new List<Usuario>();
                if (GlobalObject.GloListUsu.Count != 0)
                {
                    list = GlobalObject.GloListUsu.ToList();
                }
                R = await Cmvm.PostControlMar(PckFecha.Date, TmHoraEn.Time, TmHoraAp.Time, int.Parse(TxtTemperatura.Text.Trim()), TxtIntensidaMov.Text.Trim(), TxtLote.Text.Trim(), list);
                int codigo = await Cmvm.GetUltimoID();
                
                if (R)
                {
                    await DisplayAlert("Validación exitosa", "Se a registrado el control de Marmita con éxito", "OK");
                    await vmb.PostBitacora(DateTime.Now, GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2 +
                            " Guardo un Control de Marmita. control: Cod." + codigo + " del lote" + TxtLote.Text.ToString());
                    await Navigation.PushAsync(new VistaControlMarmita());
                }
                else
                {
                    await DisplayAlert("Error de Validación", "No se a podido realizar el ingreso del control.", "Ok");
                }
            }
        }

        private async void BtnagregarUsu_Clicked(object sender, EventArgs e)
        {
            
            GlobalObject.GloControMarmi_Cont.Fecha = PckFecha.Date;
            GlobalObject.GloControMarmi_Cont.HoraEn = TmHoraEn.Time;
            GlobalObject.GloControMarmi_Cont.HoraAp = TmHoraAp.Time;
            if (TxtTemperatura.Text == null)
            {
                GlobalObject.GloControMarmi_Cont.Temperatura = 0;
            }
            else
            {
                GlobalObject.GloControMarmi_Cont.Temperatura = int.Parse(TxtTemperatura.Text.Trim());
            }
            if (TxtIntensidaMov.Text == null)
            {
                GlobalObject.GloControMarmi_Cont.IntensidadMov = null;
            }
            else
            {
                GlobalObject.GloControMarmi_Cont.IntensidadMov = TxtIntensidaMov.Text;
            }
            if (TxtLote.Text == null)
            {
                GlobalObject.GloControMarmi_Cont.Lote = null;
            }
            else
            {
                GlobalObject.GloControMarmi_Cont.Lote = TxtLote.Text;
            }

            await Navigation.PushAsync(new SeleccionUsuario());
        }

        private async void BtnModificar_Clicked(object sender, EventArgs e)
        {
            if (ValidarEntradas())
            {
                bool R = false;
                List<Usuario> list = new List<Usuario>();
                if (GlobalObject.GloListUsu.Count != 0)
                {
                    list = GlobalObject.GloListUsu.ToList();
                }
                int codigo = GlobalObject.GloControlMarDTO.Codigo;
                R = await Cmvm.PutControlMar(codigo, PckFecha.Date, TmHoraEn.Time, TmHoraAp.Time, int.Parse(TxtTemperatura.Text.Trim()), TxtIntensidaMov.Text.Trim(), TxtLote.Text.Trim(), list);

                if (R)
                {
                    await DisplayAlert("Validación exitosa", "Se a modificado el control de Marmita con éxito", "OK");
                    await vmb.PostBitacora(DateTime.Now, GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2 +
                            " Modifico un Control de Marmita. control: Cod." + codigo + " del lote" + TxtLote.Text.ToString());
                    GlobalObject.GloListInven.Clear();
                    GlobalObject.GloPedidos = new Models.Pedidos();
                    GlobalObject.GloPedidos_Cont = new Models.Pedidos();
                    GlobalObject.GloPedidosDTO = new Models.PedidosDTO();
                    GlobalObject.GloUsuPedi = new Usuario();
                    await Navigation.PushAsync(new VistaControlMarmita());
                }
                else
                {
                    await DisplayAlert("Error de Validación", "No se a podido realizar la modificación del control.", "Ok");
                }
            }
        }
        //Elimina de lista de usuarios seleccionados un usuario
        private void BtnEliminarUsu_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloListUsu.Remove(usuario);
            CargarListaUsuarios();
            FmEliminar.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            GlobalObject.GloListInven.Clear();
            GlobalObject.GloPedidos = new Models.Pedidos();
            GlobalObject.GloPedidos_Cont = new Models.Pedidos();
            GlobalObject.GloPedidosDTO = new Models.PedidosDTO();
            GlobalObject.GloUsuPedi = new Usuario();
            Navigation.PushAsync(new VistaControlMarmita());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}