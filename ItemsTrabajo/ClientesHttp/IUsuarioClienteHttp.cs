using ItemsTrabajo.Api.DTOs;

namespace ItemsTrabajo.Api.ClientesHttp
{
    public interface IUsuarioClienteHttp
    {
        Task<List<UsuarioDto>> ObtenerUsuariosActivosAsync();
    }
}
