using ItemsTrabajo.Api.Data;
using ItemsTrabajo.Api.Interfaces;
using ItemsTrabajo.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemsTrabajo.Api.Repositorios
{
    public class ItemTrabajoRepositorio : IItemTrabajoRepositorio
    {
        private readonly BdItemsTrabajoContext _contexto;

        // Inicializa el repositorio con el contexto de base de datos de ítems de trabajo.
        public ItemTrabajoRepositorio(BdItemsTrabajoContext contexto)
        {
            _contexto = contexto;
        }

        // Consulta todos los ítems de trabajo almacenados.
        public async Task<List<ItemTrabajo>> ObtenerTodosAsync()
        {
            return await _contexto.ItemsTrabajos.ToListAsync();
        }

        // Consulta un ítem de trabajo por su identificador.
        public async Task<ItemTrabajo?> ObtenerPorIdAsync(int idItemTrabajo)
        {
            return await _contexto.ItemsTrabajos
                .FirstOrDefaultAsync(i => i.IdItemTrabajo == idItemTrabajo);
        }

        // Guarda un nuevo ítem de trabajo en la base de datos.
        public async Task<ItemTrabajo> CrearAsync(ItemTrabajo itemTrabajo)
        {
            _contexto.ItemsTrabajos.Add(itemTrabajo);
            await _contexto.SaveChangesAsync();

            return itemTrabajo;
        }

        // Cuenta los ítems pendientes asignados a un usuario.
        public async Task<int> ObtenerCantidadPendientesPorUsuarioAsync(int idUsuario)
        {
            return await _contexto.ItemsTrabajos
                .CountAsync(i => i.IdUsuarioAsignado == idUsuario && i.Estado == "Pendiente");
        }

        // Cuenta los ítems pendientes relevantes asignados a un usuario.
        public async Task<int> ObtenerCantidadRelevantesPendientesPorUsuarioAsync(int idUsuario)
        {
            // Se obtiene la cantidad de ítems pendientes altamente relevantes asignados al usuario.
            return await _contexto.ItemsTrabajos
                .CountAsync(i =>
                    i.IdUsuarioAsignado == idUsuario &&
                    i.Estado == "Pendiente" &&
                    i.EsRelevante == true);
        }

        // Actualiza el estado de un ítem de trabajo para dejarlo como completado.
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

        // Obtiene los ítems pendientes ordenados por usuario, relevancia y fecha de entrega.
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
