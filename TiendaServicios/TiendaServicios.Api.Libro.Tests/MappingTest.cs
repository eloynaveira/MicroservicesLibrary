using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Servicios.DTOs;

namespace TiendaServicios.Api.Libro.Tests
{
    public class MappingTest : Profile
    {
        public MappingTest()
        {
            CreateMap<LibreriaMaterial, LibreriaMaterialDTO>();
        }
    }
}
