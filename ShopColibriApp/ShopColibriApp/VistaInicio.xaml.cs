﻿using ShopColibriApp.ViewModels;
using ShopColibriApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaInicio : ContentPage
    {
        UsuarioViewModel vm {  get; set; }
        public VistaInicio()
        {
            InitializeComponent();

            vm = new UsuarioViewModel();
            Task.Delay(200);
            Usuario();
        }

        private async void Usuario()
        {
            bool R = false;
            if (Application.Current.Properties.ContainsKey("Usuario") && Application.Current.Properties.ContainsKey("Pass"))
            {
                try
                {
                    string u = Application.Current.Properties["Usuario"].ToString();
                    string p = Application.Current.Properties["Pass"].ToString();
                    
                    GlobalObject.GloUsu = await vm.GetUsuario(u);
                    R = await vm.ValidarAccesoUsuario(u, p);
                    if (R)
                    {
                        await Navigation.PushAsync(new MainPage());
                    }
                    else
                    {
                        new NavigationPage(new Login());
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }


        }
    }
}