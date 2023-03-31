using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.ViewModels
{
    public class ProductoViewModel:BaseViewModel
    {
        public Producto MiProducto { get; set; }
        public ProductoViewModel()
        {
            ValidarConexionInternet();
            MiProducto = new Producto();
        }

        public async Task<bool> PostProducto(string pNombre,
                                            string? pDescripcion)
        {
            if (IsBusy) return false;
            IsBusy = true;

            try
            {

                MiProducto.Nombre = pNombre;
                MiProducto.Descripcion = pDescripcion;

                bool R = await MiProducto.PostProducto();
                

                return R;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<ObservableCollection<Producto>> GetBuscarProducto(string? Filtro)
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
                    ObservableCollection<Producto> list = new ObservableCollection<Producto>();
                    list = await MiProducto.GetBuscarProducto(Filtro);

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

        public async Task<bool> PutProducto(int codigo,
                                            string pNombre,
                                            string? pDescripcion)
        {
            if (IsBusy) return false;
            IsBusy = true;

            try
            {
                MiProducto.Codigo = codigo;
                MiProducto.Nombre = pNombre;
                MiProducto.Descripcion = pDescripcion;

                bool R = await MiProducto.PutProducto();


                return R;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<bool> deleteProducto(int codigo)
        {
            if (IsBusy) return false;
            IsBusy = true;
            IsBusy = true;
            try
            {
                MiProducto.Codigo = codigo;
                bool R = await MiProducto.DeleteProducto();
                return R;
            }
            catch (Exception)
            {
                return false;
            }
            finally { IsBusy = false; }
         
        }
    }
}
