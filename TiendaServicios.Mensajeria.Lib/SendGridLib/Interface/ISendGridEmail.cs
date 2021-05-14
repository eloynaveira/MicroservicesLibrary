using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Lib.SendGridLib.Model;

namespace TiendaServicios.Mensajeria.Lib.SendGridLib.Interface
{
    public interface ISendGridEmail
    {
        Task<(bool result, string errorMessage)> SendEmail(SendGridData data);
    }
}
