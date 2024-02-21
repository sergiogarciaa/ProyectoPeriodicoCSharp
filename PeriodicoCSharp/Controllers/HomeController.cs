using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PeriodicoCSharp.Models;
using System.Diagnostics;
using DAL.Entidades;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Servicios;

namespace PeriodicoCSharp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InterfazNoticia _noticia;
        private readonly PeriodicoContext _contexto;

        public HomeController(ILogger<HomeController> logger, InterfazNoticia noticiaInterfaz, PeriodicoContext periodicoContext, InterfazNoticia interfazNoticia)
        {
            _logger = logger;
            _contexto = periodicoContext;
            _noticia = interfazNoticia;
        }

        /// <summary>
        /// Gestiona las peticiones GET de la url / mostrando la primera vista de la aplicación (home)
        /// </summary>
        /// <returns>La vista de home</returns>
        public IActionResult Index()
        {
            try
            {
                List<NoticiaDTO> noticias = _noticia.buscar4Primeras();
                // Resumir noticias
                foreach (var noticia in noticias)
                {
                    noticia.resumenNoticia = _noticia.resumirNoticia(noticia.DescNoticia);
                }
                ViewBag.Noticia = noticias;


                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return View(noticias);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al mostrar la vista de Home";
                return View();
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}