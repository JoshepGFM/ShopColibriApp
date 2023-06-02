using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.ViewModels
{
    public class EmpaqueViewModel : BaseViewModel
    {
        Empaque MiEmpaque { get; set; }
        FechaIngre MiFechaIngre { get; set; }
        UsuarioFechaIngre MiUsuFechIngre { get; set; }
        public EmpaqueViewModel()
        {
            ValidarConexionInternet();
            MiEmpaque = new Empaque();
            MiFechaIngre = new FechaIngre();
            MiUsuFechIngre = new UsuarioFechaIngre();
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
                MiEmpaque.Nombre = pNombre;
                MiEmpaque.Tamannio = pTamannio;
                MiEmpaque.Stock = pStock;

                R = await MiEmpaque.PostEmpaque();
                if (R)
                {
                    MiFechaIngre.Id = 0;
                    MiFechaIngre.Fecha = DateTime.Now;
                    MiFechaIngre.Entrada = pStock;
                    MiFechaIngre.EmpaqueId = await GetUltiEmpaque();
                    bool T = await MiFechaIngre.PostFechaIngre();

                    MiUsuFechIngre.DetalleId = 0;
                    MiUsuFechIngre.Fecha = DateTime.Now;
                    MiUsuFechIngre.UsuarioIdUsuario = GlobalObject.GloUsu.IdUsuario;
                    MiUsuFechIngre.FechaIngreId = await GetUltiFechaIngre();
                    bool U = await MiUsuFechIngre.PostUsuarioFechaIngre();

                    if (!T && !U)
                    {
                        await DisplayAlert("Error de validación", "No se a crear la fecha de ingreso de empaques", "OK");
                    }
                }
                return R;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<ObservableCollection<Empaque>> GetBuscarEmpaque(string? Filtro, bool stock)
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
                    list = await MiEmpaque.GetBuscarEmpaque(Filtro,stock);

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

        public async Task<bool> PutEmpaque(int pId, string pNombre, string pTamannio, int pStock, int pEntradaSt)
        {
            bool R = false;
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                MiEmpaque.Id = pId;
                MiEmpaque.Nombre = pNombre;
                MiEmpaque.Tamannio = pTamannio;
                MiEmpaque.Stock = pStock;

                R = await MiEmpaque.PutEmpaque();

                if (R && pStock > 0)
                {
                    MiFechaIngre.Id = 0;
                    MiFechaIngre.EmpaqueId = pId;
                    MiFechaIngre.Entrada = pEntradaSt;
                    MiFechaIngre.Fecha = DateTime.Now;

                    bool T = await MiFechaIngre.PostFechaIngre();

                    MiUsuFechIngre.DetalleId = 0;
                    MiUsuFechIngre.Fecha = DateTime.Now;
                    MiUsuFechIngre.UsuarioIdUsuario = GlobalObject.GloUsu.IdUsuario;
                    MiUsuFechIngre.FechaIngreId = await GetUltiFechaIngre();
                    bool U = await MiUsuFechIngre.PostUsuarioFechaIngre();

                    if (!T && !U)
                    {
                        await DisplayAlert("Error de validación", "No se a crear la fecha de ingreso de empaques", "OK");
                    }
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

        public async Task<bool> DeleteEmpaque(int id)
        {
            if(IsBusy) return false;
            IsBusy = true;
            try
            {
                MiEmpaque.Id = id;
                bool U = await MiUsuFechIngre.DeleteUsuFechaIngre(id);
                bool T = await MiFechaIngre.DeleteFechaIngre(id);
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

        public async Task<int> GetUltiEmpaque()
        {
            try
            {
                List<Empaque> list = new List<Empaque>();
                list = await MiEmpaque.GetEmpaque();
                Empaque empaque = new Empaque();
                empaque = list.Last();
                return empaque.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<int> GetUltiFechaIngre()
        {
            try
            {
                List<FechaIngre> list = new List<FechaIngre>();
                list = await MiFechaIngre.GetFechaIngre();
                FechaIngre item = new FechaIngre();
                item = list.Last();
                return item.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

    }
}
