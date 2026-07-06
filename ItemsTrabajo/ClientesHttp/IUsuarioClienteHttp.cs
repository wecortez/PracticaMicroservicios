using ItemsTrabajo.Api.DTOs;

namespace ItemsTrabajo.Api.ClientesHttp
{
    public interface IUsuarioClienteHttp
    {
        // Define la consulta de usuarios activos desde el microservicio GestionUsuarios.
        Task<List<UsuarioDto>> ObtenerUsuariosActivosAsync();
    }
}
