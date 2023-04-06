using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async Task<List<Empaque>> GetEmpaque()
        {
            try
            {
                List<Empaque> empaque = new List<Empaque>();

                empaque = await MiEmpaque.GetEmpaque();

                if (empaque == null)
                {
                    return null;
                }
                else
                {
                    return empaque;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
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

        public async Task<ObservableCollection<Empaque>> GetBuscarEmpaque(string? Filtro)
        {
            if (IsBusy)
            {
                return null;
            }
            else
            {
                IsBusy = true;
                try
                {
                    ObservableCollection<Empaque> list = new ObservableCollection<Empaque>();
                    list = await MiEmpaque.GetBuscarEmpaque(Filtro);

                    if (list == null)
                    {
                        return null;
                    }
                    return list;
                }
                catch (Exception)
                {
                    return null;

                }
                finally { IsBusy = false; }
            }

        }

        public async Task<bool> PutEmpaque(int pId, string pNombre, string pTamannio, int pStock)
        {
            bool R = false;
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                Empaque empaque = new Empaque();

                empaque.Id = pId;
                empaque.Nombre = pNombre;
                empaque.Tamannio = pTamannio;
                empaque.Stock = pStock;

                R = await MiEmpaque.PutEmpaque();
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<bool> DeleteEmpaque(int id)
        {
            if(IsBusy) return false;
            IsBusy = true;
            try
            {
                MiEmpaque.Id = id;
                bool R = await MiEmpaque.DeleteEmpaque();
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
