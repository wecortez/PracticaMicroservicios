namespace ItemsTrabajo.Api.DTOs
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool EstaActivo { get; set; }
    }
}
