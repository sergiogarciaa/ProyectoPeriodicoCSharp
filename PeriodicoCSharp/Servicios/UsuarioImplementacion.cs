using System;
using DAL.Entidades;
using Microsoft.AspNetCore.Identity;

using PeriodicoCSharp.DTO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PeriodicoCSharp.Utils;
using System.Text.RegularExpressions;

namespace PeriodicoCSharp.Servicios
{
    public class ImplementacionUsuario : UsuarioServicio
    {
        private readonly PeriodicoContext _contexto;
        private readonly InterfazEncriptar _servicioEncriptar;
        private readonly ConversionDao _toDao;
        private readonly ConversionDTO _toDto;
        private readonly InterfazEmail _emailServicio;

        public ImplementacionUsuario(PeriodicoContext contexto, InterfazEncriptar servicioEncriptar, ConversionDao toDao,
                                     ConversionDTO toDto, InterfazEmail emailServicio)
        {
            _contexto = contexto;
            _servicioEncriptar = servicioEncriptar;
            _toDao = toDao;
            _toDto = toDto;
            _emailServicio = emailServicio;
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="userDto">DTO del usuario a registrar.</param>
        /// <returns>DTO del usuario registrado.</returns>
        public UsuarioDTO Registrar(UsuarioDTO userDto)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método Registrar() en ImplementacionUsuario");

                var usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == userDto.EmailUsuario && !u.CuentaConfirmada);

                if (usuarioExistente != null)
                {
                    userDto.EmailUsuario = "EmailNoConfirmado";
                    return userDto;
                }

                var emailExistente = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == userDto.EmailUsuario && u.CuentaConfirmada);

                if (emailExistente != null)
                {
                    userDto.EmailUsuario = "EmailRepetido";
                    return userDto;
                }

