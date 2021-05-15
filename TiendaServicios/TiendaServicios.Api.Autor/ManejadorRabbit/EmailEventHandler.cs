using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Lib.SendGridLib.Interface;
using TiendaServicios.Mensajeria.Lib.SendGridLib.Model;
using TiendaServicios.RabbitMQ.Lib.BusRabbit;
using TiendaServicios.RabbitMQ.Lib.EventQueue;

namespace TiendaServicios.Api.Autor.ManejadorRabbit
{
    public class EmailEventHandler : IEventHandler<EmailEventQueue>
    {
        private readonly ILogger<EmailEventHandler> logger;
        private readonly ISendGridEmail sendGridEmail;
        private readonly IConfiguration configuration;

        public EmailEventHandler()
        {
        }

        public EmailEventHandler(ILogger<EmailEventHandler> logger, ISendGridEmail sendGridEmail, IConfiguration configuration)
        {
            this.logger = logger;
            this.sendGridEmail = sendGridEmail;
            this.configuration = configuration;
        }

        public async Task Handle(EmailEventQueue @event)
        {
            logger.LogInformation(@event.Titulo);

            var objData = new SendGridData();
            objData.Content = @event.Contenido;
            objData.EmailDestination = @event.Destinatario;
            objData.NameDestination = @event.Destinatario;
            objData.EmailTitle = @event.Titulo;
            objData.SendGridAPIKey = configuration["SendGrid:ApiKey"];

            var result = await sendGridEmail.SendEmail(objData);
            if(result.result)
            {
                await Task.CompletedTask;
                return;
            }
        }
    }
}
