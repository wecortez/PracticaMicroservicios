using GestionUsuarios.Api.DTOs;
using GestionUsuarios.Api.Interfaces;
using GestionUsuarios.Api.Models;

namespace GestionUsuarios.Api.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        // Inicializa el servicio con el repositorio de usuarios.
        public UsuarioServicio(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        // Obtiene todos los usuarios activos registrados.
        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _usuarioRepositorio.ObtenerTodosAsync();
        }

        // Busca un usuario activo por su identificador.
        public async Task<Usuario?> ObtenerPorIdAsync(int idUsuario)
        {
            return await _usuarioRepositorio.ObtenerPorIdAsync(idUsuario);
        }

        // Crea un usuario activo a partir de la información recibida.
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
