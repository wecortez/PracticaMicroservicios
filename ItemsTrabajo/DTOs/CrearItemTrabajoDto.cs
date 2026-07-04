namespace ItemsTrabajo.Api.DTOs
{
    public class CrearItemTrabajoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool EsRelevante { get; set; }
        public DateTime FechaEntrega { get; set; }
    }
}
