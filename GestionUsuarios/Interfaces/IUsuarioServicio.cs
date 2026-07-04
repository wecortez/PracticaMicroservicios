using GestionUsuarios.Api.DTOs;
using GestionUsuarios.Api.Models;

namespace GestionUsuarios.Api.Interfaces
{
    public interface IUsuarioServicio
    {
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int idUsuario);
        Task<Usuario> CrearAsync(CrearUsuarioDto usuarioDto);
    }
}
