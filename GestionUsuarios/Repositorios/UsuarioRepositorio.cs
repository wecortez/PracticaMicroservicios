using GestionUsuarios.Api.Data;
using GestionUsuarios.Api.Interfaces;
using GestionUsuarios.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionUsuarios.Api.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BdGestionUsuariosContext _contexto;

        public UsuarioRepositorio(BdGestionUsuariosContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _contexto.Usuarios
                .Where(u => u.EstaActivo == true)
                .ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int idUsuario)
        {
            return await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario && u.EstaActivo == true);
        }

        public async Task<Usuario> CrearAsync(Usuario usuario)
        {
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> ExisteUsuarioAsync(int idUsuario)
        {
            return await _contexto.Usuarios
                .AnyAsync(u => u.IdUsuario == idUsuario && u.EstaActivo == true);
        }
    }
}