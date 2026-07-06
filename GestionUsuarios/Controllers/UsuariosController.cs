using GestionUsuarios.Api.DTOs;
using GestionUsuarios.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionUsuarios.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioServicio _usuarioServicio;

        // Inicializa el controlador con el servicio de usuarios.
        public UsuariosController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        // Solicitud para listar todos los usuarios activos.
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarios = await _usuarioServicio.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        // Solicitud para obtener un usuario por su id.
        [HttpGet("{idUsuario}")]
        public async Task<IActionResult> ObtenerPorId(int idUsuario)
        {
            var usuario = await _usuarioServicio.ObtenerPorIdAsync(idUsuario);

            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            return Ok(usuario);
        }

        // Solicitud para crear un nuevo usuario.
        [HttpPost]        
        public async Task<IActionResult> Crear([FromBody] CrearUsuarioDto usuarioDto)
        {
            var usuarioCreado = await _usuarioServicio.CrearAsync(usuarioDto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { idUsuario = usuarioCreado.IdUsuario },
                usuarioCreado
            );
        }
    }
}
