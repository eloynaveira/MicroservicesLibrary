using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.InterfaceRemote;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> logger;
        private readonly IAutorRemote autorRemote;

        public LibroHandler(ILogger<LibroHandler> _logger, IAutorRemote _autorRemote)
        {
            logger = _logger;
            autorRemote = _autorRemote;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var time = Stopwatch.StartNew();

            logger.LogInformation("Start request");
            var response = await base.SendAsync(request, cancellationToken);
            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
                var result = JsonSerializer.Deserialize<LibroModeloRemote>(content, options);
                var responseAutor = await autorRemote.GetAutor(result.AutorLibro ?? Guid.Empty);
                if(responseAutor.resultado)
                {
                    var objAutor = responseAutor.autor;
                    result.AutorData = objAutor;
                    var resultadoJson = JsonSerializer.Serialize(result);
                    response.Content = new StringContent(resultadoJson, System.Text.Encoding.UTF8, "application/json");
                }
            }
            logger.LogInformation($"Request final: {time.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
