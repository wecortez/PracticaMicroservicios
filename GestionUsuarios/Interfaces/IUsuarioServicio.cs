using GestionUsuarios.Api.DTOs;
using GestionUsuarios.Api.Models;

namespace GestionUsuarios.Api.Interfaces
{
    public interface IUsuarioServicio
    {
        // Define la obtención de todos los usuarios activos.
        Task<List<Usuario>> ObtenerTodosAsync();
        // Define la búsqueda de un usuario activo por identificador.
        Task<Usuario?> ObtenerPorIdAsync(int idUsuario);
        // Define la creación de un usuario a partir de un DTO.
        Task<Usuario> CrearAsync(CrearUsuarioDto usuarioDto);
    }
}
