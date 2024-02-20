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

public class LoginController : Controller
{
    private readonly UsuarioServicio _usuarioServicio;
    private readonly InterfazUsuarioToDTO _usuarioToDTO;

    public LoginController(UsuarioServicio usuarioServicio, InterfazUsuarioToDTO usuarioToDTO)
    {
        _usuarioServicio = usuarioServicio;
        _usuarioToDTO = usuarioToDTO;
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
                return View("~/Views/Home/registro.cshtml", usuarioDTO);

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                Console.WriteLine(e);
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
 
            if (nuevoUsuario.EmailUsuario == "EmailNoConfirmado")
                {
                    ViewData["EmailNoConfirmado"] = "Ya existe un usuario registrado con ese email pero con la cuenta sin verificar";
                    return View("~/Views/Home/login.cshtml");

                }
                else if (nuevoUsuario.EmailUsuario == "EmailRepetido")
                {
                    ViewData["EmailRepetido"] = "Ya existe un usuario con ese email registrado en el sistema";
                    return View("~/Views/Home/registro.cshtml");
                }
                else
                {
                    ViewData["MensajeRegistroExitoso"] = "Registro del nuevo usuario OK";
                    return View("~/Views/Home/login.cshtml");
                }


            }
            catch (Exception e)
            {
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
            Console.WriteLine("aqui");
            bool credencialesValidas = _usuarioServicio.verificarCredenciales(usuarioDTO.EmailUsuario, usuarioDTO.ClaveUsuario);

            if (credencialesValidas)
            {
                UsuarioDTO u = _usuarioServicio.BuscarPorEmail(usuarioDTO.EmailUsuario);
                Console.WriteLine(u);

                // Al hacer login correctamente se crea una identidad de reclamaciones (claims identity) con información del usuario 
                // y su rol, de esta manera se controla que solo los admin puedan acceder a la administracion de usuarios
                // y se mantiene esa info del usuario autenticado durante toda la sesión en una cookie.
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

                return RedirectToAction("index", "Login");
            }
            else
            {
                Console.WriteLine("EROR CREDENCIALES");
                ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada. Inténtelo de nuevo.";
                return View("~/Views/Home/login.cshtml");
            }
        }
        catch (Exception e)
        {
            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            Console.WriteLine(e);
            return View("~/Views/Home/login.cshtml");
        }
    }

    [HttpGet("/auth/confirmacionCorreo")]
    public IActionResult ConfirmarCuenta([FromQuery] string token)
    {
        try
        {
            var confirmacionExitosa = _usuarioServicio.ConfirmarCuenta(token);

            if (confirmacionExitosa)
            {
                ViewBag.CuentaVerificada = "Su dirección de correo ha sido confirmada correctamente";
            }
            else
            {
                ViewBag.CuentaNoVerificada = "Error al confirmar su email";
            }

            return View("login");
        }
        catch (Exception e)
        {
            ViewBag.Error = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            Console.WriteLine(e);
            return View("login");
        }
    }
    [Authorize]
    [HttpGet]
    [Route("/privada/index")]
    public IActionResult index()
    {
        UsuarioDTO u = _usuarioServicio.BuscarPorEmail(User.Identity.Name);
        ViewBag.UsuarioDTO = u;
        return View("~/Views/Home/Index.cshtml");
    }
}
