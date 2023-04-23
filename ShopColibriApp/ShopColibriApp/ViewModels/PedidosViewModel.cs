using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopColibriApp.ViewModels
{
    public class PedidosViewModel : BaseViewModel
    {
        Pedidos MiPedidos { get; set; }
        public PedidosViewModel()
        {
            ValidarConexionInternet();

            MiPedidos = new Pedidos();
        }

    }
}
