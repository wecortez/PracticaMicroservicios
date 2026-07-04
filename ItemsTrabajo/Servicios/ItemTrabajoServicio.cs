using ItemsTrabajo.Api.ClientesHttp;
using ItemsTrabajo.Api.DTOs;
using ItemsTrabajo.Api.Interfaces;
using ItemsTrabajo.Api.Models;

namespace ItemsTrabajo.Api.Servicios
{
    public class ItemTrabajoServicio : IItemTrabajoServicio
    {
        private readonly IItemTrabajoRepositorio _itemTrabajoRepositorio;
        private readonly IUsuarioClienteHttp _usuarioClienteHttp;

        public ItemTrabajoServicio(IItemTrabajoRepositorio itemTrabajoRepositorio, IUsuarioClienteHttp usuarioClienteHttp)
        {
            _itemTrabajoRepositorio = itemTrabajoRepositorio;
            _usuarioClienteHttp = usuarioClienteHttp;
        }

        public async Task<List<ItemTrabajo>> ObtenerTodosAsync()
        {
            return await _itemTrabajoRepositorio.ObtenerTodosAsync();
        }

        public async Task<ItemTrabajo?> ObtenerPorIdAsync(int idItemTrabajo)
        {
            return await _itemTrabajoRepositorio.ObtenerPorIdAsync(idItemTrabajo);
        }

        public async Task<ItemTrabajo> CrearAsync(CrearItemTrabajoDto itemTrabajoDto)
        {
            // Se obtiene la lista de usuarios activos desde el microservicio GestionUsuarios.Api.
            var usuariosActivos = await _usuarioClienteHttp.ObtenerUsuariosActivosAsync();

            if (!usuariosActivos.Any())
                throw new Exception("No existen usuarios activos para asignar el ítem de trabajo.");

            // Se selecciona el usuario que tenga menos ítems pendientes asignados.
            var idUsuarioAsignado = await ObtenerUsuarioParaAsignacionAsync(usuariosActivos, itemTrabajoDto);

            var itemTrabajo = new ItemTrabajo
            {
                Titulo = itemTrabajoDto.Titulo,
                Descripcion = itemTrabajoDto.Descripcion,
                EsRelevante = itemTrabajoDto.EsRelevante,
                FechaEntrega = itemTrabajoDto.FechaEntrega,
                Estado = "Pendiente",
                IdUsuarioAsignado = idUsuarioAsignado,
                FechaCreacion = DateTime.Now
            };

            return await _itemTrabajoRepositorio.CrearAsync(itemTrabajo);
        }

        private async Task<int> ObtenerUsuarioParaAsignacionAsync(List<UsuarioDto> usuariosActivos, CrearItemTrabajoDto itemTrabajoDto)
        {
            var fechaActual = DateTime.Now.Date;
            var diasParaEntrega = (itemTrabajoDto.FechaEntrega.Date - fechaActual).TotalDays;

            var fechaProximaAVencer = diasParaEntrega < 3;

            var usuariosDisponibles = new List<(int IdUsuario, int Pendientes, int RelevantesPendientes)>();

            // Se calcula la carga de trabajo de cada usuario activo.
            foreach (var usuario in usuariosActivos)
            {
                var cantidadPendientes = await _itemTrabajoRepositorio
                    .ObtenerCantidadPendientesPorUsuarioAsync(usuario.IdUsuario);

                var cantidadRelevantesPendientes = await _itemTrabajoRepositorio
                    .ObtenerCantidadRelevantesPendientesPorUsuarioAsync(usuario.IdUsuario);

                // Si el usuario tiene más de tres ítems altamente relevantes pendientes, se considera saturado.
                if (cantidadRelevantesPendientes > 3)
                    continue;

                usuariosDisponibles.Add((
                    usuario.IdUsuario,
                    cantidadPendientes,
                    cantidadRelevantesPendientes
                ));
            }

            if (!usuariosDisponibles.Any())
                throw new Exception("No existen usuarios disponibles para asignar el ítem de trabajo.");

            if (fechaProximaAVencer)
            {
                // Si la fecha está próxima a vencer, se asigna al usuario con menos pendientes sin importar la relevancia.
                return usuariosDisponibles
                    .OrderBy(u => u.Pendientes)
                    .ThenBy(u => u.RelevantesPendientes)
                    .Select(u => u.IdUsuario)
                    .First();
            }

            if (itemTrabajoDto.EsRelevante)
            {
                // Si el ítem es relevante, se asigna al usuario con menor lista de pendientes.
                return usuariosDisponibles
                    .OrderBy(u => u.Pendientes)
                    .ThenBy(u => u.RelevantesPendientes)
                    .Select(u => u.IdUsuario)
                    .First();
            }

            // Si no está próximo a vencer y no es relevante, se asigna igualmente al usuario con menor carga.
            return usuariosDisponibles
                .OrderBy(u => u.Pendientes)
                .ThenBy(u => u.RelevantesPendientes)
                .Select(u => u.IdUsuario)
                .First();
        }

        public async Task<bool> MarcarComoCompletadoAsync(int idItemTrabajo)
        {
            // Se delega al repositorio la actualización del estado del ítem de trabajo.
            return await _itemTrabajoRepositorio.MarcarComoCompletadoAsync(idItemTrabajo);
        }

        public async Task<List<PendientesPorUsuarioDto>> ObtenerPendientesPorUsuarioAsync()
        {
            // Se obtiene la lista ordenada de pendientes desde la base de datos.
            var itemsPendientes = await _itemTrabajoRepositorio.ObtenerPendientesOrdenadosAsync();

            // Se agrupan los ítems pendientes por usuario asignado para facilitar el seguimiento de carga.
            var resultado = itemsPendientes
                .GroupBy(i => i.IdUsuarioAsignado!.Value)
                .Select(grupo => new PendientesPorUsuarioDto
                {
                    IdUsuarioAsignado = grupo.Key,
                    TotalPendientes = grupo.Count(),
                    TotalRelevantesPendientes = grupo.Count(i => i.EsRelevante),
                    ItemsPendientes = grupo.Select(i => new ItemTrabajoPendienteDto
                    {
                        IdItemTrabajo = i.IdItemTrabajo,
                        Titulo = i.Titulo,
                        Descripcion = i.Descripcion,
                        EsRelevante = i.EsRelevante,
                        FechaEntrega = i.FechaEntrega,
                        Estado = i.Estado
                    }).ToList()
                })
                .OrderBy(r => r.IdUsuarioAsignado)
                .ToList();

            return resultado;
        }
    }
}