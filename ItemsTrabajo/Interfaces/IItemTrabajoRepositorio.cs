using ItemsTrabajo.Api.DTOs;
using ItemsTrabajo.Api.Models;

namespace ItemsTrabajo.Api.Interfaces
{
    public interface IItemTrabajoRepositorio
    {
        // Define la consulta de todos los ítems de trabajo.
        Task<List<ItemTrabajo>> ObtenerTodosAsync();
        // Define la consulta de un ítem de trabajo por identificador.
        Task<ItemTrabajo?> ObtenerPorIdAsync(int idItemTrabajo);
        // Define el guardado de un nuevo ítem de trabajo.
        Task<ItemTrabajo> CrearAsync(ItemTrabajo itemTrabajo);
        // Define el conteo de ítems pendientes de un usuario.
        Task<int> ObtenerCantidadPendientesPorUsuarioAsync(int idUsuario);
        // Define el conteo de ítems pendientes relevantes de un usuario.
        Task<int> ObtenerCantidadRelevantesPendientesPorUsuarioAsync(int idUsuario);
        // Define la actualización de un ítem de trabajo como completado.
        Task<bool> MarcarComoCompletadoAsync(int idItemTrabajo);
        // Define la consulta de ítems pendientes ordenados para reportes.
        Task<List<ItemTrabajo>> ObtenerPendientesOrdenadosAsync();
    }
}
