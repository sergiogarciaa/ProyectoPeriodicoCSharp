namespace PeriodicoCSharp.Servicios
{
    public interface InterfazEmail
    {
        /// <summary>
        /// Envía un correo electrónico de recuperación de contraseña.
        /// </summary>
        /// <param name="emailDestino">Dirección de correo electrónico del usuario destinatario.</param>
        /// <param name="nombreUsuario">Nombre del usuario para mostrarlo en el email enviado.</param>
        /// <param name="token">Token asociado a la recuperación.</param>
        void EnviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token);

        /// <summary>
        /// Envía un correo electrónico de confirmación de nueva cuenta de usuario.
        /// </summary>
        /// <param name="emailDestino">Dirección de correo electrónico del usuario destinatario.</param>
        /// <param name="nombreUsuario">Nombre del usuario para mostrarlo en el email enviado.</param>
        /// <param name="token">Token asociado a la confirmación.</param>
        void EnviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token);
    }
}
