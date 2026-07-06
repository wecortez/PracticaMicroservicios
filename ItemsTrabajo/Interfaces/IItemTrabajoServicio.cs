using ItemsTrabajo.Api.DTOs;
using ItemsTrabajo.Api.Models;

namespace ItemsTrabajo.Api.Interfaces
{
    public interface IItemTrabajoServicio
    {
        // Define la obtención de todos los ítems de trabajo.
        Task<List<ItemTrabajo>> ObtenerTodosAsync();
        // Define la búsqueda de un ítem de trabajo por identificador.
        Task<ItemTrabajo?> ObtenerPorIdAsync(int idItemTrabajo);
        // Define la creación de un ítem de trabajo a partir de un DTO.
        Task<ItemTrabajo> CrearAsync(CrearItemTrabajoDto itemTrabajoDto);
        // Define el cambio de estado de un ítem de trabajo a completado.
        Task<bool> MarcarComoCompletadoAsync(int idItemTrabajo);
        // Define la obtención del resumen de pendientes agrupados por usuario.
        Task<List<PendientesPorUsuarioDto>> ObtenerPendientesPorUsuarioAsync();
    }
}
