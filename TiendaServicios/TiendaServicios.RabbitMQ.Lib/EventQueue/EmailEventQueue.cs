using System;
using System.Collections.Generic;
using System.Text;
using TiendaServicios.RabbitMQ.Lib.Events;

namespace TiendaServicios.RabbitMQ.Lib.EventQueue
{
    public class EmailEventQueue : Event
    {
        public EmailEventQueue(string destinatario, string titulo, string contenido)
        {
            Destinatario = destinatario;
            Titulo = titulo;
            Contenido = contenido;
        }

        public string Destinatario { get; set; }

        public string Titulo { get; set; }

        public string Contenido { get; set; }


    }
}
