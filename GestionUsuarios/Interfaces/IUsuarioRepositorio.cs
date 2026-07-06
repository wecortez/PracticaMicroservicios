using GestionUsuarios.Api.Models;

namespace GestionUsuarios.Api.Interfaces
{
    public interface IUsuarioRepositorio
    {
        // Define la consulta de todos los usuarios activos.
        Task<List<Usuario>> ObtenerTodosAsync();
        // Define la consulta de un usuario activo por identificador.
        Task<Usuario?> ObtenerPorIdAsync(int idUsuario);
        // Define el guardado de un nuevo usuario.
        Task<Usuario> CrearAsync(Usuario usuario);
        // Define la verificación de existencia de un usuario activo.
        Task<bool> ExisteUsuarioAsync(int idUsuario);
    }
}
