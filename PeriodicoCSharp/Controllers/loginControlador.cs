using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DAL.Entidades;
using PeriodicoCSharp.Utils;

public class LoginController : Controller
{
    private readonly UsuarioServicio _usuarioServicio;
    private readonly ConversionDTO _usuarioToDTO;
    private readonly InterfazNoticia _noticia;
    private readonly InterfazCategoria _categoria;

    public LoginController(UsuarioServicio usuarioServicio, ConversionDTO usuarioToDTO, InterfazNoticia interfazNoticia, InterfazCategoria categoria)
    {
        _usuarioServicio = usuarioServicio;
        _usuarioToDTO = usuarioToDTO;
        _noticia = interfazNoticia;
        _categoria = categoria;
    }

    [HttpGet]
    [Route("/auth/login")]
    public IActionResult Login()
    {
        UsuarioDTO usuarioDTO = new UsuarioDTO();
        return View("~/Views/Home/login.cshtml", usuarioDTO);
    }

    [HttpGet("/auth/landing")]
    public IActionResult Index()
    {
        return Redirect("/");
    }

    /// <summary>
    /// Método HTTP GET de la url /auth/crear-cuenta para mostrar la vista de registro.
    /// </summary>
    /// <returns>La vista de registro</returns>
    [HttpGet]
    [Route("/auth/crear-cuenta")]
    public IActionResult RegistrarGet()
    {
        try
        {
            UsuarioDTO usuarioDTO = new UsuarioDTO();

            // Registro de éxito en el log
            Log.escribirEnFicheroLog("[RegistrarGet] Se ha mostrado la vista de registro correctamente.");

            return View("~/Views/Home/registro.cshtml", usuarioDTO);
        }
        catch (Exception e)
        {
            // Registro de error en el log
            Log.escribirEnFicheroLog("[RegistrarGet] Error al procesar la solicitud: " + e.Message);

            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            return View("~/Views/Home/registro.cshtml");
        }
    }

    /// <summary>
    /// Método HTTP POST para procesar el registro de un nuevo usuario.
    /// </summary>
    /// <param name="usuarioDTO">DTO del usuario con los datos de registro</param>
    /// <returns>La vista correspondiente según el resultado del registro</returns>
    [HttpPost]
    [Route("/auth/crear-cuenta")]
    public IActionResult RegistrarPost(UsuarioDTO usuarioDTO)
    {
        try
        {
            UsuarioDTO nuevoUsuario = _usuarioServicio.Registrar(usuarioDTO);

            if (usuarioDTO.EmailUsuario == "EmailNoConfirmado")
            {
                ViewData["EmailNoConfirmado"] = "Ya existe un usuario registrado con ese email pero con la cuenta sin verificar";

                // Registro de éxito en el log
                Log.escribirEnFicheroLog("[RegistrarPost] Registro del nuevo usuario con email no confirmado.");

                return View("~/Views/Home/login.cshtml");
            }
            else if (nuevoUsuario.EmailUsuario == "EmailRepetido")
            {
                ViewData["EmailRepetido"] = "Ya existe un usuario con ese email registrado en el sistema";

                // Registro de éxito en el log
                Log.escribirEnFicheroLog("[RegistrarPost] Intento de registro con email repetido.");

                return View("~/Views/Home/registro.cshtml");
            }
            else if(nuevoUsuario.EmailUsuario == "NoValido")
            {
                ViewData["NoValido"] = "Los datos aportados no son válidos";

                // Registro de éxito en el log
                Log.escribirEnFicheroLog("[RegistrarPost] Intento de registro aporta datos no válidos.");

                return View("~/Views/Home/registro.cshtml");
            }
            else
            {
                ViewData["MensajeRegistroExitoso"] = "Registro del nuevo usuario OK";

                // Registro de éxito en el log
                Log.escribirEnFicheroLog("[RegistrarPost] Registro del nuevo usuario exitoso.");

                return View("~/Views/Home/login.cshtml");
            }
        }
        catch (Exception e)
        {
            // Registro de error en el log
            Log.escribirEnFicheroLog("[RegistrarPost] Error al procesar la solicitud: " + e.Message);

            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            return View("~/Views/Home/registro.cshtml");
        }
    }

