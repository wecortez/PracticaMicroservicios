using ItemsTrabajo.Api.DTOs;

namespace ItemsTrabajo.Api.ClientesHttp
{
    public class UsuarioClienteHttp : IUsuarioClienteHttp
    {
        private readonly HttpClient _httpClient;

        public UsuarioClienteHttp(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UsuarioDto>> ObtenerUsuariosActivosAsync()
        {
            var usuarios = await _httpClient.GetFromJsonAsync<List<UsuarioDto>>("api/Usuarios");

            //Filtrar usuario activos
            return usuarios ?? new List<UsuarioDto>();
        }
    }
}