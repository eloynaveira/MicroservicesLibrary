using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> logger;

        public LibroHandler(ILogger<LibroHandler> _logger)
        {
            logger = _logger;
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
            }
            logger.LogInformation($"Request final: {time.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
