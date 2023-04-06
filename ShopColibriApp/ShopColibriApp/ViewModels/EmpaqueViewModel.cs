using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.ViewModels
{
    public class EmpaqueViewModel : BaseViewModel
    {
        Empaque MiEmpaque { get; set; }
        public EmpaqueViewModel()
        {
            ValidarConexionInternet();
            MiEmpaque = new Empaque();
        }

        public async Task<bool> PostEmpaque(string pNombre, string pTamannio, int pStock)
        {
            bool R = false;
            if(IsBusy) return false;
            IsBusy = true;
            try
            {
                Empaque empaque = new Empaque();

                empaque.Nombre = pNombre;
                empaque.Tamannio = pTamannio;
                empaque.Stock = pStock;

                R = await MiEmpaque.PostEmpaque();
                return R;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }
    }
}
