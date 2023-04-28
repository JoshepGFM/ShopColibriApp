using Android.App;
using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Java.Util.ResourceBundle;

namespace ShopColibriApp.ViewModels
{
    public class PedidosViewModel : BaseViewModel
    {
        Pedidos MiPedidos { get; set; }
        PedidosDTO MiPedidosDTO { get; set; }
        PedidosInventario MiPedidosInve { get; set; }
        InventarioViewModel ivm { get; set; }
        public PedidosViewModel()
        {
            ValidarConexionInternet();

            MiPedidos = new Pedidos();
            MiPedidosDTO = new PedidosDTO();
            MiPedidosInve = new PedidosInventario();
            ivm = new InventarioViewModel();
        }

        public async Task<ObservableCollection<PedidosDTO>> GetPedidosBusqueda(string? Filtro)
        {
            if (IsBusy) return null;
            IsBusy = true;
            try
            {
                ObservableCollection<PedidosDTO> list = new ObservableCollection<PedidosDTO>();
                list = await MiPedidosDTO.GetPedidosBusqueda(Filtro);
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<int> GetUltiPedido()
        {
            try
            {
                List<Pedidos> list = new List<Pedidos>();
                list = await MiPedidos.GetPedidos();
                Pedidos pedido = new Pedidos();
                pedido = list.Last();
                return pedido.Codigo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> PostPedidos(DateTime pFecha, DateTime pFechaEn, ObservableCollection<PedidosCalcu> pIventario, 
                                           Usuario pUsuario, decimal pTotal)
        {
            if (IsBusy) return false;
            IsBusy= true;
            try
            {
                MiPedidos.Codigo = 0;
                MiPedidos.Fecha = pFecha;
                MiPedidos.FechaEn = pFechaEn;
                MiPedidos.UsuarioIdUsuario = pUsuario.IdUsuario;
                MiPedidos.Total = pTotal;
                bool R = await MiPedidos.PostPedidos();
                int codigo = await GetUltiPedido();
                bool T = false;
                for (int i = 0; i < pIventario.Count; i++)
                {
                    T = await PostPedidosInve(codigo, pIventario[i].Id, pIventario[i].Cantidad, 
                                              pIventario[i].Precio, pIventario[i].Total);
                    if (T)
                    {
                        int restaStock = pIventario[i].Stock - pIventario[i].Cantidad;
                        T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, restaStock, pIventario[i].Precio, 
                            pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId);
                    }

                }
                if (!T)
                {
                    await DisplayAlert("Error de validación", "Por algún motivo no se a podido realizar el vinculo con el Producto", "OK");
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

        public async Task<bool> PostPedidosInve(int pCodigo, int pIdInventario, int pCantidad,
                                                decimal pPrecio, decimal pTotal)
        {
            try
            {
                MiPedidosInve.DetalleId = 0;
                MiPedidosInve.PedidosCodigo = pCodigo;
                MiPedidosInve.InventarioId = pIdInventario;
                MiPedidosInve.Cantidad = pCantidad;
                MiPedidosInve.Precio = pPrecio;
                MiPedidosInve.Total = pTotal;

                bool R = await MiPedidosInve.PostPedidosInven();
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> PutPedidos(int pCodigo, DateTime pFecha, DateTime pFechaEn, ObservableCollection<PedidosCalcu> pIventario,
                                           Usuario pUsuario, decimal pTotal)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                MiPedidos.Codigo = pCodigo;
                MiPedidos.Fecha = pFecha;
                MiPedidos.FechaEn = pFechaEn;
                MiPedidos.UsuarioIdUsuario = pUsuario.IdUsuario;
                MiPedidos.Total = pTotal;
                bool R = await MiPedidos.PutPedidos();
                List<PedidosInventario> pedido = new List<PedidosInventario>();
                pedido = await MiPedidosInve.GetBuscarPedido(pCodigo);
                bool T = false;
                if (pedido.Count > 0)
                {
                    for (int i = 0; i < pedido.Count; i++)
                    {
                        if (i < pIventario.Count)
                        {
                            int diferCantid = 0;
                            if (pedido[i].InventarioId == pIventario[i].Id)
                            {
                                if (pedido[i].Cantidad > pIventario[i].Cantidad)
                                {
                                    diferCantid = pedido[i].Cantidad - pIventario[i].Cantidad;
                                    int sumaStock = pIventario[i].Stock + diferCantid;
                                    T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, sumaStock, pIventario[i].Precio,
                                pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId);
                                }
                                else if (pedido[i].Cantidad < pIventario[i].Cantidad)
                                {
                                    diferCantid = pIventario[i].Cantidad - pedido[i].Cantidad;
                                    int restaStock = pIventario[i].Stock - diferCantid;
                                    T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, restaStock, pIventario[i].Precio,
                                pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId);
                                }
                            }
                            else
                            {
                                for(int e  = 0; e < pedido.Count; e++)
                                {
                                    if (pedido[e].InventarioId == pIventario[i].Id)
                                    {
                                        if (pedido[e].Cantidad > pIventario[i].Cantidad)
                                        {
                                            diferCantid = pedido[i].Cantidad - pIventario[i].Cantidad;
                                            int sumaStock = pIventario[i].Stock + diferCantid;
                                            T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, sumaStock, pIventario[i].Precio,
                                        pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId);
                                        }
                                        else if (pedido[e].Cantidad < pIventario[i].Cantidad)
                                        {
                                            diferCantid = pIventario[i].Cantidad - pedido[i].Cantidad;
                                            int restaStock = pIventario[i].Stock - diferCantid;
                                            T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, restaStock, pIventario[i].Precio,
                                        pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId);
                                        }
                                    }
                                }
                            }
                            T = await PutPedidosInve(pedido[i].DetalleId, pCodigo, pIventario[i].Id, 
                                pIventario[i].Cantidad, pIventario[i].PrecioUn, pIventario[i].Total, pIventario[i].Fecha);
                        }
                        else if (pedido.Count >= pIventario.Count)
                        {
                            T = await MiPedidosInve.DeletePedidosInventario(pedido[i].DetalleId);
                            bool J = false;
                            for (int e = 0; e < pedido.Count; e++)
                            {
                                if (pIventario[i].Id != pedido[e].InventarioId)
                                {
                                    J = true;
                                }
                                if (pIventario[i].Id == pedido[e].InventarioId)
                                {
                                    J = false;
                                    break;
                                }
                            }
                                if (T && J )
                                {
                                    int suma = pedido[i].Cantidad + pIventario[i].Stock;
                                    T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, suma, pIventario[i].Precio,
                                pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId);
                                }
                        }
                    }
                    if (pedido.Count < pIventario.Count)
                    {
                        for (int i = pedido.Count; i < pIventario.Count; i++)
                        {
                            T = await PostPedidosInve(pCodigo, pIventario[i].Id,
                                pIventario[i].Cantidad, pIventario[i].PrecioUn, pIventario[i].Total);
                            if (T)
                            {
                                int restaStock = pIventario[i].Stock - pIventario[i].Cantidad;
                                T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, restaStock, pIventario[i].Precio,
                                    pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < pIventario.Count; i++)
                    {
                        T = await PostPedidosInve(pCodigo, pIventario[i].Id,
                                pIventario[i].Cantidad, pIventario[i].PrecioUn, pIventario[i].Total);
                        if (T)
                        {
                            int restaStock = pIventario[i].Stock - pIventario[i].Cantidad;
                            T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, restaStock, pIventario[i].Precio,
                                pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId);
                        }
                    }
                }
                if (!T)
                {
                    await DisplayAlert("Error de validación", "Por algún motivo no se a podido realizar el vinculo con el o los Productos", "OK");
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

        public async Task<bool> PutPedidosInve(int pId, int pCodigo, int pIdInventario, int pCantidad,
                                                decimal pPrecio, decimal pTotal, DateTime pFecha)
        {
            try
            {
                MiPedidosInve.DetalleId = pId;
                MiPedidosInve.PedidosCodigo = pCodigo;
                MiPedidosInve.InventarioId = pIdInventario;
                MiPedidosInve.Cantidad = pCantidad;
                MiPedidosInve.Precio = pPrecio;
                MiPedidosInve.Total = pTotal;
                MiPedidosInve.Fecha = pFecha;

                bool R = await MiPedidosInve.PutPedidosInven();
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> DeletePedido(int pId)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                List<PedidosInventario> list = new List<PedidosInventario>();
                list = await MiPedidosInve.GetBuscarPedido(pId);
                bool T = false;
                for (int i = 0; i < list.Count; i++)
                {
                    T = await MiPedidosInve.DeletePedidosInventario(list[i].DetalleId);
                }
                if (!T)
                {
                    await DisplayAlert("Error de validación", "Por algún motivo no se a podido eliminar el vinculo con el Pedido", "OK");
                }
                bool R = await MiPedidos.DeletePedido(pId);
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
