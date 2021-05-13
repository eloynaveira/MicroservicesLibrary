using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaServicios.RabbitMQ.Lib.Events
{
    public abstract class Message  : IRequest<bool>
    {
        public string MessageType { get; set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
