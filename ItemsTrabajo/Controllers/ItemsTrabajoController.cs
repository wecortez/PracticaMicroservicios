using ItemsTrabajo.Api.DTOs;
using ItemsTrabajo.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemsTrabajo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsTrabajoController : ControllerBase
    {
        private readonly IItemTrabajoServicio _itemTrabajoServicio;

        public ItemsTrabajoController(IItemTrabajoServicio itemTrabajoServicio)
        {
            _itemTrabajoServicio = itemTrabajoServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var itemsTrabajo = await _itemTrabajoServicio.ObtenerTodosAsync();
            return Ok(itemsTrabajo);
        }

        [HttpGet("{idItemTrabajo}")]
        public async Task<IActionResult> ObtenerPorId(int idItemTrabajo)
        {
            var itemTrabajo = await _itemTrabajoServicio.ObtenerPorIdAsync(idItemTrabajo);

            if (itemTrabajo == null)
                return NotFound("Ítem de trabajo no encontrado.");

            return Ok(itemTrabajo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearItemTrabajoDto itemTrabajoDto)
        {
            var itemCreado = await _itemTrabajoServicio.CrearAsync(itemTrabajoDto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { idItemTrabajo = itemCreado.IdItemTrabajo },
                itemCreado
            );
        }

        [HttpPut("{idItemTrabajo}/completar")]
        public async Task<IActionResult> MarcarComoCompletado(int idItemTrabajo)
        {
            // Se marca el ítem como completado para actualizar el seguimiento de carga del usuario.
            var resultado = await _itemTrabajoServicio.MarcarComoCompletadoAsync(idItemTrabajo);

            if (!resultado)
                return NotFound("Ítem de trabajo no encontrado.");

            return Ok("Ítem de trabajo marcado como completado correctamente.");
        }

        [HttpGet("pendientes-por-usuario")]
        public async Task<IActionResult> ObtenerPendientesPorUsuario()
        {
            // Se devuelve el resumen de pendientes agrupado y ordenado por usuario.
            var resultado = await _itemTrabajoServicio.ObtenerPendientesPorUsuarioAsync();

            return Ok(resultado);
        }
    }
}