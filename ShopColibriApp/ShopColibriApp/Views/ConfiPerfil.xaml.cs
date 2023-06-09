﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShopColibriApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfiPerfil : ContentPage
    {
        public ConfiPerfil()
        {
            InitializeComponent();
        }

        private async void BtnEditarPer_Clicked(object sender, EventArgs e)
        {
            GlobalObject.GloUsu_Registro = null;
            GlobalObject.EditUsuario = true;
            await Navigation.PushAsync(new Registro());
        }

        private async void BtnCambClave_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Recuperacion());
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MainPage());

            // Retornar true para indicar que se ha manejado el evento del botón "Back"
            return true;
        }
    }
}