    [HttpPost]
    [Route("/auth/LoginCorrecto")]
    public IActionResult LoginCorrecto(UsuarioDTO usuarioDTO)
    {
        try
        {
            bool credencialesValidas = _usuarioServicio.verificarCredenciales(usuarioDTO.EmailUsuario, usuarioDTO.ClaveUsuario);

            if (credencialesValidas)
            {
                UsuarioDTO u = _usuarioServicio.BuscarPorEmail(usuarioDTO.EmailUsuario);
                Console.WriteLine(u);

                // Al hacer login correctamente se crea una identidad de reclamaciones (claims identity) con información del usuario 
                // y su rol y se mantiene esa info del usuario autenticado durante toda la sesión en una cookie.
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioDTO.EmailUsuario),
                    };
                if (!string.IsNullOrEmpty(u.Rol))
                {
                    claims.Add(new Claim(ClaimTypes.Role, u.Rol));
                }

                var identidadDeReclamaciones = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // establece una cookie en el navegador con los datos del usuario antes mencionados y se mantiene en el contexto.
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidadDeReclamaciones));

                // Registro de éxito en el log
                Log.escribirEnFicheroLog("[LoginCorrecto] Inicio de sesión exitoso para el usuario: " + usuarioDTO.EmailUsuario);

                return RedirectToAction("Menu", "Login");
            }
            else
            {
                ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada. Inténtelo de nuevo.";

                // Registro de error en el log
                Log.escribirEnFicheroLog("[LoginCorrecto] Error en el inicio de sesión para el usuario: " + usuarioDTO.EmailUsuario + ". Credenciales inválidas o cuenta no confirmada.");

                return View("~/Views/Home/login.cshtml");
            }
        }
        catch (Exception e)
        {
            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            Console.WriteLine(e);

            // Registro de error en el log
            Log.escribirEnFicheroLog("[LoginCorrecto] Error al procesar la solicitud: " + e.Message);

            return View("~/Views/Home/login.cshtml");
        }
    }

    /// <summary>
    /// Método para confirmar la cuenta de usuario mediante un token de confirmación.
    /// </summary>
    /// <param name="token">Token de confirmación de la cuenta de usuario</param>
    /// <returns>La vista de inicio de sesión con un mensaje sobre el estado de la confirmación</returns>
    [HttpGet]
    [Route("/auth/confirmacionCorreo")]
    public IActionResult ConfirmarCuenta([FromQuery] string token)
    {
        try
        {
            var confirmacionExitosa = _usuarioServicio.ConfirmarCuenta(token);

            if (confirmacionExitosa)
            {
                ViewBag.CuentaVerificada = "Su dirección de correo ha sido confirmada correctamente";

                // Registro de éxito en el log
                Log.escribirEnFicheroLog("[ConfirmarCuenta] Confirmación exitosa para el token: " + token);
            }
            else
            {
                ViewBag.CuentaNoVerificada = "Error al confirmar su email";

                // Registro de error en el log
                Log.escribirEnFicheroLog("[ConfirmarCuenta] Error al confirmar el email para el token: " + token);
            }

            return View("~/Views/Home/login.cshtml");
        }
        catch (Exception e)
        {
            ViewBag.Error = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            Console.WriteLine(e);

            // Registro de error en el log
            Log.escribirEnFicheroLog("[ConfirmarCuenta] Error al procesar la solicitud para el token: " + token + ". Detalles del error: " + e.Message);

            return View("~/Views/Home/login.cshtml");
        }
    }

    /// <summary>
    /// Método HTTP GET para mostrar el menú principal de la aplicación después de iniciar sesión.
    /// </summary>
    /// <returns>La vista del menú principal con la información del usuario autenticado y las últimas noticias</returns>
    [Authorize]
    [HttpGet]
    [Route("/privada/menu")]
    public IActionResult Menu()
    {
        try
        {
            // Buscar la información del usuario autenticado
            UsuarioDTO u = _usuarioServicio.BuscarPorEmail(User.Identity.Name);
            ViewBag.UsuarioDTO = u;

            // Obtener las 4 primeras noticias y todas las categorías
            List<NoticiaDTO> noticias = _noticia.buscar4Primeras();
            List<CategoriaDTO> categorias = _categoria.BuscarTodas();

            // Resumir el contenido de las noticias
            foreach (var noticia in noticias)
            {
                noticia.resumenNoticia = _noticia.resumirNoticia(noticia.DescNoticia);
            }

            // Pasar las noticias y categorías a la vista
            ViewBag.Noticia = noticias;
            ViewBag.Categorias = categorias;

            return View("~/Views/Home/menu.cshtml");
        }
        catch (Exception e)
        {
            // En caso de error, mostrar un mensaje genérico y registrar el error en el log
            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            Console.WriteLine(e);

            // Registro de error en el log
            Log.escribirEnFicheroLog("[Menu] Error al procesar la solicitud. Detalles del error: " + e.Message);

            return View("~/Views/Home/menu.cshtml");
        }
    }

    /// <summary>
    /// Método HTTP GET para mostrar la vista de inicio de solicitud de recuperación de contraseña.
    /// </summary>
    /// <returns>La vista de solicitud de recuperación de contraseña</returns>
    [HttpGet]
    [Route("/auth/recuperarPass")]
    public IActionResult MostrarVistaIniciarRecuperacion()
    {
        try
        {
            // Crea un nuevo objeto UsuarioDTO para pasar a la vista
            UsuarioDTO usuarioDTO = new UsuarioDTO();

            // Devuelve la vista de solicitud de recuperación de contraseña con el objeto UsuarioDTO
            return View("~/Views/Home/solicitarRecuperarPass.cshtml", usuarioDTO);
        }
        catch (Exception e)
        {
            // En caso de error, muestra un mensaje genérico y redirige a la página de inicio
            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            return View("~/Views/Home/Index.cshtml");
        }
    }

    /// <summary>
    /// Método HTTP POST para procesar el inicio del proceso de recuperación de contraseña.
    /// </summary>
    /// <param name="usuarioDTO">DTO del usuario con el email para iniciar la recuperación</param>
    /// <returns>La vista correspondiente según el resultado del inicio de recuperación</returns>
    [HttpPost]
    [Route("/auth/recuperacionPassGo")]
    public IActionResult ProcesarRecuperarPass([Bind("EmailUsuario")] UsuarioDTO usuarioDTO)
    {
        try
        {
            // Intenta iniciar el proceso de recuperación de contraseña enviando un correo electrónico
            bool envioConExito = _usuarioServicio.IniciarResetPassConEmail(usuarioDTO.EmailUsuario);

            // Si el envío fue exitoso, muestra un mensaje de éxito y redirige a la vista de inicio de sesión
            if (envioConExito)
            {
                ViewData["MensajeExitoMail"] = "Proceso de recuperación OK";
                return View("~/Views/Home/login.cshtml");
            }
            // Si el envío no fue exitoso, muestra un mensaje de error y redirige a la vista de solicitud de recuperación de contraseña
            else
            {
                ViewData["MensajeErrorMail"] = "No se inició el proceso de recuperación, cuenta de correo electrónico no encontrada.";
                return View("~/Views/Home/solicitarRecuperarPass.cshtml");
            }
        }
        // En caso de excepción, muestra un mensaje de error genérico y redirige a la vista de solicitud de recuperación de contraseña
        catch (Exception e)
        {
            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            return View("~/Views/Home/solicitarRecuperarPass.cshtml");
        }
    }

    /// <summary>
    /// Método para mostrar la vista de recuperación de contraseña.
    /// </summary>
    /// <param name="token">Token de recuperación de contraseña</param>
    /// <returns>La vista de recuperación de contraseña o la vista de solicitud de recuperación de contraseña en caso de error</returns>
    [HttpGet]
    [Route("/auth/recuperar")]
    public IActionResult MostrarVistaRecuperar([FromQuery(Name = "token")] string token)
    {
        try
        {
            // Intenta obtener el usuario asociado al token de recuperación
            UsuarioDTO usuario = _usuarioServicio.ObtenerUsuarioPorToken(token);

            // Si se encuentra un usuario válido, muestra la vista de recuperación de contraseña
            if (usuario != null)
            {
                ViewData["UsuarioDTO"] = usuario;
            }
            // Si no se encuentra un usuario válido, muestra un mensaje de error y redirige a la vista de solicitud de recuperación de contraseña
            else
            {
                ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido o el usuario no se ha encontrado";
                return View("~/Views/Home/solicitarRecuperarPass.cshtml");
            }
            return View("~/Views/Home/recuperar.cshtml");
        }
        // En caso de excepción, muestra un mensaje de error genérico y redirige a la vista de solicitud de recuperación de contraseña
        catch (Exception e)
        {
            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            return View("~/Views/Home/solicitarRecuperarPass.cshtml");
        }
    }

    /// <summary>
    /// Método para procesar la recuperación de contraseña.
    /// </summary>
    /// <param name="usuarioDTO">DTO del usuario con los datos de recuperación de contraseña</param>
    /// <returns>La vista correspondiente según el resultado de la operación</returns>
    [HttpPost]
    [Route("/auth/recuperar")]
    public IActionResult ProcesarRecuperacionContraseña(UsuarioDTO usuarioDTO)
    {
        try
        {
            // Obtener el usuario existente a partir del token de recuperación proporcionado
            UsuarioDTO usuarioExistente = _usuarioServicio.ObtenerUsuarioPorToken(usuarioDTO.TokenRecuperacion);

            // Verificar si se encontró un usuario válido con el token de recuperación
            if (usuarioExistente == null)
            {
                ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido";
                return View("~/Views/Home/solicitarRecuperarPass.cshtml");
            }

            // Verificar si el token de recuperación ha expirado
            if (usuarioExistente.ExpiracionToken.HasValue && usuarioExistente.ExpiracionToken.Value < DateTime.Now)
            {
                ViewData["MensajeErrorTokenExpirado"] = "El enlace de recuperación ha expirado";
                return View("~/Views/Home/solicitarRecuperarPass.cshtml");
            }

            // Modificar la contraseña del usuario con el token de recuperación
            bool modificadaPassword = _usuarioServicio.ModificarContraseñaConToken(usuarioDTO);

            // Verificar si la contraseña se modificó con éxito
            if (modificadaPassword)
            {
                ViewData["ContraseñaModificadaExito"] = "Contraseña modificada OK";
                return View("~/Views/Home/login.cshtml");
            }
            else
            {
                ViewData["ContraseñaModificadaError"] = "Error al cambiar de contraseña";
                return View("~/Views/Home/solicitarRecuperarPass.cshtml");
            }
        }
        // Manejar cualquier excepción y mostrar un mensaje de error genérico
        catch (Exception e)
        {
            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            return View("~/Views/Home/solicitarRecuperarPass.cshtml");
        }
    }

    /// <summary>
    /// Método HTTP POST para cerrar la sesión del usuario y redirigirlo a la página de inicio.
    /// </summary>
    /// <returns>Redirección a la página de inicio</returns>
    [HttpPost]
    public IActionResult Salir()
    {
        // Cerrar la sesión del usuario
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Redirigir al usuario a la página de inicio
        return RedirectToAction("Index", "Home");
    }
}
