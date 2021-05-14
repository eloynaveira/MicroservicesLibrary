using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Lib.SendGridLib.Interface;
using TiendaServicios.Mensajeria.Lib.SendGridLib.Model;

namespace TiendaServicios.Mensajeria.Lib.SendGridLib.Impl
{
    public class SendGridEmail : ISendGridEmail
    {
        public async Task<(bool result, string errorMessage)> SendEmail(SendGridData data)
        {
            try
            {
                var sendGridClient = new SendGridClient(data.SendGridAPIKey);
                var destination = new EmailAddress(data.EmailDestination, data.NameDestination);
                var emailTitle = data.EmailTitle;
                var sender = new EmailAddress("eloyguisamo@gmail.com", "Eloy Prueba");
                var contentMessage = data.Content;

                var objMessage = MailHelper.CreateSingleEmail(sender, destination, emailTitle, contentMessage, contentMessage);

                await sendGridClient.SendEmailAsync(objMessage);

                return (true, null);
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
            
        }
    }
}
