using AutoMapper;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Servicios.DTOs;

namespace TiendaServicios.Api.Libro.Servicios
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LibreriaMaterial, LibreriaMaterialDTO>();
        }
    }
}
