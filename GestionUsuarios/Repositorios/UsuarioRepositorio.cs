using GestionUsuarios.Api.Data;
using GestionUsuarios.Api.Interfaces;
using GestionUsuarios.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionUsuarios.Api.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BdGestionUsuariosContext _contexto;

        // Inicializa el repositorio con el contexto de base de datos de usuarios.
        public UsuarioRepositorio(BdGestionUsuariosContext contexto)
        {
            _contexto = contexto;
        }

        // Consulta todos los usuarios que se encuentran activos.
        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _contexto.Usuarios
                .Where(u => u.EstaActivo == true)
                .ToListAsync();
        }

        // Consulta un usuario activo por su identificador.
        public async Task<Usuario?> ObtenerPorIdAsync(int idUsuario)
        {
            return await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario && u.EstaActivo == true);
        }

        // Guarda un nuevo usuario en la base de datos.
        public async Task<Usuario> CrearAsync(Usuario usuario)
        {
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
            return usuario;
        }

        // Verifica si existe un usuario activo con el identificador indicado.
        public async Task<bool> ExisteUsuarioAsync(int idUsuario)
        {
            return await _contexto.Usuarios
                .AnyAsync(u => u.IdUsuario == idUsuario && u.EstaActivo == true);
        }
    }
}
