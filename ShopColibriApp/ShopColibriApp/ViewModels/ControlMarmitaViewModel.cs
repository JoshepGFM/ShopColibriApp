using Org.Apache.Http.Conn.Routing;
using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.Servicios
{
    public class ControlMarmitaViewModel : BaseViewModel
    {
        ControlMarmita MiControlMar { get; set; }
        ControlMarmitaDTO MiControlMarDTO { get; set; }
        UsuarioControlMarmitum MiUsControl { get; set; }
        public ControlMarmitaViewModel()
        {
            ValidarConexionInternet();
            MiControlMar = new ControlMarmita();
            MiUsControl = new UsuarioControlMarmitum();
            MiControlMarDTO = new ControlMarmitaDTO();
        }
        public async Task<ObservableCollection<ControlMarmitaDTO>> GetControlMarmiBuscar(string? Filter)
        {
            if (IsBusy) return null;
            IsBusy = true;
            try
            {
                ObservableCollection<ControlMarmitaDTO> controlMaris = new ObservableCollection<ControlMarmitaDTO>();

                controlMaris = await MiControlMarDTO.GetControlMarmitumsBuscar(Filter);

                return controlMaris;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<int> GetUltimoID()
        {
            try
            {
                List<ControlMarmita> list = new List<ControlMarmita>();
                list = await MiControlMar.GetControlMarmitums();
                ControlMarmita controlMarmita = new ControlMarmita();
                controlMarmita = list.Last();
                return controlMarmita.Codigo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<ControlMarmita> GetControlMarmiId(int id)
        {
            try
            {
                ControlMarmita controlMaris = new ControlMarmita();

                controlMaris = await MiControlMar.GetControlMarmitumsId(id);

                return controlMaris;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> PostControlMar(DateTime pFecha, TimeSpan pHoraEn, TimeSpan pHoraAp, int pTemparatura, string pIntenciadaMov, string pLote, List<Usuario> usuarios)
        {
            if(IsBusy) return false;
            IsBusy = true;
            try
            {
                MiControlMar.Fecha = pFecha;
                MiControlMar.HoraAp = pHoraAp;
                MiControlMar.HoraEn = pHoraEn;
                MiControlMar.Temperatura = pTemparatura;
                MiControlMar.IntensidadMov = pIntenciadaMov;
                MiControlMar.Lote = pLote;
                bool R = await MiControlMar.PostControlMar();
                bool T = false;
                int id = await GetUltimoID();
                for(int i = 0; i < usuarios.Count; i++)
                {
                    T = await PostUsControlMar(id, usuarios[i].IdUsuario);
                }
                if(!T)
                {
                    await DisplayAlert("Error de Validación", "Por algun motivo no se pido vincular con el o los Usuarios", "Ok");
                }
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }
        
        public async Task<bool> PostUsControlMar(int Codigo, int id)
        {
            try
            {
                MiUsControl.DetalleId = 0;
                MiUsControl.ControlMarmitaCodigo = Codigo;
                MiUsControl.UsuarioIdUsuario = id;

                bool R = await MiUsControl.PostUsuarioControlMarmitum();
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> PutControlMar(int pCodigo,DateTime pFecha, TimeSpan pHoraEn, TimeSpan pHoraAp, int pTemparatura, string pIntenciadaMov, string pLote, List<Usuario> usuarios)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                MiControlMar.Codigo = pCodigo;
                MiControlMar.Fecha = pFecha;
                MiControlMar.HoraAp = pHoraAp;
                MiControlMar.HoraEn = pHoraEn;
                MiControlMar.Temperatura = pTemparatura;
                MiControlMar.IntensidadMov = pIntenciadaMov;
                MiControlMar.Lote = pLote;
                bool R = await MiControlMar.PutControlMar();
                List<UsuarioControlMarmitum> control = new List<UsuarioControlMarmitum>();
                control = await MiUsControl.GetUsuarioControlMarmitumBusqueda(pCodigo);
                bool T = false;
                if (control.Count > 0)
                {
                    for (int i = 0; i < control.Count; i++)
                    {
                        if (i < usuarios.Count)
                        {
                            T = await PutUsControlMar(control[i].DetalleId, pCodigo, usuarios[i].IdUsuario, control[i].Fecha);
                        }
                        else if ( control.Count >= usuarios.Count)
                        {
                            T = await MiUsControl.DeleteUsuContMar(control[i].DetalleId);
                        }
                    }
                    if (control.Count < usuarios.Count)
                    {
                        for(int i = control.Count; i < usuarios.Count; i++)
                        {
                            T = await PostUsControlMar(pCodigo, usuarios[i].IdUsuario);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < usuarios.Count; i++)
                    {
                        T = await PostUsControlMar(pCodigo, usuarios[i].IdUsuario);
                    }
                }
                return R & T;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<bool> PutUsControlMar(int idControl, int Codigo, int id, DateTime pFecha)
        {
            try
            {
                MiUsControl.DetalleId = idControl;
                MiUsControl.ControlMarmitaCodigo = Codigo;
                MiUsControl.UsuarioIdUsuario = id;
                MiUsControl.Fecha = pFecha;

                bool R = await MiUsControl.PutUsuarioControlMarmitum();
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<bool> DeleteControlMarmita(int id)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                List<UsuarioControlMarmitum> usuarioControlMarmitum = new List<UsuarioControlMarmitum>();
                usuarioControlMarmitum = await MiUsControl.GetUsuarioControlMarmitumBusqueda(id);
                bool T = false;
                if ( usuarioControlMarmitum.Count > 0)
                {
                    for (int i = 0;i < usuarioControlMarmitum.Count;i++)
                    {
                        T = await MiUsControl.DeleteUsuContMar(usuarioControlMarmitum[i].DetalleId);
                    }
                }
                if (!T)
                {
                    await DisplayAlert("Error de Validación", "Por algún motivo no se a podido eliminar el o los Usuarios vinculados", "Ok");
                }
                bool R = await MiControlMar.DeleteControlMar(id);
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }
    }
}
