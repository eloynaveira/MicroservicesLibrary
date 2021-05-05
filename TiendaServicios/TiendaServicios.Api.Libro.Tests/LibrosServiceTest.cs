using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using TiendaServicios.Api.Libro.Servicios;
using TiendaServicios.Api.Libro.Servicios.DTOs;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {
        private IEnumerable<LibreriaMaterial> ObtenerDatosPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
        }

        private Mock<ContextoLibreria> CrearContexto()
        {
            var datosPrueba = ObtenerDatosPrueba().AsQueryable();

            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(datosPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(datosPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(datosPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(datosPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(datosPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(datosPrueba.Provider));

            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
        }

        [Fact]
        public async void GetLIbroIdAsync()
        {
            var mockContexto = CrearContexto();
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });
            var mapper = mapConfig.CreateMapper();

            var request = new ConsultaFiltro.LibroUnico();
            request.LibroId = Guid.Empty;

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);

            var libro = await manejador.Handle(request, new CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);
        }

        [Fact]
        public async void GetLibrosAsync()
        {
            System.Diagnostics.Debugger.Launch();

            var mockContexto = CrearContexto();
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });
            var mapper = mapConfig.CreateMapper();
            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();

            var lista = await manejador.Handle(request, new CancellationToken());

            Assert.True(lista.Any());
        }
    }
}
