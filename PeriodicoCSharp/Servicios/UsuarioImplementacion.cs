using System;
using DAL.Entidades;
using Microsoft.AspNetCore.Identity;

using PeriodicoCSharp.DTO;
using System.Security.Cryptography;
using System.Text;

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

        public UsuarioDTO Registrar(UsuarioDTO userDto)
        {
            try
            {

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
            catch (ArgumentException ae)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - RegistrarAsync()] Argumento no válido al registrar usuario {ae.Message}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - RegistrarAsync()] Error al registrar usuario {e.Message}");
                return null;
            }
        }

        public bool EstaLaCuentaConfirmada(string email)
        {
            try
            {
                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == email);
                return usuario != null && usuario.CuentaConfirmada;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - EstaLaCuentaConfirmada()] Error al comprobar si la cuenta ha sido confirmada {e.Message}");
                return false;
            }
        }

        public bool ConfirmarCuenta(string token)
        {
            try
            {
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);

                if (usuarioExistente != null && !usuarioExistente.CuentaConfirmada)
                {
                    UsuarioDTO usuario = _toDto.usuarioToDto(usuarioExistente);
                    return true;
                }
                else
                {
                    Console.WriteLine("La cuenta no existe o ya está confirmada");
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - ConfirmarCuenta()] Error al confirmar la cuenta {ae.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - ConfirmarCuenta()] Error de persistencia al confirmar la cuenta {e.Message}");
                return false;
            }
        }

        public bool IniciarResetPassConEmail(string emailUsuario)
        {
            try
            {
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
                    Console.WriteLine($"[Error IniciarResetPassConEmailAsync()] El usuario con email {emailUsuario} no existe");
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - IniciarResetPassConEmailAsync()] {ae.Message}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - IniciarResetPassConEmailAsync()] {e.Message}");
                return false;
            }
        }

        private string generarToken()
        {
            try
            {

                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {
                    byte[] tokenBytes = new byte[30];
                    rng.GetBytes(tokenBytes);

                    return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
                }
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("[Error UsuarioServicioImpl -  generarToken()] Error al generar un token de usuario " + ae.Message);
                return null;
            }

        }

        public bool ModificarContraseñaConToken(UsuarioDTO usuario)
        {
            try
            {
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == usuario.TokenRecuperacion);

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
                Console.WriteLine($"[Error ImplementacionUsuario - ModificarContraseñaConTokenAsync()] {e.Message}");
                return false;
            }
        }

        public UsuarioDTO ObtenerUsuarioPorToken(string token)
        {
            try
            {
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.TokenRecuperacion == token);
                if (usuarioExistente != null)
                {
                    return _toDto.usuarioToDto(usuarioExistente);
                }
                else
                {
                    Console.WriteLine($"No existe el usuario con el token {token}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - ObtenerUsuarioPorToken()] {e.Message}");
                return null;
            }
        }

        public bool verificarCredenciales(string emailUsuario, string claveUsuario)
        {
            try
            {
     

                string contraseñaEncriptada = _servicioEncriptar.Encriptar(claveUsuario);
                Usuario? usuarioExistente = _contexto.Usuarios.FirstOrDefault(u => u.EmailUsuario == emailUsuario && u.ClaveUsuario == contraseñaEncriptada);
                if (usuarioExistente == null)
                {
                   
                    return false;
                }
                if (!usuarioExistente.CuentaConfirmada)
                { 
                    return false;
                }

        
                return true;
            }
            catch (ArgumentNullException e)
            {
              
                return false;
            }

        }

        public UsuarioDTO BuscarPorEmail(string email)
        {
            try
            {
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
                Console.WriteLine($"[Error ImplementacionUsuario - BuscarPorEmail()] {e.Message}");
                return null;
            }
        }

        public Usuario Eliminar(long id)
        {
            try
            {
                Usuario? usuario = _contexto.Usuarios.Find(id);
                if (usuario != null)
                {
                    _contexto.Usuarios.Remove(usuario);
                    _contexto.SaveChanges();
                    Console.WriteLine("Borrado con éxito");
                }
                return usuario;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - Eliminar()] {e.Message}");
                return null;
            }
        }

        public void ActualizarUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                Usuario? usuarioActual = _contexto.Usuarios.Find(usuarioDTO.Id);
                Console.WriteLine(usuarioDTO.NombreUsuario);
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
                Console.WriteLine($"[Error ImplementacionUsuario - ActualizarUsuario()] {ae.Message}");
            }
        }

        public UsuarioDTO BuscarDtoPorId(long id)
        {
            try
            {
                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                if (usuario != null)
                {
                    return _toDto.usuarioToDto(usuario);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - BuscarDtoPorId()] {e.Message}");
            }
            return null;
        }

        public List<UsuarioDTO> BuscarTodos()
        {
            try
            {
                return _toDto.listaUsuarioToDto(_contexto.Usuarios.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - BuscarTodos()] {e.Message}");
                return new List<UsuarioDTO>();
            }
        }

        public bool BuscarPorDni(string dni)
        {
            try
            {

                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.DniUsuario == dni);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - BuscarPorDni()] {e.Message}");
                return false;
            }
        }

        public Usuario BuscarPorId(long id)
        {
            try
            {
                Usuario? usuario = _contexto.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                return usuario;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - BuscarPorId()] {e.Message}");
                return null;
            }
        }

    }
}
