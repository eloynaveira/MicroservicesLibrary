using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Lib.BusRabbit;
using TiendaServicios.RabbitMQ.Lib.EventQueue;

namespace TiendaServicios.Api.Autor.ManejadorRabbit
{
    public class EmailEventHandler : IEventHandler<EmailEventQueue>
    {
        private readonly ILogger<EmailEventHandler> logger;

        public EmailEventHandler()
        {
        }

        public EmailEventHandler(ILogger<EmailEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(EmailEventQueue @event)
        {
            logger.LogInformation(@event.Titulo);

            return Task.CompletedTask;
        }
    }
}
