using System;
using System.Collections.Generic;
using System.Text;

namespace ShopColibriApp.Servicios
{
    internal class CnnToShopColibri
    {
        //http://www.ShopColibri.somee.com/api/
        public static string UrlProduction = "http://192.168.1.150:45455/api/";
        public static string UrlTest = "http://192.168.1.150:45455//api/";

        public static string ApiKeyName = "ColibriShop";
        public static string ApiValue = "Shopr534f23Colibri/*";

        public static string mimetype = "application/json";
        public static string contentType = "Content-Type";
    }
}
