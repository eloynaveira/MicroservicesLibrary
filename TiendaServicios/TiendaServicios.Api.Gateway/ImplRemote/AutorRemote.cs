using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.InterfaceRemote;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.ImplRemote
{
    public class AutorRemote : IAutorRemote
    {
        private readonly IHttpClientFactory httpClient;
        private readonly ILogger<AutorRemote> logger;

        public AutorRemote(IHttpClientFactory _httpClient, ILogger<AutorRemote> _logger)
        {
            httpClient = _httpClient;
            logger = _logger;
        }

        public async Task<(bool resultado, AutorModeloRemote autor, string ErrorMessage)> GetAutor(Guid autorId)
        {
            try
            {
                var client = httpClient.CreateClient("AutorService");
                var response = await client.GetAsync($"/Autor/{autorId}");
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<AutorModeloRemote>(content, options);
                    return (true, result, null);
                }

                return (false, null, response.ReasonPhrase);
            } 
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
