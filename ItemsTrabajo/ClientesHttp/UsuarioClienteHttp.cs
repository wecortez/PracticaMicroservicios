using ItemsTrabajo.Api.DTOs;

namespace ItemsTrabajo.Api.ClientesHttp
{
    public class UsuarioClienteHttp : IUsuarioClienteHttp
    {
        private readonly HttpClient _httpClient;

        // Inicializa el cliente HTTP usado para consumir el microservicio de usuarios.
        public UsuarioClienteHttp(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Consulta los usuarios activos disponibles desde el microservicio GestionUsuarios.
        public async Task<List<UsuarioDto>> ObtenerUsuariosActivosAsync()
        {
            var usuarios = await _httpClient.GetFromJsonAsync<List<UsuarioDto>>("api/Usuarios");

            //Filtrar usuario activos
            return usuarios ?? new List<UsuarioDto>();
        }
    }
}
