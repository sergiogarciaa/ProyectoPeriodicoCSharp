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

[Route("[controller]")]
public class LoginRegistroControl : Controller
{
    private readonly UsuarioServicio _usuarioServicio;
    private readonly InterfazUsuarioToDTO _usuarioToDTO;

    public LoginRegistroControl(UsuarioServicio usuarioServicio, InterfazUsuarioToDTO usuarioToDTO)
    {
        _usuarioServicio = usuarioServicio;
        _usuarioToDTO = usuarioToDTO;
    }

    [HttpGet("/auth/login")]
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

    [HttpGet("/auth/registrar")]
    public IActionResult RegistrarGet()
    {
        try
        {
            ViewBag.UsuarioDTO = new UsuarioDTO();
            return View("registro");
        }
        catch (Exception e)
        {
            ViewBag.Error = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
            return View("registro");
        }
    }

    [HttpPost("/auth/registrar")]
    public IActionResult RegistrarPost([FromForm] UsuarioDTO usuarioDTO)
    {
        var nuevoUsuario = _usuarioServicio.Registrar(usuarioDTO);

        if (nuevoUsuario != null && !nuevoUsuario.CuentaConfirmada)
        {
            ViewBag.MensajeRegistroExitoso = "Registro del nuevo usuario OK";
            return View("login");
        }
        else if (nuevoUsuario.CuentaConfirmada)
        {
            return View("login");
        }
        else
        {
            ViewBag.MensajeErrorMail = "Ya existe un usuario con ese email";
            return View("registro");
        }
    }

    [HttpGet("/privada/index")]
    public IActionResult LoginCorrecto(UsuarioDTO usuarioDTO)
    {
        try
        {
            bool credencialesValidas = _usuarioServicio.verificarCredenciales(usuarioDTO.EmailUsuario, usuarioDTO.ClaveUsuario);

            if (credencialesValidas)
            {
                UsuarioDTO u = _usuarioServicio.BuscarPorEmail(usuarioDTO.EmailUsuario);

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

                return RedirectToAction("Dashboard", "Login");
            }
            else
            {
                ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada. Inténtelo de nuevo.";
                return View("~/Views/Home/login.cshtml");
            }
        }
        catch (Exception e)
        {
            ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
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
            return View("login");
        }
    }
}
