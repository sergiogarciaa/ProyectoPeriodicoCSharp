using DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Servicios;
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

        [HttpPost]
        [Route("/auth/{idNoticia}/comentario")]
        [Authorize]
        public IActionResult AgregarComentario(long idNoticia, ComentarioDTO comentarioDTO)
        {
            try
            {
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
                    // Manejar el caso en el que no se encuentre al usuario autenticado
                    return Unauthorized("El usuario autenticado no pudo ser encontrado.");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir durante el proceso de guardado
                Console.WriteLine("ERROR: " + ex.Message);
                return RedirectToAction("Menu", "Login");
            }
        }
    }
}
