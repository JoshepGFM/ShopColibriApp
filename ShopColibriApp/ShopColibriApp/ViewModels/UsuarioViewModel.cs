using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ShopColibriApp.Servicios;
using Xamarin.Forms;
using System.Linq;

namespace ShopColibriApp.ViewModels
{
    public class UsuarioViewModel : BaseViewModel
    {
        public Usuario MiUsuario { get; set; }
        public Tusuario MiTusuario { get; set; }
        public VerificacionEmail VEmail { get; set; }
        public int Index { get; set; }
        public UsuarioViewModel() 
        {
            VEmail = new VerificacionEmail();
            MiUsuario = new Usuario();
            MiTusuario = new Tusuario();
            ValidarConexionInternet();
        }

        public bool IsPasswordSecure(string password)
        {
            bool hasNumber = false;
            bool hasUpperChar = false;
            bool hasMinusChar = false;

            if (password.Length < 8)
                return false;

            foreach (char c in password)
            {
                if (c >= '0' && c <= '9')
                {
                    hasNumber = true;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    hasUpperChar = true;
                }
                else if (c >= 'a' && c <= 'z')
                {
                    hasMinusChar = true;
                }
                else
                {
                    return false;
                }
            }
            return hasNumber && hasUpperChar && hasMinusChar;
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";
            Match match = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            if (match.Success)
                return true;
            else
                return false;
        }

        public async Task<Usuario> GetUsuario(string Usuario)
        {
            try
            {
                Usuario usuario = new Usuario();

                usuario = await MiUsuario.GetUserData(Usuario);

                if (usuario == null)
                {
                    return null;
                }
                else
                {
                    return usuario;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Usuario>> GetUser()
        {
            try
            {
                List<Usuario> usuario = new List<Usuario>();

                usuario = await MiUsuario.GetUser();

                if (usuario == null)
                {
                    return null;
                }
                else
                {
                    return usuario;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Tusuario>> GetTUsuario()
        {
            try
            {
                List<Tusuario> tusuario = new List<Tusuario>();

                tusuario = await MiTusuario.GetTUsuario();

                if (tusuario == null)
                {
                    return null;
                }
                else
                {
                    return tusuario;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Usuario> GetUsuarioId(int id)
        {
            try
            {
                Usuario usuario = new Usuario();
                usuario = await MiUsuario.GetUsuarioId(id);
                return usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> ValidarAccesoUsuario(string pEmail, string pass)
        {
            if (IsBusy) return false;
            IsBusy = true;

            try
            {
                MiUsuario.Email = pEmail;
                MiUsuario.Contrasennia = pass;

                bool R = await MiUsuario.ValidarLogin();

                return R;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> ValidarConexion()
        {
            if (IsBusy) return false;
            IsBusy = true;

            try
            {

                bool R = await MiUsuario.ValidarConexion();

                return R;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<bool> PostUsuario(string pNombre, string pApellido1, string pApellido2,
                                            string? pEmail, string pContrasennia, string? pEmailResp,
                                            string pTelefono,int pTUsuario)
        {
            if (IsBusy) return false;
            IsBusy = true;

            try
            {

                MiUsuario.Nombre = pNombre;
                MiUsuario.Apellido1 = pApellido1;
                MiUsuario.Apellido2 = pApellido2;
                MiUsuario.Email = pEmail;
                MiUsuario.Contrasennia = pContrasennia;
                MiUsuario.EmailResp = pEmailResp;
                MiUsuario.TusuarioId = pTUsuario;
                MiUsuario.Telefono = pTelefono;
                MiUsuario.Tipo = "";

                Usuario usuario = GlobalObject.GloUsu;
                int id = 3;
                bool R = false;
                if (usuario.IdUsuario > 0)
                {
                    id = usuario.IdUsuario;
                     R = await MiUsuario.PostUsuario(1);
                }
                else
                {
                    R = await MiUsuario.PostUsuario(id);
                }

                MiUsuario.IdUsuario = await GetUltUser();
                if ( R && id != 1)
                {
                    string enlace = string.Format(Servicios.CnnToShopColibri.UrlProduction + "Usuarios/Validar?id={0}&R={1}", MiUsuario.IdUsuario, false);
                    string menssage = string.Format("<h5>Para activar tu cuenta dar <a href='{0}'> Click aquí</a></h5>", enlace);

                    VEmail.Index(pEmail, "Verificar cuenta", menssage);

                    await DisplayAlert("Envío correo","Se le envío un correo a " + pEmail,"OK");
                }

                return R;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally { IsBusy = false; }
        }
        public async Task<ObservableCollection<Usuario>> GetUsuBuscar(string? Filtro, bool? estado, bool cliete = true)
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
                    ObservableCollection<Usuario> list = new ObservableCollection<Usuario>();
                    list = await MiUsuario.GetUsuarioBuscar(Filtro, estado, cliete);

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

        public async Task<bool> PatchUsuario(Usuario usuario, string pass)
        {
            
            if (IsBusy) return false;
            IsBusy = true;

            try
            {
                MiUsuario.IdUsuario = usuario.IdUsuario;
                MiUsuario.Nombre = usuario.Nombre;
                MiUsuario.Apellido1 = usuario.Apellido1;
                MiUsuario.Apellido2 = usuario.Apellido2;
                MiUsuario.Email = usuario.Email;
                MiUsuario.Contrasennia = pass;
                MiUsuario.EmailResp = usuario.EmailResp;
                MiUsuario.TusuarioId = usuario.TusuarioId;
                MiUsuario.Telefono = usuario.Telefono;
                MiUsuario.Tipo = usuario.Tipo;

                bool R = await MiUsuario.PatchUsuario();

                return R;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<bool> ValidarUsuario(int id)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                bool R = await MiUsuario.ValidarUsuario(id, true);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<bool> DeleteUsuario(int id)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                bool R = await MiUsuario.DeleteUsuario(id);
                return R;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally { IsBusy = false; }
        }

        private async Task<int> GetUltUser()
        {
            try
            {
                List<Usuario> usuario = new List<Usuario>();
                usuario = await MiUsuario.GetAllUser();
                Usuario item = new Usuario();
                item = usuario.Last();

                if (item != null)
                {
                    return item.IdUsuario;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
