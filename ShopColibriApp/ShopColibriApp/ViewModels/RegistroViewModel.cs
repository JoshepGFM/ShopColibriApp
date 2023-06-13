using ShopColibriApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ShopColibriApp.ViewModels
{
    public class RegistroViewModel : BaseViewModel
    {
        Registro MiRegistro { get; set; }
        Usuario MiUsuario { get; set; }
        public RegistroViewModel()
        {
            ValidarConexionInternet();
            MiUsuario = new Usuario();
            MiRegistro = new Registro();
        }

        public async Task<ObservableCollection<RegistroDTO>> GetRegistroBuscar(string? Filtro)
        {
            if (IsBusy) return null;
            IsBusy = true;
            try
            {
                ObservableCollection<RegistroDTO> list = new ObservableCollection<RegistroDTO>();
                list = await MiRegistro.GetRegistroBuscar(Filtro);
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<RegistroDTO> GetRegistroId(int id)
        {
            if (IsBusy) return null;
            IsBusy = true;
            try
            {
                RegistroDTO registroDTO = new RegistroDTO();
                Registro registro = new Registro();
                registro = await MiRegistro.GetRegistroId(id);
                Usuario usuario = new Usuario();
                usuario = await MiUsuario.GetUsuarioId(registro.UsuarioIdUsuario);

                registroDTO.Id = registro.Id;
                registroDTO.Fecha = registro.Fecha;
                registroDTO.HorasL = registro.HorasL;
                registroDTO.HorasX = registro.HorasX;
                registroDTO.HorasM = registro.HorasM;
                registroDTO.HorasJ = registro.HorasJ;
                registroDTO.HorasV = registro.HorasV;
                registroDTO.HorasS = registro.HorasS;
                registroDTO.TotalHoras = registro.TotalHoras;
                registroDTO.CostoHora = registro.CostoHora;
                registroDTO.Total = registro.Total;
                registroDTO.UsuarioIdUsuario = usuario.IdUsuario;
                registroDTO.UsuarioName = usuario.Nombre + ' ' + usuario.Apellido1 + ' ' + usuario.Apellido2;

                return registroDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<bool> PostRegistro(int pIdUsuario, DateTime pFecha, int pL,int pX, int pM, int pJ, 
                                       int pV, int pS, int pTotalHoras, int pCostoHora, int pTotal)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                MiRegistro.UsuarioIdUsuario = pIdUsuario;
                MiRegistro.Fecha = pFecha;
                MiRegistro.HorasL = pL;
                MiRegistro.HorasX = pX;
                MiRegistro.HorasM = pM;
                MiRegistro.HorasJ = pJ;
                MiRegistro.HorasV = pV;
                MiRegistro.HorasS = pS;
                MiRegistro.TotalHoras = pTotalHoras;
                MiRegistro.CostoHora = pCostoHora;
                MiRegistro.Total = pTotal;

                bool R = await MiRegistro.PostRegistro();
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }

        public async Task<bool> PutRegistro(int pId, int pIdUsuario, DateTime pFecha, int pL, int pX, int pM, int pJ,
                                       int pV, int pS, int pTotalHoras, int pCostoHora, int pTotal)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                MiRegistro.Id = pId;
                MiRegistro.UsuarioIdUsuario = pIdUsuario;
                MiRegistro.Fecha = pFecha;
                MiRegistro.HorasL = pL;
                MiRegistro.HorasX = pX;
                MiRegistro.HorasM = pM;
                MiRegistro.HorasJ = pJ;
                MiRegistro.HorasV = pV;
                MiRegistro.HorasS = pS;
                MiRegistro.TotalHoras = pTotalHoras;
                MiRegistro.CostoHora = pCostoHora;
                MiRegistro.Total = pTotal;

                bool R = await MiRegistro.PutRegistro();
                return R;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { IsBusy = false; }
        }
        public async Task<bool> DeleteRegistro(int id)
        {
            if (IsBusy) return false;
            IsBusy = true;
            try
            {
                bool R = await MiRegistro.DeleteRegistro(id);
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
