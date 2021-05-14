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
        public EmailEventHandler()
        {
        }

        public Task Handle(EmailEventQueue @event)
        {
            return Task.CompletedTask;
        }
    }
}
