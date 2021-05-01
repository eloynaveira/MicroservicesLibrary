using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Servicios
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime FechaCreacionSesion { get; set; }

            public List<string> ProductoLista { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto contexto;

            public Manejador(CarritoContexto contexto)
            {
                this.contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritocompra = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };

                contexto.CarritoSesion.Add(carritocompra);
                var result = await contexto.SaveChangesAsync();

                if (result == 0)
                    throw new Exception("Error al registrar un nuevo carrito de compras");

                int id = carritocompra.CarritoSesionId;

                foreach(var item in request.ProductoLista)
                {
                    var detallesesion = new CarritoSesionDetalle
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = id,
                        ProductoSeleccionado = item
                    };
                    contexto.CarritoSesionDetalle.Add(detallesesion);
                }

                result = await contexto.SaveChangesAsync();

                if (result > 0)
                    return Unit.Value;

                throw new Exception("Error al registrar el detalle del carrito");
            }
        }
    }
}
