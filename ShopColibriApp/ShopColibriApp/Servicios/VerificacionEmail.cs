using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
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
                    mailMessage.To.Add(receptor);
                    mailMessage.Subject = asunto;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.From = new MailAddress("verificacionesshopcolibri@gmail.com", "ShopColibri");

                    using (SmtpClient cliente =  new SmtpClient())
                    {
                        cliente.UseDefaultCredentials = false;
                        cliente.Credentials = new NetworkCredential("verificacionesshopcolibri@gmail.com", "zbtcojcccaclfzaq");
                        cliente.Port = 587;
                        cliente.EnableSsl = true;

                        cliente.Host = "smtp.gmail.com";
                        cliente.Send(mailMessage);
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
