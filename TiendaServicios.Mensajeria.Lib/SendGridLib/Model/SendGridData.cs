using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaServicios.Mensajeria.Lib.SendGridLib.Model
{
    public class SendGridData
    {
        public string SendGridAPIKey { get; set; }

        public string EmailDestination { get; set; }

        public string NameDestination { get; set; }

        public string EmailTitle { get; set; }

        public string Content { get; set; }
    }
}
