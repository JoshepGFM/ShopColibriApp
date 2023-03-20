using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.ViewModels
{
    public class UsuarioViewModel : BaseViewModel
    {
        public Usuario MiUsuario { get; set; }
        public Tusuario MiTusuario { get; set; }
        public UsuarioViewModel() 
        {
            MiUsuario = new Usuario();
            MiTusuario = new Tusuario();
            ValidarConexionInternet();
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

        public async Task<bool> PostUsuario(string pNombre,
                                            string pApellido1,
                                            string pApellido2,
                                            string? pEmail,
                                            string pContrasennia,
                                            string? pEmailResp,
                                            string pTelefono,
                                            int pTUsuario)
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

                bool R = await MiUsuario.PostUsuario();

                return R;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            finally { IsBusy = false; }
        }
        public async Task<ObservableCollection<Usuario>> GetUsuBuscar(string? Filtro, bool estado)
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
                    list = await MiUsuario.GetUsuarioBuscar(Filtro, estado);

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


    }
}
