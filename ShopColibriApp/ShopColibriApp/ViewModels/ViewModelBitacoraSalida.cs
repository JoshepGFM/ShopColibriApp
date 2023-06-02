using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.ViewModels
{
    class ViewModelBitacoraSalida : BaseViewModel
    {
        public BitacoraSalida MiBitacora { get; set; }
        public ViewModelBitacoraSalida()
        {
            MiBitacora = new BitacoraSalida();
            ValidarConexionInternet();
        }

        public async Task<ObservableCollection<BitacoraSalida>> GetBitacoraSalidas(DateTime inicio, DateTime final, string? producto, bool Todo)
        {
            try
            {
                ObservableCollection<BitacoraSalida> list = new ObservableCollection<BitacoraSalida>();
                list = await MiBitacora.GetBitacoraSalida(inicio, final, producto, Todo);
                return list;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> PostBitacoraSalidas(DateTime fecha, string objetoRef, int salida)
        {
            if(IsBusy) return false;
            IsBusy = true;
            try
            {
                MiBitacora.Id = 0;
                MiBitacora.Fecha = fecha;
                MiBitacora.Salida = salida;
                MiBitacora.ObjetoRef = objetoRef;

                bool R = await MiBitacora.PostBitacoraSalidas();

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
