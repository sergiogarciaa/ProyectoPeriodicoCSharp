using PeriodicoCSharp.DTO;
using DAL.Entidades;

namespace PeriodicoCSharp.Servicios
{
    public interface UsuarioServicio
    {
        /// <summary>
        /// Se registra a un usuario antes comprobando si ya se encuentra en la BBDD registrado el usuario
        /// </summary>
        /// <param name="userDTO">El usuario a registrar</param>
        /// <returns>El usuario registrado</returns>
        UsuarioDTO Registrar(UsuarioDTO userDTO);

        /// <summary>
        /// Busca a un usuario por su identificador asignado en la bbdd
        /// </summary>
        /// <param name="id">ID del usuario a buscar</param>
        /// <returns>El usuario buscado</returns>
        Usuario BuscarPorId(long id);

        /// <summary>
        /// Busca a un usuario por su dirección de email registrada
        /// </summary>
        /// <param name="email">Email del usuario que se quiere encontrar</param>
        /// <returns>El usuario buscado</returns>
        UsuarioDTO BuscarPorEmail(string email);

        /// <summary>
        /// Busca a un usuario por su dni
        /// </summary>
        /// <param name="dni">DNI del usuario que se quiere encontrar</param>
        /// <returns>True si el usuario existe, false en caso contrario</returns>
        bool BuscarPorDni(string dni);

        /// <summary>
        /// Busca todos los usuarios registrados
        /// </summary>
        /// <returns>La lista de todos los usuarios DTO</returns>
        List<UsuarioDTO> BuscarTodos();

        /// <summary>
        /// Busca un usuario por su token de recuperación.
        /// </summary>
        /// <param name="token">Token que identifica al usuario recibió un correo con la URL y dicho token</param>
        /// <returns>El usuario buscado</returns>
        UsuarioDTO ObtenerUsuarioPorToken(string token);

        /// <summary>
        /// Inicia el proceso de recuperación (generando token y vencimiento) con el email del usuario 
        /// </summary>
        /// <param name="emailUsuario">El email del usuario a recuperar la contraseña</param>
        /// <returns>True si el proceso se ha iniciado correctamente, false en caso contrario</returns>
        bool IniciarResetPassConEmail(string emailUsuario);

        /// <summary>
        /// Establece la nueva contraseña del usuario con el token
        /// </summary>
        /// <param name="usuario">El usuario al que se le establecerá la nueva contraseña</param>
        /// <returns>True si el proceso se ha realizado correctamente, false en caso contrario</returns>
        bool ModificarContraseñaConToken(UsuarioDTO usuario);

        /// <summary>
        /// Elimina un usuario por su identificador
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>El usuario eliminado o null si no existe</returns>
        Usuario Eliminar(long id);

        /// <summary>
        /// Actualiza un usuario
        /// </summary>
        /// <param name="usuarioDTO">El DTO del usuario a actualizar</param>
        void ActualizarUsuario(UsuarioDTO usuarioDTO);


        /// <summary>
        /// Verifica las credenciales de un usuario.
        /// </summary>
        /// <param name="emailUsuario">Correo electrónico del usuario.</param>
        /// <param name="claveUsuario">Clave de acceso del usuario.</param>
        /// <returns>Booleano que indica si las credenciales son válidas.</returns>
        bool verificarCredenciales(string emailUsuario, string claveUsuario);

        /// <summary>
        /// Método para buscar un usuario por ID.
        /// </summary>
        /// <param name="id">Identificador del usuario.</param>
        /// <returns>El DTO del usuario encontrado o null si no existe.</returns>
        UsuarioDTO BuscarDtoPorId(long id);

        /// <summary>
        /// Comprueba si el usuario existe y si su cuenta ha sido confirmada
        /// </summary>
        /// <param name="email">El email del usuario</param>
        /// <returns>True si el usuario existe y su cuenta ha sido confirmada, false en caso contrario</returns>
        bool EstaLaCuentaConfirmada(string email);

        /// <summary>
        /// Confirma la cuenta de usuario mediante el token proporcionado
        /// </summary>
        /// <param name="token">El token de confirmación</param>
        /// <returns>True si la cuenta se confirmó correctamente, false en caso contrario</returns>
        bool ConfirmarCuenta(string token);
        public Usuario BuscarPorEmailDAO(string? name);
    }
}
