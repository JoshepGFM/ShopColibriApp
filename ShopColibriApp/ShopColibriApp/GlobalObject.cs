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
        public static Usuario GloUsu = new Usuario();
        public static Usuario GloUsu_Registro = new Usuario();
        public static Producto GloProd = new Producto();
        public static int NumeroRecuperacion;
        public static bool AgregadoUsuSis = false;
        public static ObservableCollection<FileImageSource> GloImagenes = new ObservableCollection<FileImageSource>();
        public static Inventario GLoInventario = new Inventario();
    }
}
