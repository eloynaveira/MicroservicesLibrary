using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaServicios.Api.CarritoCompra.Servicios.DTOs
{
    public class CarritoDTO
    {
        public int CarritoId { get; set; }

        public DateTime? FechaCreacionSesion { get; set; }

        public List<CarritoDetalleDTO> ListaProductos { get; set; }
    }
}
