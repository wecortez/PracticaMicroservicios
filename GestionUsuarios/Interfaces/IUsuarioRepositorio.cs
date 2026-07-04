using GestionUsuarios.Api.Models;

namespace GestionUsuarios.Api.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int idUsuario);
        Task<Usuario> CrearAsync(Usuario usuario);
        Task<bool> ExisteUsuarioAsync(int idUsuario);
    }
}
