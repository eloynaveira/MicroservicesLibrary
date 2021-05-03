using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteService
{
    public class LibrosService : ILibrosService
    {
        private readonly IHttpClientFactory httpClient;
        private readonly ILogger logger;

        public LibrosService(IHttpClientFactory _httpClient, ILogger _logger)
        {
            httpClient = _httpClient;
            logger = _logger;
        }

        public async Task<(bool resultado, LibroRemote libro, string errorMessage)> GetLibro(Guid libroId)
        {
            try
            {
                var client = httpClient.CreateClient("Libros");
                var response = await client.GetAsync($"api/LibroMaterial/{libroId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var resultado = JsonSerializer.Deserialize<LibroRemote>(content, options);
                    return (true, resultado, null);
                }
                
                return (false, null, response.ReasonPhrase);
            } 
            catch(Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }         
        }
    }
}
