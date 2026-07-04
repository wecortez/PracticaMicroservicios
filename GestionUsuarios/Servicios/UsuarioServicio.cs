using GestionUsuarios.Api.DTOs;
using GestionUsuarios.Api.Interfaces;
using GestionUsuarios.Api.Models;

namespace GestionUsuarios.Api.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioServicio(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _usuarioRepositorio.ObtenerTodosAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int idUsuario)
        {
            return await _usuarioRepositorio.ObtenerPorIdAsync(idUsuario);
        }

        public async Task<Usuario> CrearAsync(CrearUsuarioDto usuarioDto)
        {
            var usuario = new Usuario
            {
                NombreCompleto = usuarioDto.NombreCompleto,
                Correo = usuarioDto.Correo,
                EstaActivo = true,
                FechaCreacion = DateTime.Now
            };

            return await _usuarioRepositorio.CrearAsync(usuario);
        }
    }
}