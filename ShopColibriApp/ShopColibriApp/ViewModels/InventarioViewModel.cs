using Android.Media.Midi;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ShopColibriApp.Servicios;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShopColibriApp.ViewModels
{
    public class InventarioViewModel : BaseViewModel
    {
        string[] Scopes = { DriveService.Scope.Drive };
        string AplicationName = "ShopColibriApp";
        Inventario MiInventario { get; set; }
        InventarioDTO MiInventarioDTO { get; set; }
        FechaIngre MiFechaIngre { get; set; }
        UsuarioFechaIngre MiUsuFechaIngre { get; set; }
        EmpaqueViewModel evm { get; set; }
        public InventarioViewModel()
        {
            MiInventario = new Inventario();
            MiInventarioDTO = new InventarioDTO();
            MiFechaIngre = new FechaIngre();
            MiUsuFechaIngre = new UsuarioFechaIngre();
        }

        public async Task<int> GetUltimoInventario()
        {
            try
            {
                List<Inventario> list = new List<Inventario>();
                list = await MiInventario.GetInventario();
                Inventario inventario = new Inventario();
                inventario = list.Last();
                return inventario.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> PostInventario(DateTime pFecha,
                                               int pStock,
                                               decimal pPrecio,
                                               string pOrigen,
                                               int pProducto,
                                               int pEmpaque)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                MiInventario.Fecha = pFecha;
                MiInventario.Stock = pStock;
                MiInventario.PrecioUn = pPrecio;
                MiInventario.Origen = pOrigen;
                MiInventario.ProductoCodigo = pProducto;
                MiInventario.EmpaqueId = pEmpaque;

                bool R = await MiInventario.PostInventario();
                if (R)
                {
                    MiFechaIngre.Id = 0;
                    MiFechaIngre.Fecha = DateTime.Now;
                    MiFechaIngre.Entrada = pStock;
                    MiFechaIngre.InventarioId = await GetUltimoInventario();
                    bool T = await MiFechaIngre.PostFechaIngre();

                    MiUsuFechaIngre.DetalleId = 0;
                    MiUsuFechaIngre.Fecha = DateTime.Now;
                    MiUsuFechaIngre.UsuarioIdUsuario = GlobalObject.GloUsu.IdUsuario;
                    MiUsuFechaIngre.FechaIngreId = await GetUltiFechaIngre();
                    bool U = await MiUsuFechaIngre.PostUsuarioFechaIngre();

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
                return false;
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<bool> PutInventario(int pId, 
                                              DateTime pFecha,
                                              int pStock,
                                              decimal pPrecio,
                                              string pOrigen,
                                              int pProducto,
                                              int pEmpaque,
                                              int pEntradaSt)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                MiInventario.Id = pId;
                MiInventario.Fecha = pFecha;
                MiInventario.Stock = pStock;
                MiInventario.PrecioUn = pPrecio;
                MiInventario.Origen = pOrigen;
                MiInventario.ProductoCodigo = pProducto;
                MiInventario.EmpaqueId = pEmpaque;

                bool R = await MiInventario.PutInventario();
                if (R && pStock > 0)
                {
                    MiFechaIngre.Id = 0;
                    MiFechaIngre.InventarioId = pId;
                    MiFechaIngre.Entrada = pEntradaSt;
                    MiFechaIngre.Fecha = DateTime.Now;

                    bool T = await MiFechaIngre.PostFechaIngre();

                    MiUsuFechaIngre.DetalleId = 0;
                    MiUsuFechaIngre.Fecha = DateTime.Now;
                    MiUsuFechaIngre.UsuarioIdUsuario = GlobalObject.GloUsu.IdUsuario;
                    MiUsuFechaIngre.FechaIngreId = await GetUltiFechaIngre();
                    bool U = await MiUsuFechaIngre.PostUsuarioFechaIngre();

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
                return false;
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<ObservableCollection<InventarioDTO>> GetInveBuscar(string? Filtro, bool? estado)
        {
            if (IsBusy) return null;
            IsBusy = true;
            try
            {
                ObservableCollection<InventarioDTO> list = new ObservableCollection<InventarioDTO>();
                list = await MiInventarioDTO.GetBuscarInventario(Filtro, estado);

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

        public async Task<bool> DeleteInventario( int pId )
        {
            if(IsBusy) return false;
            IsBusy=true;
            try
            {
                bool R = await MiInventario.DeleteInventario(pId);

                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
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
