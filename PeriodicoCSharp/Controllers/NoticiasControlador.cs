using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeriodicoCSharp.Servicios;

namespace PeriodicoCSharp.Controllers
{
    public class NoticiasController : Controller
    {
        private readonly UsuarioServicio _usuarioServicio;
        private readonly ConversionDTO _toDto;
        private readonly ConversionDao _toDao;
        private readonly InterfazNoticia _noticia;
        private readonly InterfazCategoria _categoria;
        public NoticiasController(UsuarioServicio usuarioServicio, ConversionDTO toDto, InterfazNoticia interfazNoticia, InterfazCategoria categoria, ConversionDao toDao)
        {
            _usuarioServicio = usuarioServicio;
            _toDto = toDto;
            _toDao = toDao;
            _noticia = interfazNoticia;
            _categoria = categoria;
        }

        [HttpGet]
        [Route("/privada/ver/{idCategoria}")]
        [Authorize]
        public IActionResult VerCategoria(long idCategoria)
        {
            try
            {
                var usuario = _usuarioServicio.BuscarPorEmail(User.Identity.Name);

                if (usuario == null || !User.Identity.IsAuthenticated)
                {
                    ViewBag.Mensaje = "Debes estar logeado para acceder a esta página.";
                    return View("Login");
                }
                else
                {
                    var categoriaDTO = _categoria.BuscarPorId(idCategoria);
                    var noticiasPorCategoria = _noticia.buscarPorCategoria(idCategoria);

                    foreach (var noticiaDTO in noticiasPorCategoria)
                    {
                        noticiaDTO.resumenNoticia = _noticia.resumirNoticia(noticiaDTO.DescNoticia);
                    }

                    var categoriaTodasDTO = _categoria.BuscarTodas();

                    ViewBag.NoticiasPorCategoria = noticiasPorCategoria;
                    ViewBag.Categoria = categoriaDTO;
                    ViewBag.CategoriaTodasDTO = categoriaTodasDTO;

                    return View("~/Views/Home/verCategoria.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                Console.WriteLine("Error en VerCategoria: " + e.Message);
                return View("Menu");
            }
        }
    }
}
