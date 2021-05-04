using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.Servicios.DTOs;

namespace TiendaServicios.Api.CarritoCompra.Servicios
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDTO>
        {
            public int CarritoSesionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDTO>
        {
            private readonly CarritoContexto contexto;
            private readonly ILibrosService service;

            public Manejador(CarritoContexto _contexto, ILibrosService _service)
            {
                contexto = _contexto;
                service = _service;
            }

            public async Task<CarritoDTO> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await contexto.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle = await contexto.CarritoSesionDetalle.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                var listaCarritoDTO = new List<CarritoDetalleDTO>();

                foreach(var libro in carritoSesionDetalle)
                {
                    var response = await service.GetLibro(new Guid(libro.ProductoSeleccionado));
                    if(response.resultado)
                    {
                        var objLibro = response.libro;
                        var carritoDetalle = new CarritoDetalleDTO
                        {
                            TituloLibro = objLibro.Titulo,
                            FechaPublicacion = objLibro.FechaPublicacion,
                            LibroId = objLibro.LibreriaMaterialId
                        };
                        listaCarritoDTO.Add(carritoDetalle);
                    }
                }
                var carritoSesionDTO = new CarritoDTO
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = listaCarritoDTO
                };

                return carritoSesionDTO;
            }
        }
    }
}
