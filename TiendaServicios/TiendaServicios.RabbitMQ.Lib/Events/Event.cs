using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaServicios.RabbitMQ.Lib.Events
{
    public abstract class Event
    {
        protected Event()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; protected set; }
    }
}
