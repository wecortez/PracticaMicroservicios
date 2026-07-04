using ItemsTrabajo.Api.DTOs;
using ItemsTrabajo.Api.Models;

namespace ItemsTrabajo.Api.Interfaces
{
    public interface IItemTrabajoServicio
    {
        Task<List<ItemTrabajo>> ObtenerTodosAsync();
        Task<ItemTrabajo?> ObtenerPorIdAsync(int idItemTrabajo);
        Task<ItemTrabajo> CrearAsync(CrearItemTrabajoDto itemTrabajoDto);
        Task<bool> MarcarComoCompletadoAsync(int idItemTrabajo);
        Task<List<PendientesPorUsuarioDto>> ObtenerPendientesPorUsuarioAsync();
    }
}
