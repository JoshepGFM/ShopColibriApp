using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Xamarin.Forms;
using System.Net.Mail;
using System.Net;

namespace ShopColibriApp.Servicios
{
    public class VerificacionEmail
    {
        public bool Index( string receptor, string asunto, string body)
        {
            bool R = false;
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.To.Add(receptor);//a quien se le envía el correo
                    mailMessage.Subject = asunto;//el asunto que tiene a fin en correo
                    mailMessage.Body = body;//el mensaje
                    mailMessage.IsBodyHtml = true;//establese si se tiene que usar html
                    mailMessage.From = new MailAddress("verificacionesshopcolibri@gmail.com", "ShopColibri");//le coloca un sobre nombre a correo con que se envia

                    using (SmtpClient cliente =  new SmtpClient())
                    {
                        cliente.UseDefaultCredentials = false;
                        cliente.Credentials = new NetworkCredential("verificacionesshopcolibri@gmail.com", "zbtcojcccaclfzaq");//Añade las credenciales para el ingreso
                        cliente.Port = 587;
                        cliente.EnableSsl = true;

                        cliente.Host = "smtp.gmail.com";
                        cliente.Send(mailMessage);//Envía el correo del que se preparo
                        R = true;
                    }
                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return R;
        }
    }
}
