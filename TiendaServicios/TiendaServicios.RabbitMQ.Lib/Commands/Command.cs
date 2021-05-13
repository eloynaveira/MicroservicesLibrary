using System;
using System.Collections.Generic;
using System.Text;
using TiendaServicios.RabbitMQ.Lib.Events;

namespace TiendaServicios.RabbitMQ.Lib.Commands
{
    public abstract class Command : Message
    {
        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; protected set; }


    }
}
