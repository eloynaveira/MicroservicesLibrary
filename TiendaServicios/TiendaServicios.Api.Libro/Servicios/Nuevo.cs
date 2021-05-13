using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using TiendaServicios.RabbitMQ.Lib.BusRabbit;
using TiendaServicios.RabbitMQ.Lib.EventQueue;

namespace TiendaServicios.Api.Libro.Servicios
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime? FechaPublicacion { get; set; }

            public string Titulo { get; set; }

            public Guid? AutorLibro { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoLibreria contexto;

            private readonly IRabbitEventBus eventBus;

            public Manejador(ContextoLibreria contexto, IRabbitEventBus eventBus)
            {
                this.contexto = contexto;
                this.eventBus = eventBus;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libro = new LibreriaMaterial
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro
                };

                contexto.LibreriaMaterial.Add(libro);

                var value = await contexto.SaveChangesAsync();

                eventBus.Publish(new EmailEventQueue("eloy@prueba.es", request.Titulo, "Email de prueba"));

                if (value > 0)
                    return Unit.Value;

                throw new Exception("Error al registrar el nuevo libro");
            }
        }
    }
}
