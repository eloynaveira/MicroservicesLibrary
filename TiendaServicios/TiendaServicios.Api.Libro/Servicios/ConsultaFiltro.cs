using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using TiendaServicios.Api.Libro.Servicios.DTOs;

namespace TiendaServicios.Api.Libro.Servicios
{
    public class ConsultaFiltro
    {
        public class LibroUnico : IRequest<LibreriaMaterialDTO>
        {
            public Guid? LibroId { get; set; }
        }

        public class Manejador : IRequestHandler<LibroUnico, LibreriaMaterialDTO>
        {
            private readonly ContextoLibreria contexto;
            private readonly IMapper mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                this.contexto = contexto;
                this.mapper = mapper;
            }

            public async Task<LibreriaMaterialDTO> Handle(LibroUnico request, CancellationToken cancellationToken)
            {
                var libro = await contexto.LibreriaMaterial.Where(x => x.LibreriaMaterialId == request.LibroId).FirstOrDefaultAsync();

                if (libro == null)
                    throw new Exception("Error al recuperar el libro");

                var libroDto = mapper.Map<LibreriaMaterial, LibreriaMaterialDTO>(libro);
                return libroDto;
            }
        }
    }
}
