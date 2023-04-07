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

namespace ShopColibriApp.ViewModels
{
    public class InventarioViewModel : BaseViewModel
    {
        string[] Scopes = { DriveService.Scope.Drive };
        string AplicationName = "ShopColibriApp";
        Inventario MiInventario { get; set; }
        Servicios.Drive MiDrive { get; set; }
        public InventarioViewModel()
        {
            MiInventario = new Inventario();
            MiDrive = new Servicios.Drive();
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
                Inventario inventario = new Inventario();

                inventario.Fecha = pFecha;
                inventario.Stock = pStock;
                inventario.PrecioUn = pPrecio;
                inventario.Origen = pOrigen;
                inventario.ProductoCodigo = pProducto;
                inventario.EmpaqueId = pEmpaque;

                bool R = await MiInventario.PostInventario();
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

        public void VerificarAccesoDrive()
        {
            MiDrive.GetCredentials();
        }
    }
}
