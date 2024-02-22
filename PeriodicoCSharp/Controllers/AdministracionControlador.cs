using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Servicios;

namespace PeriodicoCSharp.Controllers
{
    public class AdministracionController : Controller
    {
        private readonly UsuarioServicio _usuarioServicio;
        public AdministracionController(UsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        /// <summary>
        /// Método para mostrar el listado de usuarios en la vista de administración.
        /// </summary>
        /// <returns>IActionResult</returns>
        /// <remarks>
        /// Devuelve una vista que muestra el listado de todos los usuarios registrados en el sistema.
        /// </remarks>
        [Authorize(Roles = "ROLE_4")]
        [HttpGet]
        [Route("/privada/administracion")]
        public IActionResult ListadoUsuarios()
        {
            try
            {
                List<UsuarioDTO> usuarios = new List<UsuarioDTO>();
                ViewBag.Usuarios = _usuarioServicio.BuscarTodos();
                Console.WriteLine(usuarios);

                return View("~/Views/Home/administracion.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener la lista de usuarios";
                Console.WriteLine("[ERROR] Se lanzó una excepción en el método ListadoUsuarios() de la clase AdministracionUsuariosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/menu.cshtml");
            }
        }
        /// <summary>
        /// Método para eliminar un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns>IActionResult</returns>
        /// <remarks>
        /// Elimina un usuario por su ID y redirige a la vista de administración de usuarios con el resultado de la eliminación.
        /// </remarks>
        [Authorize(Roles = "ROLE_4")]
        [HttpGet]
        [Route("/privada/eliminar/{id}")]
        public IActionResult EliminarUsuario(long id)
        {
            try
            {
                UsuarioDTO usuario = _usuarioServicio.BuscarDtoPorId(id);
                List<UsuarioDTO> usuarios = _usuarioServicio.BuscarTodos();

                string emailUsuarioActual = User.Identity.Name;

                if (emailUsuarioActual == usuario.EmailUsuario)
                {
                    ViewData["ErrorBorrarAsiMismo"] = "No te puedes eliminar a ti mismo";
                    ViewBag.Usuarios = usuarios;
                    return View("~/Views/Home/administracion.cshtml");
                }

                _usuarioServicio.Eliminar(id);
                ViewData["EliminacionCorrecta"] = "El usuario se ha eliminado correctamente";
                ViewBag.Usuarios = _usuarioServicio.BuscarTodos();

                return View("~/Views/Home/administracion.cshtml");

            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al eliminar el usuario";
                Console.WriteLine("[ERROR] Se lanzó una excepción en el método EliminarUsuario() de la clase AdministracionUsuariosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/menu.cshtml");
            }
        }

        /// <summary>
        /// Método para editar un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario a editar.</param>
        /// <returns>IActionResult</returns>
        /// <remarks>
        /// Obtiene un usuario por su ID y muestra el formulario de edición con los detalles del usuario.
        /// </remarks>
        [Authorize(Roles = "ROLE_4")]
        [HttpGet]
        [Route("/privada/editar/{id}")]
        public IActionResult Editar(long id)
        {
            try
            {
                UsuarioDTO usuarioDTO = _usuarioServicio.BuscarDtoPorId(id);

                return View("~/Views/Home/edicion.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener el usuario para editar";
                Console.WriteLine("[ERROR] Se lanzó una excepción en el método MostrarFormularioEdicion() de la clase AdministracionUsuariosController: " + e.Message + e.StackTrace);
                return View("~/Views/Home/menu.cshtml");
            }
        }

        /// <summary>
        /// Método para guardar los cambios realizados en la edición de un usuario.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        /// <param name="nombre">Nuevo nombre del usuario.</param>
        /// <param name="apellidos">Nuevos apellidos del usuario.</param>
        /// <param name="dni">Nuevo DNI del usuario.</param>
        /// <param name="telefono">Nuevo teléfono del usuario.</param>
        /// <param name="rol">Nuevo rol del usuario.</param>
        /// <returns>IActionResult</returns>
        /// <remarks>
        /// Guarda los cambios realizados en un usuario y redirige a la vista de administración de usuarios con el resultado de la edición.
        /// </remarks>
        [Authorize(Roles = "ROLE_4")]
        [HttpPost]
        [Route("/privada/confirmarEdicion")]
        public IActionResult GuardarEdicion(long id, string nombre, string apellidos, string dni, string telefono, string rol)
        {
            try
            {

                UsuarioDTO usuarioDTO = _usuarioServicio.BuscarDtoPorId(id);
                usuarioDTO.NombreUsuario = nombre;
                usuarioDTO.ApellidosUsuario = apellidos;
                usuarioDTO.DniUsuario = dni;
                usuarioDTO.TlfUsuario = telefono;
                usuarioDTO.Rol = rol;


                _usuarioServicio.ActualizarUsuario(usuarioDTO);

                ViewData["EdicionCorrecta"] = "El Usuario se ha editado correctamente";
                ViewBag.Usuarios = _usuarioServicio.BuscarTodos();

                return View("~/Views/Home/administracion.cshtml");
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Ocurrió un error al editar el usuario";
                return View("~/Views/Home/menu.cshtml");
            }
        }

    }
}
