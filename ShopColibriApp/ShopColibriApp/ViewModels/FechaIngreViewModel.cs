using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.ViewModels
{
    public class FechaIngreViewModel : BaseViewModel
    {
        FechaIngreDTO MiFechaIngreDTO { get; set; }
        public FechaIngreViewModel()
        {
            MiFechaIngreDTO = new FechaIngreDTO();

            ValidarConexionInternet();
        }

        public async Task<ObservableCollection<FechaIngreDTO>> GetFechaIngres(DateTime inicio, DateTime final, bool? seleccion, bool todo)
        {
            try
            {
                ObservableCollection<FechaIngreDTO> list = new ObservableCollection<FechaIngreDTO>();
                list = await MiFechaIngreDTO.GetFechaIngreRequisitos(inicio, final, seleccion, todo);
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
