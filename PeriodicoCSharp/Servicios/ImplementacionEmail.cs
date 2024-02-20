using System.Net.Mail;

namespace PeriodicoCSharp.Servicios
{
    public class ImplementacionEmail : InterfazEmail
    {
        private readonly SmtpClient _smtpClient;
        public void EnviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            { 

                string urlDominio = "https://localhost:7142";

                string EmailOrigen = "recuperacionpass12@gmail.com";
                //Se crea la URL de recuperación con el token que se enviará al mail del user.
                string urlDeRecuperacion = String.Format("{0}/auth/confirmar-cuenta/?token={1}", urlDominio, token);

                //Hacemos que el texto del email sea un archivo html que se encuentra en la carpeta Plantilla.
                string htmlContent = $@"﻿<!DOCTYPE html>
                <html lang='es'>
                <body>
                <div style='width: 600px; padding: 20px; border: 2px solid black; border-radius: 13px; background-color: #fff; font-family: Sans-serif;'>
                <h1 style='color:rgb(192, 192, 192)'>Confirmar cuenta - <b style='color:#285845; text-decoration: underline'>La Revista</b></h1>
                <p style='margin-bottom:25px'>Estimad@&nbsp;<b>{nombreUsuario}</b>:</p> <p style='margin-bottom:25px'>
                Bienvenid@ al Periodico La Revista. Para confirmar tu cuenta, haga click en el botón:</p>
                <a style='padding: 10px 15px; border-radius: 10px; background-color: #5993d3; color: white; text-decoration: none' href='' target='_blank'>Confirmar cuenta</a>
                <p style='margin-top:25px'>¡Ahora somos uno más!.</p> </div> </body> </html>";
                //Asignamos el nombre de usuario que tendrá el cuerpo del mail y el URL de recuperación con el token al HTML.
                htmlContent = String.Format(htmlContent, nombreUsuario, urlDeRecuperacion);

                MailMessage mensajeDelCorreo = new MailMessage(EmailOrigen, emailDestino, "CONFIRMAR EMAIL", htmlContent);

                mensajeDelCorreo.IsBodyHtml = true;

                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, "");

                smtpCliente.Send(mensajeDelCorreo);

                smtpCliente.Dispose();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error EmailServicioImpl - EnviarEmailConfirmacion()] Ha ocurrido un error al enviar el email! {ex.Message}");
            }
        }

        public void EnviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                MailMessage mensaje = new MailMessage();
                mensaje.From = new MailAddress("recuperacionpasss12@gmail.com");
                mensaje.To.Add(emailDestino);
                mensaje.Subject = "Recuperación de contraseña. La Revista";

                string urlDominio = "http://localhost:8080";
                string urlDeRecuperacion = $"{urlDominio}/auth/recuperar?token={token}";

                string cuerpoMensaje = $@"﻿<!DOCTYPE html>
                <html lang='es'>
                <body>
                <div style='width: 600px; padding: 20px; border: 2px solid black; border-radius: 13px; background-color: #fff; font-family: Sans-serif;'>
                <h1 style='color:rgb(192, 192, 192)'>Restablecer contraseña. - <b style='color:#285845; text-decoration: underline'>La Revista</b></h1>
                <p style='margin-bottom:25px'>Estimad@&nbsp;<b>{nombreUsuario}</b>:</p> <p style='margin-bottom:25px'>
                Se ha enviado este correo para restablecer la contraseña de su cuenta. Haga click en el botón para proseguir. Si usted no ha solicitado este cambio, ignore este mensaje.</p>
                <a style='padding: 10px 15px; border-radius: 20px; background-color: #285845; color: white; text-decoration: none' href='{urlDeRecuperacion}' target='_blank'>Cambiar contraseña</a>
                <p style='margin-top:25px'>¡Gracías por confiar en nosotros!.</p> </div> </body> </html>";

                mensaje.Body = cuerpoMensaje;
                mensaje.IsBodyHtml = true;

                _smtpClient.Send(mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error EmailServicioImpl - EnviarEmailRecuperacion()] Ha ocurrido un error al enviar el email! {ex.Message}");
            }
        }
    }
}
