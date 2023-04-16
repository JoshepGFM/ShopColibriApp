using Org.Apache.Http.Conn.Routing;
using ShopColibriApp.Models;
using ShopColibriApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.Servicios
{
    public class ControlMarmitaViewModel : BaseViewModel
    {
        ControlMarmita MiControlMar { get; set; }
        public ControlMarmitaViewModel()
        {
            ValidarConexionInternet();
            MiControlMar = new ControlMarmita();
        }

        public async Task<ObservableCollection<ControlMarmita>> GetControlMarmi(string? Filter)
        {
            if (IsBusy) return null;
            IsBusy = true;
            try
            {
                ObservableCollection<ControlMarmita> controlMaris = new ObservableCollection<ControlMarmita>();

                return controlMaris;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
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
                for(int i = 0; i < usuarios.Count; i++)
                {
                    MiControlMar.idUsuario.Add(usuarios[i].IdUsuario);
                }

                bool R = await MiControlMar.PostControlMar();
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