                if (userDto.NombreUsuario != null &&
                    userDto.ApellidosUsuario != null&&
                    userDto.DniUsuario != null &&
                    userDto.EmailUsuario != null &&
                    userDto.TlfUsuario != null &&
                    userDto.ClaveUsuario != null)
                {
                    bool emailValido = Regex.IsMatch(userDto.EmailUsuario, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
                    bool dniValido = Regex.IsMatch(userDto.DniUsuario, @"^\d{8}[a-zA-Z]$");
                    bool telefonoValido = Regex.IsMatch(userDto.TlfUsuario, @"^\d{9}$");

                    if (emailValido && dniValido && telefonoValido)
                    {
                        userDto.ClaveUsuario = _servicioEncriptar.Encriptar(userDto.ClaveUsuario);
                        Usuario usuarioDao = _toDao.usuarioToDao(userDto);
                        usuarioDao.FchAltaUsuario = DateTime.Now;
                        usuarioDao.Rol = "ROLE_1";
                        string token = generarToken();
                        usuarioDao.TokenRecuperacion = token;

                        _contexto.Usuarios.Add(usuarioDao);
                        _contexto.SaveChanges();

                        string nombreUsuario = usuarioDao.NombreUsuario + " " + usuarioDao.ApellidosUsuario;
                        _emailServicio.EnviarEmailConfirmacion(userDto.EmailUsuario, nombreUsuario, token);

                        return userDto;
                    }
                    else
                    {
                        userDto.EmailUsuario = "NoValido";
                        return userDto;
                    }
                }
                else
                {
                    userDto.EmailUsuario = "NoValido";
                    return userDto;
                }



            }
            catch (ArgumentException ae)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - Registrar()] Argumento no válido al registrar usuario {ae.Message}");
                return null;
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - Registrar()] Error al registrar usuario {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Verifica si la cuenta asociada al correo electrónico está confirmada.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <returns>True si la cuenta está confirmada, False si no lo está o si ocurre un error.</returns>
        public bool EstaLaCuentaConfirmada(string email)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método EstaLaCuentaConfirmada() en ImplementacionUsuario");

                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == email);
                return usuario != null && usuario.CuentaConfirmada;
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - EstaLaCuentaConfirmada()] Error al comprobar si la cuenta ha sido confirmada {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Confirma la cuenta de usuario utilizando un token de confirmación.
        /// </summary>
        /// <param name="token">Token de confirmación de cuenta.</param>
        /// <returns>True si la cuenta se confirma correctamente, False si la cuenta no existe o ya está confirmada.</returns>
        public bool ConfirmarCuenta(string token)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método ConfirmarCuenta() en ImplementacionUsuario");

                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);

                if (usuarioExistente != null && !usuarioExistente.CuentaConfirmada)
                {
                    usuarioExistente.CuentaConfirmada = true;
                    _contexto.SaveChanges();
                    return true;
                }
                else
                {
                    Log.escribirEnFicheroLog("La cuenta no existe o ya está confirmada");
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - ConfirmarCuenta()] Error al confirmar la cuenta {ae.Message}");
                return false;
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - ConfirmarCuenta()] Error de persistencia al confirmar la cuenta {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Inicia el proceso de restablecimiento de contraseña utilizando el correo electrónico del usuario.
        /// </summary>
        /// <param name="emailUsuario">Correo electrónico del usuario.</param>
        /// <returns>True si el proceso se inicia correctamente, False si el usuario no existe.</returns>
        public bool IniciarResetPassConEmail(string emailUsuario)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método IniciarResetPassConEmail() en ImplementacionUsuario");

                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == emailUsuario);

                if (usuarioExistente != null)
                {
                    // Generar el token y establecer la fecha de expiración
                    string token = generarToken();
                    DateTime fechaExpiracion = DateTime.Now.AddMinutes(1);

                    // Actualizar el usuario con el nuevo token y la fecha de expiración
                    usuarioExistente.TokenRecuperacion = token;
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    // Enviar el correo de recuperación
                    string nombreUsuario = $"{usuarioExistente.NombreUsuario} {usuarioExistente.ApellidosUsuario}";
                    _emailServicio.EnviarEmailRecuperacion(emailUsuario, nombreUsuario, token);

                    return true;
                }
                else
                {
                     Log.escribirEnFicheroLog($"El usuario con email {emailUsuario} no existe");
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - IniciarResetPassConEmail()] {ae.Message}");
                return false;
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - IniciarResetPassConEmail()] {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Modifica la contraseña del usuario utilizando el token de recuperación.
        /// </summary>
        /// <param name="usuario">DTO del usuario con el token de recuperación y la nueva contraseña.</param>
        /// <returns>True si la contraseña se modifica correctamente, False si no se encuentra el usuario o si ocurre un error.</returns>
        public bool ModificarContraseñaConToken(UsuarioDTO usuario)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método ModificarContraseñaConToken() en ImplementacionUsuario");

                Usuario usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == usuario.TokenRecuperacion);

                if (usuarioExistente != null)
                {
                    string nuevaContraseña = _servicioEncriptar.Encriptar(usuario.ClaveUsuario);
                    usuarioExistente.ClaveUsuario = nuevaContraseña;
                    usuarioExistente.TokenRecuperacion = null; // Se establece como null para invalidar el token ya consumido al cambiar la contraseña
                    _contexto.Usuarios.Update(usuarioExistente);
                    _contexto.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - ModificarContraseñaConToken()] {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Genera un token de seguridad.
        /// </summary>
        /// <returns>Token generado.</returns>
        private string generarToken()
        {
            try
            {
                 Log.escribirEnFicheroLog("Generando token de seguridad");

                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {
                    byte[] tokenBytes = new byte[30];
                    rng.GetBytes(tokenBytes);

                    return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
                }
            }
            catch (ArgumentException ae)
            {
                Log.escribirEnFicheroLog($"[Error UsuarioServicioImpl - generarToken()] Error al generar un token de usuario {ae.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene un usuario por su token de recuperación.
        /// </summary>
        /// <param name="token">Token de recuperación del usuario.</param>
        /// <returns>DTO del usuario si existe, de lo contrario, null.</returns>
        public UsuarioDTO ObtenerUsuarioPorToken(string token)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método ObtenerUsuarioPorToken() en ImplementacionUsuario");

                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);
                if (usuarioExistente != null)
                {
                    return _toDto.usuarioToDto(usuarioExistente);
                }
                else
                {
                     Log.escribirEnFicheroLog($"No existe el usuario con el token {token}");
                    return null;
                }
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - ObtenerUsuarioPorToken()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Verifica las credenciales de inicio de sesión de un usuario.
        /// </summary>
        /// <param name="emailUsuario">Correo electrónico del usuario.</param>
        /// <param name="claveUsuario">Contraseña del usuario.</param>
        /// <returns>True si las credenciales son válidas y la cuenta está confirmada, False de lo contrario.</returns>
        public bool verificarCredenciales(string emailUsuario, string claveUsuario)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método verificarCredenciales() en ImplementacionUsuario");

                string contraseñaEncriptada = _servicioEncriptar.Encriptar(claveUsuario);
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == emailUsuario && u.ClaveUsuario == contraseñaEncriptada);
                if (usuarioExistente == null)
                {
                     Log.escribirEnFicheroLog($"Credenciales inválidas para el usuario con email {emailUsuario}");
                    return false;
                }
                if (!usuarioExistente.CuentaConfirmada)
                {
                     Log.escribirEnFicheroLog($"La cuenta del usuario con email {emailUsuario} no está confirmada");
                    return false;
                }

                return true;
            }
            catch (ArgumentNullException e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - verificarCredenciales()] {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Busca un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <returns>DTO del usuario si existe, de lo contrario, null.</returns>
        public UsuarioDTO BuscarPorEmail(string email)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método BuscarPorEmail() en ImplementacionUsuario");

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                var usuario = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == email);

                if (usuario != null)
                {
                    usuarioDTO = _toDto.usuarioToDto(usuario);
                }

                return usuarioDTO;
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - BuscarPorEmail()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns>Usuario eliminado si existe, de lo contrario, null.</returns>
        public Usuario Eliminar(long id)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método Eliminar() en ImplementacionUsuario");

                Usuario? usuario = _contexto.Usuarios.Find(id);
                if (usuario != null)
                {
                    _contexto.Usuarios.Remove(usuario);
                    _contexto.SaveChanges();
                     Log.escribirEnFicheroLog("Usuario eliminado con éxito");
                }
                return usuario;
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - Eliminar()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Actualiza la información de un usuario.
        /// </summary>
        /// <param name="usuarioDTO">DTO del usuario con la información actualizada.</param>
        public void ActualizarUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método ActualizarUsuario() en ImplementacionUsuario");

                Usuario? usuarioActual = _contexto.Usuarios.Find(usuarioDTO.Id);
                if (usuarioActual != null)
                {
                    usuarioActual.NombreUsuario = usuarioDTO.NombreUsuario;
                    usuarioActual.ApellidosUsuario = usuarioDTO.ApellidosUsuario;
                    usuarioActual.TlfUsuario = usuarioDTO.TlfUsuario;
                    usuarioActual.DniUsuario = usuarioDTO.DniUsuario;
                    usuarioActual.Rol = usuarioDTO.Rol;
                    _contexto.Usuarios.Update(usuarioActual);
                    _contexto.SaveChanges();
                }
            }
            catch (ArgumentException ae)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - ActualizarUsuario()] {ae.Message}");
            }
        }

        /// <summary>
        /// Busca un usuario por su ID y devuelve su DTO.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>DTO del usuario si existe, de lo contrario, null.</returns>
        public UsuarioDTO BuscarDtoPorId(long id)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método BuscarDtoPorId() en ImplementacionUsuario");

                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                if (usuario != null)
                {
                    return _toDto.usuarioToDto(usuario);
                }
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - BuscarDtoPorId()] {e.Message}");
            }
            return null;
        }

        /// <summary>
        /// Busca todos los usuarios y los devuelve como una lista de DTOs.
        /// </summary>
        /// <returns>Lista de DTOs de usuarios.</returns>
        public List<UsuarioDTO> BuscarTodos()
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método BuscarTodos() en ImplementacionUsuario");

                return _toDto.listaUsuarioToDto(_contexto.Usuarios.ToList());
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - BuscarTodos()] {e.Message}");
                return new List<UsuarioDTO>();
            }
        }

        /// <summary>
        /// Busca un usuario por su número de DNI.
        /// </summary>
        /// <param name="dni">Número de DNI del usuario.</param>
        /// <returns>True si el usuario con el DNI dado existe, False de lo contrario.</returns>
        public bool BuscarPorDni(string dni)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método BuscarPorDni() en ImplementacionUsuario");

                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.DniUsuario == dni);
                return usuario != null;
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - BuscarPorDni()] {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Busca un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <returns>Usuario si existe, de lo contrario, null.</returns>
        public Usuario BuscarPorId(long id)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método BuscarPorId() en ImplementacionUsuario");

                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                return usuario;
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - BuscarPorId()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Busca un usuario por su correo electrónico.
        /// </summary>
        /// <param name="name">Correo electrónico del usuario.</param>
        /// <returns>Usuario si existe, de lo contrario, null.</returns>
        public Usuario BuscarPorEmailDAO(string? name)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método BuscarPorEmailDAO() en ImplementacionUsuario");

                var usuario = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == name);
                return usuario;
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionUsuario - BuscarPorEmail()] {e.Message}");
                return null;
            }
        }
    }
}
