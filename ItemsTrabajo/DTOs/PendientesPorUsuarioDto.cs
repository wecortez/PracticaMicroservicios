namespace ItemsTrabajo.Api.DTOs
{
    public class PendientesPorUsuarioDto
    {
        public int IdUsuarioAsignado { get; set; }
        public int TotalPendientes { get; set; }
        public int TotalRelevantesPendientes { get; set; }
        public List<ItemTrabajoPendienteDto> ItemsPendientes { get; set; } = new();
    }

    public class ItemTrabajoPendienteDto
    {
        public int IdItemTrabajo { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool EsRelevante { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
