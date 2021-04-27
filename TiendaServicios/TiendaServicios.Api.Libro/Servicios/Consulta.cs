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
using static TiendaServicios.Api.Libro.Servicios.Nuevo;

namespace TiendaServicios.Api.Libro.Servicios
{
    public class Consulta
    {
        public class Ejecuta : IRequest<List<LibreriaMaterialDTO>> { }

        public class Manejador : IRequestHandler<Ejecuta, List<LibreriaMaterialDTO>>
        {
            private readonly ContextoLibreria contexto;
            private readonly IMapper mapper;

            public Manejador(ContextoLibreria contexto, IMapper mapper)
            {
                this.contexto = contexto;
                this.mapper = mapper;
            }

            public async Task<List<LibreriaMaterialDTO>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libros = await contexto.LibreriaMaterial.ToListAsync();
                var librosDTO = mapper.Map<List<LibreriaMaterial>, List<LibreriaMaterialDTO>>(libros);

                return librosDTO;
            }
        }
    }
}
