using DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Servicios;
using PeriodicoCSharp.Utils;
using System.Globalization;

namespace PeriodicoCSharp.Controllers
{
    public class ComentarioController : Controller
    {
        private readonly InterfazNoticia _noticiaServicio;
        private readonly UsuarioServicio _usuarioServicio;
        private readonly PeriodicoContext _contexto;

        public ComentarioController(InterfazNoticia noticiaServicio, UsuarioServicio usuarioServicio, PeriodicoContext contexto)
        {
            _noticiaServicio = noticiaServicio;
            _usuarioServicio = usuarioServicio;
            _contexto = contexto;
        }

        /// <summary>
        /// Método para agregar un comentario a una noticia.
        /// </summary>
        /// <param name="idNoticia">ID de la noticia a la que se agrega el comentario.</param>
        /// <param name="comentarioDTO">DTO del comentario a agregar.</param>
        /// <returns>IActionResult</returns>
        /// <remarks>
        /// Agrega un comentario a una noticia específica y redirige a la página de visualización de la noticia.
        /// </remarks>
        [HttpPost]
        [Route("/auth/{idNoticia}/comentario")]
        [Authorize]
        public IActionResult AgregarComentario(long idNoticia, ComentarioDTO comentarioDTO)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando en el método AgregarComentario() de la clase NoticiasController");

                // Recuperar el usuario autenticado por su nombre de usuario
                var usuario = _usuarioServicio.BuscarPorEmail(User.Identity.Name);

                if (usuario != null)
                {
                    // Buscar la noticia por su ID
                    var noticia = _contexto.Noticias.FirstOrDefault(n => n.IdNoticia == idNoticia);

                    // Verificar si la noticia existe
                    if (noticia == null)
                    {
                        return NotFound("La noticia especificada no existe.");
                    }

                    // Crear un nuevo comentario
                    var comentario = new Comentario
                    {
                        DescComentario = comentarioDTO.DescComentario,
                        FchPublicacionComentario = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        IdUsuario = usuario.Id // Asignar el ID del usuario al comentario
                    };

                    // Agregar el comentario a la noticia
                    noticia.Comentarios.Add(comentario);

                    // Guardar cambios en la base de datos
                    _contexto.SaveChanges();

                    // Redirigir a la página de visualización de la noticia
                    return RedirectToAction("VerNoticia", "Noticias", new { idNoticia });
                }
                else
                {
                    return View("~/Views/Home/login.cshtml");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir durante el proceso de guardado
                Log.escribirEnFicheroLog("[ERROR] Se lanzó una excepción en el método AgregarComentario() de la clase NoticiasController: " + ex.Message + ex.StackTrace);
                return RedirectToAction("Menu", "Login");
            }
        }
    }
}
