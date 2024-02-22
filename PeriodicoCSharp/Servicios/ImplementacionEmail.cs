using PeriodicoCSharp.Utils;
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
                Credentials = new System.Net.NetworkCredential("sergiopeke6@gmail.com", "")
            };
        }

        /// <summary>
        /// Envía un correo electrónico de confirmación de cuenta.
        /// </summary>
        /// <param name="emailDestino">Correo electrónico del destinatario.</param>
        /// <param name="nombreUsuario">Nombre del usuario destinatario.</param>
        /// <param name="token">Token de confirmación de cuenta.</param>
        public void EnviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método EnviarEmailConfirmacion() en ImplementacionEmail");

                string urlDominio = "http://localhost:5105";
                string urlDeRecuperacion = $"{urlDominio}/auth/confirmacionCorreo/?token={token}";

                string htmlContent = $@"<!DOCTYPE html>
        <html lang='es'>
        <body>
        <div style='width: 600px; padding: 20px; border: 2px solid black; border-radius: 13px; background-color: #fff; font-family: Sans-serif;'>
        <h1 style='color:rgb(192, 192, 192)'>Confirmar cuenta - <b style='color:#285845; text-decoration: underline'>La Revista</b></h1>
        <p style='margin-bottom:25px'>Estimad@&nbsp;<b>{nombreUsuario}</b>:</p> <p style='margin-bottom:25px'>
        Bienvenid@ al Periodico La Revista. Para confirmar tu cuenta, haz click en el botón:</p>
        <a style='padding: 10px 15px;border-radius: 20px; background-color: #285845; color: white; text-decoration: none' href='{urlDeRecuperacion}' target='_blank'>Confirmar cuenta</a>
        <p style='margin-top:25px'>¡Ahora somos uno más!.</p> </div> </body> </html>";

                MailMessage mensajeDelCorreo = new MailMessage("recuperacionpass12@gmail.com", emailDestino, "CONFIRMAR EMAIL", htmlContent)
                {
                    IsBodyHtml = true
                };

                _smtpClient.Send(mensajeDelCorreo);
            }
            catch (Exception ex)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionEmail - EnviarEmailConfirmacion()] Ha ocurrido un error al enviar el email! {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Envía un correo electrónico de recuperación de contraseña.
        /// </summary>
        /// <param name="emailDestino">Correo electrónico del destinatario.</param>
        /// <param name="nombreUsuario">Nombre del usuario destinatario.</param>
        /// <param name="token">Token de recuperación de contraseña.</param>
        public void EnviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método EnviarEmailRecuperacion() en ImplementacionEmail");

                string urlDominio = "http://localhost:5105";
                string urlDeRecuperacion = $"{urlDominio}/auth/recuperar/?token={token}";

                string cuerpoMensaje = $@"<!DOCTYPE html>
        <html lang='es'>
        <body>
        <div style='width: 600px; padding: 20px; border: 2px solid black; border-radius: 13px; background-color: #fff; font-family: Sans-serif;'>
        <h1 style='color:rgb(192, 192, 192)'>Restablecer contraseña - <b style='color:#285845; text-decoration: underline'>La Revista</b></h1>
        <p style='margin-bottom:25px'>Estimad@&nbsp;<b>{nombreUsuario}</b>:</p> <p style='margin-bottom:25px'>
        Se ha enviado este correo para restablecer la contraseña de su cuenta. Haga click en el botón para proseguir. Si usted no ha solicitado este cambio, ignore este mensaje.</p>
        <a style='padding: 10px 15px; border-radius: 20px; background-color: #285845; color: white; text-decoration: none' href='{urlDeRecuperacion}' target='_blank'>Cambiar contraseña</a>
        <p style='margin-top:25px'>¡Gracias por confiar en nosotros!.</p> </div> </body> </html>";

                MailMessage mensaje = new MailMessage("sergiopeke6@gmail.com", emailDestino, "Recuperación de contraseña - La Revista", cuerpoMensaje)
                {
                    IsBodyHtml = true
                };

                _smtpClient.Send(mensaje);
            }
            catch (Exception ex)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionEmail - EnviarEmailRecuperacion()] Ha ocurrido un error al enviar el email! {ex.Message}");
                throw;
            }
        }
    }
}
