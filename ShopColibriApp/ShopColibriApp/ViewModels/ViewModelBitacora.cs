using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.ViewModels
{
    public class ViewModelBitacora : BaseViewModel
    {
        Bitacora MiBitacora { get; set; }
        public ViewModelBitacora()
        {
            MiBitacora = new Bitacora();

            ValidarConexionInternet();
        }

        public async Task<ObservableCollection<Bitacora>> GetBitacora(DateTime inicio, DateTime final, string? producto, bool Todo)
        {
            try
            {
                ObservableCollection<Bitacora> list = new ObservableCollection<Bitacora>();

                list = await MiBitacora.GetBitacora(inicio, final, producto, Todo);

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> PostBitacora( DateTime fecha, string descripcion)
        {
            if(IsBusy) return false;
            IsBusy = true;
            try
            {
                MiBitacora.Id = 0;
                MiBitacora.Descripcion = descripcion;
                MiBitacora.Fecha = fecha;
                bool R = await MiBitacora.PostBitacora();
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
