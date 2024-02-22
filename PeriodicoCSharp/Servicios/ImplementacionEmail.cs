using System;
using System.Net.Mail;

namespace PeriodicoCSharp.Servicios
{
    public class ImplementacionEmail : InterfazEmail
    {
        private readonly SmtpClient _smtpClient;

        public ImplementacionEmail()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("recuperacionpass12@gmail.com", "")
            };
        }

        public void EnviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                string urlDominio = "https://localhost:7142";
                string urlDeRecuperacion = $"{urlDominio}/auth/confirmar-cuenta/?token={token}";

                string htmlContent = $@"<!DOCTYPE html>
                <html lang='es'>
                <body>
                <div style='width: 600px; padding: 20px; border: 2px solid black; border-radius: 13px; background-color: #fff; font-family: Sans-serif;'>
                <h1 style='color:rgb(192, 192, 192)'>Confirmar cuenta - <b style='color:#285845; text-decoration: underline'>La Revista</b></h1>
                <p style='margin-bottom:25px'>Estimad@&nbsp;<b>{nombreUsuario}</b>:</p> <p style='margin-bottom:25px'>
                Bienvenid@ al Periodico La Revista. Para confirmar tu cuenta, haz click en el botón:</p>
                <a style='padding: 10px 15px; border-radius: 10px; background-color: #5993d3; color: white; text-decoration: none' href='{urlDeRecuperacion}' target='_blank'>Confirmar cuenta</a>
                <p style='margin-top:25px'>¡Ahora somos uno más!.</p> </div> </body> </html>";

                MailMessage mensajeDelCorreo = new MailMessage("recuperacionpass12@gmail.com", emailDestino, "CONFIRMAR EMAIL", htmlContent)
                {
                    IsBodyHtml = true
                };

                _smtpClient.Send(mensajeDelCorreo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error ImplementacionEmail - EnviarEmailConfirmacion()] Ha ocurrido un error al enviar el email! {ex.Message}");
            }
        }

        public void EnviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                string urlDominio = "http://localhost:8080";
                string urlDeRecuperacion = $"{urlDominio}/auth/recuperar?token={token}";

                string cuerpoMensaje = $@"<!DOCTYPE html>
                <html lang='es'>
                <body>
                <div style='width: 600px; padding: 20px; border: 2px solid black; border-radius: 13px; background-color: #fff; font-family: Sans-serif;'>
                <h1 style='color:rgb(192, 192, 192)'>Restablecer contraseña - <b style='color:#285845; text-decoration: underline'>La Revista</b></h1>
                <p style='margin-bottom:25px'>Estimad@&nbsp;<b>{nombreUsuario}</b>:</p> <p style='margin-bottom:25px'>
                Se ha enviado este correo para restablecer la contraseña de su cuenta. Haga click en el botón para proseguir. Si usted no ha solicitado este cambio, ignore este mensaje.</p>
                <a style='padding: 10px 15px; border-radius: 20px; background-color: #285845; color: white; text-decoration: none' href='{urlDeRecuperacion}' target='_blank'>Cambiar contraseña</a>
                <p style='margin-top:25px'>¡Gracias por confiar en nosotros!.</p> </div> </body> </html>";

                MailMessage mensaje = new MailMessage("recuperacionpass12@gmail.com", emailDestino, "Recuperación de contraseña - La Revista", cuerpoMensaje)
                {
                    IsBodyHtml = true
                };

                _smtpClient.Send(mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error ImplementacionEmail - EnviarEmailRecuperacion()] Ha ocurrido un error al enviar el email! {ex.Message}");
            }
        }
    }
}
