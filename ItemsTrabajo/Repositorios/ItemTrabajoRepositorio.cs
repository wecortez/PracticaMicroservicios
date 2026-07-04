using ItemsTrabajo.Api.Data;
using ItemsTrabajo.Api.Interfaces;
using ItemsTrabajo.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemsTrabajo.Api.Repositorios
{
    public class ItemTrabajoRepositorio : IItemTrabajoRepositorio
    {
        private readonly BdItemsTrabajoContext _contexto;

        public ItemTrabajoRepositorio(BdItemsTrabajoContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<ItemTrabajo>> ObtenerTodosAsync()
        {
            return await _contexto.ItemsTrabajos.ToListAsync();
        }

        public async Task<ItemTrabajo?> ObtenerPorIdAsync(int idItemTrabajo)
        {
            return await _contexto.ItemsTrabajos
                .FirstOrDefaultAsync(i => i.IdItemTrabajo == idItemTrabajo);
        }

        public async Task<ItemTrabajo> CrearAsync(ItemTrabajo itemTrabajo)
        {
            _contexto.ItemsTrabajos.Add(itemTrabajo);
            await _contexto.SaveChangesAsync();

            return itemTrabajo;
        }

        public async Task<int> ObtenerCantidadPendientesPorUsuarioAsync(int idUsuario)
        {
            return await _contexto.ItemsTrabajos
                .CountAsync(i => i.IdUsuarioAsignado == idUsuario && i.Estado == "Pendiente");
        }

        public async Task<int> ObtenerCantidadRelevantesPendientesPorUsuarioAsync(int idUsuario)
        {
            // Se obtiene la cantidad de ítems pendientes altamente relevantes asignados al usuario.
            return await _contexto.ItemsTrabajos
                .CountAsync(i =>
                    i.IdUsuarioAsignado == idUsuario &&
                    i.Estado == "Pendiente" &&
                    i.EsRelevante == true);
        }

        public async Task<bool> MarcarComoCompletadoAsync(int idItemTrabajo)
        {
            // Se busca el ítem de trabajo por su identificador.
            var itemTrabajo = await _contexto.ItemsTrabajos
                .FirstOrDefaultAsync(i => i.IdItemTrabajo == idItemTrabajo);

            if (itemTrabajo == null)
                return false;

            // Se actualiza el estado para que ya no sea considerado como pendiente.
            itemTrabajo.Estado = "Completado";

            await _contexto.SaveChangesAsync();

            return true;
        }

        public async Task<List<ItemTrabajo>> ObtenerPendientesOrdenadosAsync()
        {
            // Se obtiene la lista de ítems pendientes ordenada por usuario, relevancia y fecha de entrega.
            return await _contexto.ItemsTrabajos
                .Where(i => i.Estado == "Pendiente" && i.IdUsuarioAsignado != null)
                .OrderBy(i => i.IdUsuarioAsignado)
                .ThenByDescending(i => i.EsRelevante)
                .ThenBy(i => i.FechaEntrega)
                .ToListAsync();
        }
    }
}