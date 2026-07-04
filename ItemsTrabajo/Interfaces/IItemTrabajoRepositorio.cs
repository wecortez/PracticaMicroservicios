using ItemsTrabajo.Api.DTOs;
using ItemsTrabajo.Api.Models;

namespace ItemsTrabajo.Api.Interfaces
{
    public interface IItemTrabajoRepositorio
    {
        Task<List<ItemTrabajo>> ObtenerTodosAsync();
        Task<ItemTrabajo?> ObtenerPorIdAsync(int idItemTrabajo);
        Task<ItemTrabajo> CrearAsync(ItemTrabajo itemTrabajo);
        Task<int> ObtenerCantidadPendientesPorUsuarioAsync(int idUsuario);
        Task<int> ObtenerCantidadRelevantesPendientesPorUsuarioAsync(int idUsuario);
        Task<bool> MarcarComoCompletadoAsync(int idItemTrabajo);
        Task<List<ItemTrabajo>> ObtenerPendientesOrdenadosAsync();
    }
}
