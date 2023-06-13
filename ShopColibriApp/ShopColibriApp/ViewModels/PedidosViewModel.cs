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
        Empaque MiEmpaque { get; set; }
        Producto MiProducto { get; set; }
        PedidosDTO MiPedidosDTO { get; set; }
        PedidosInventario MiPedidosInve { get; set; }
        Inventario MiInventario { get; set; }
        InventarioViewModel ivm { get; set; }
        ViewModelBitacoraSalida vmb { get; set; }
        public PedidosViewModel()
        {
            ValidarConexionInternet();
            MiEmpaque = new Empaque();
            MiProducto = new Producto();
            MiInventario = new Inventario();
            MiPedidos = new Pedidos();
            MiPedidosDTO = new PedidosDTO();
            MiPedidosInve = new PedidosInventario();
            ivm = new InventarioViewModel();
            vmb = new ViewModelBitacoraSalida();
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

        public async Task<Pedidos> GetPedidoId(int id)
        {
            try
            {
                Pedidos pedido = new Pedidos();
                pedido = await MiPedidos.GetPedidosId(id);
                return pedido;
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
                            pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId, -pIventario[i].Cantidad);
                    }
                    string usu = GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2;
                    string objetoRef = usu + " realizo un pedido de " + pIventario[i].Cantidad + " '" + pIventario[i].NombrePro + " " + pIventario[i].NombreEmp + "'. En el pedido Cod." + codigo;
                    bool U = await vmb.PostBitacoraSalidas(DateTime.Now, objetoRef, pIventario[i].Cantidad);
                    if (!U)
                    {
                        await DisplayAlert("Error de validación", "Error de Bitácora de Salida", "OK");
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
                                pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId, diferCantid);
                                }
                                else if (pedido[i].Cantidad < pIventario[i].Cantidad)
                                {
                                    diferCantid = pIventario[i].Cantidad - pedido[i].Cantidad;
                                    int restaStock = pIventario[i].Stock - diferCantid;
                                    T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, restaStock, pIventario[i].Precio,
                                pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId, -diferCantid);
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
                                        pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId, diferCantid);
                                        }
                                        else if (pedido[e].Cantidad < pIventario[i].Cantidad)
                                        {
                                            diferCantid = pIventario[i].Cantidad - pedido[i].Cantidad;
                                            int restaStock = pIventario[i].Stock - diferCantid;
                                            T = await ivm.PutInventario(pIventario[i].Id, pIventario[i].Fecha, restaStock, pIventario[i].Precio,
                                        pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId, -diferCantid);
                                        }
                                    }
                                }
                            }
                            T = await PutPedidosInve(pedido[i].DetalleId, pCodigo, pIventario[i].Id, 
                                pIventario[i].Cantidad, pIventario[i].PrecioUn, pIventario[i].Total, pIventario[i].Fecha);
                        }
                        else if (i > 1-pIventario.Count)
                        {
                            T = await MiPedidosInve.DeletePedidosInventario(pedido[i].DetalleId);
                            bool J = false;
                            if (T)
                            {
                                Inventario inventario = new Inventario();
                                inventario = await MiInventario.GetInventarioId(pedido[i].InventarioId);
                                int suma = pedido[i].Cantidad + inventario.Stock;
                                T = await ivm.PutInventario(inventario.Id, inventario.Fecha, suma, inventario.PrecioUn,
                                inventario.Origen, inventario.ProductoCodigo, inventario.EmpaqueId, pedido[i].Cantidad);
                            }
                        }
                        if (i < pIventario.Count)
                        {
                            string usu = GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2;
                            string objetoRef = usu + " modifico un pedido de " + pIventario[i].Cantidad + " '" + pIventario[i].NombrePro + " " + pIventario[i].NombreEmp + "'. En el pedido Cod." + pCodigo;
                            bool U = await vmb.PostBitacoraSalidas(DateTime.Now, objetoRef, pIventario[i].Cantidad);
                            if (!U)
                            {
                                await DisplayAlert("Error de validación", "Error de Bitácora de Salida", "OK");
                            }
                        }
                        else
                        {
                            Inventario inventario = new Inventario();
                            inventario = await MiInventario.GetInventarioId(pedido[i].InventarioId);
                            Producto producto = new Producto();
                            producto = await MiProducto.GetProductoId(inventario.ProductoCodigo);
                            Empaque empaque = new Empaque();
                            empaque = await MiEmpaque.GetEmpaqueId(inventario.EmpaqueId);
                            string usu = GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2;
                            string objetoRef = usu + " modifico un pedido de " + pedido[i].Cantidad + " '" + producto.Nombre + " " + empaque.Nombre + " " + empaque.Tamannio + "'. En el pedido Cod." + pCodigo;
                            bool U = await vmb.PostBitacoraSalidas(DateTime.Now, objetoRef, -pedido[i].Cantidad);
                            if (!U)
                            {
                                await DisplayAlert("Error de validación", "Error de Bitácora de Salida", "OK");
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
                                    pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId, -pIventario[i].Cantidad);
                            }
                            string usu = GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2;
                            string objetoRef = usu + " modifico un pedido de " + pIventario[i].Cantidad + " '" + pIventario[i].NombrePro + " " + pIventario[i].NombreEmp + "'. En el pedido Cod." + pCodigo;
                            bool U = await vmb.PostBitacoraSalidas(DateTime.Now, objetoRef, pIventario[i].Cantidad);
                            if (!U)
                            {
                                await DisplayAlert("Error de validación", "Error de Bitácora de Salida", "OK");
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
                                pIventario[i].Origen, pIventario[i].ProductoCodigo, pIventario[i].EmpaqueId, -pIventario[i].Cantidad);
                        }
                        string usu = GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2;
                        string objetoRef = usu + " modifico un pedido de " + pIventario[i].Cantidad + " '" + pIventario[i].NombrePro + " " + pIventario[i].NombreEmp + "'. En el pedido Cod." + pCodigo;
                        bool U = await vmb.PostBitacoraSalidas(DateTime.Now, objetoRef, pIventario[i].Cantidad);
                        if (!U)
                        {
                            await DisplayAlert("Error de validación", "Error de Bitácora de Salida", "OK");
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
                    Inventario pedido = new Inventario();
                    pedido = await MiInventario.GetInventarioId(list[i].InventarioId);
                    Empaque empaque = new Empaque();
                    Producto producto = new Producto();
                    empaque = await MiEmpaque.GetEmpaqueId(pedido.EmpaqueId);
                    producto = await MiProducto.GetProductoId(pedido.ProductoCodigo);
                    string ProductoNomb = producto.Nombre + " " + empaque.Nombre + " " + empaque.Tamannio;
                    string usu = GlobalObject.GloUsu.Nombre + " " + GlobalObject.GloUsu.Apellido1 + " " + GlobalObject.GloUsu.Apellido2;
                    string objetoRef = usu + " elimino un pedido de " + list[i].Cantidad + " '" + ProductoNomb + "'. En el pedido cod." + pId;
                    bool U = await vmb.PostBitacoraSalidas(DateTime.Now, objetoRef, -list[i].Cantidad);
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
