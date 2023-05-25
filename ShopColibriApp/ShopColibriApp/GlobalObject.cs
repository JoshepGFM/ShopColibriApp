using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ShopColibriApp
{
    public static class GlobalObject
    {
        public static int NumeroRecuperacion;
        public static bool AgregadoUsuSis = false;
        public static Usuario GloUsu = new Usuario();
        public static Usuario GloUsuPedi = new Usuario();
        public static Usuario GloUsu_Registro = new Usuario();
        public static ObservableCollection<Usuario> GloListUsu = new ObservableCollection<Usuario>();
        public static Producto GloProd = new Producto();
        public static List<Imagen> GloImagenes = new List<Imagen>();
        public static Inventario GLoInventario = new Inventario();
        public static InventarioDTO GloInven_DTO = new InventarioDTO();
        public static Empaque GloEmpaque = new Empaque();
        public static ControlMarmita GloControMarmi = new ControlMarmita();
        public static ControlMarmita GloControMarmi_Cont = new ControlMarmita();
        public static ControlMarmitaDTO GloControlMarDTO = new ControlMarmitaDTO();
        public static Registro GloRegistro = new Registro();
        public static Pedidos GloPedidos = new Pedidos();
        public static Pedidos GloPedidos_Cont = new Pedidos();
        public static PedidosDTO GloPedidosDTO = new PedidosDTO();
        public static ObservableCollection<PedidosCalcu> GloListInven = new ObservableCollection<PedidosCalcu>();
    }
}
