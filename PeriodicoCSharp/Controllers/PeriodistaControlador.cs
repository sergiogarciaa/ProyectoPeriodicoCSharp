using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Servicios;

namespace PeriodicoCSharp.Controllers
{
    public class PeriodistaController : Controller
    {
        private readonly UsuarioServicio _usuarioServicio;
        private readonly ConversionDTO _toDto;
        private readonly ConversionDao _toDao;
        private readonly InterfazNoticia _noticia;
        private readonly InterfazCategoria _categoria;

        public PeriodistaController(UsuarioServicio usuarioServicio, ConversionDTO toDto, InterfazNoticia interfazNoticia, InterfazCategoria categoria, ConversionDao toDao)
        {
            _usuarioServicio = usuarioServicio;
            _toDto = toDto;
            _toDao = toDao;
            _noticia = interfazNoticia;
            _categoria = categoria;
        }

        /// <summary>
        /// Método HTTP GET de la url /privada/zonaPeriodista para mostrar la zona de trabajo del periodista.
        /// </summary>
        /// <returns>La vista con la zona de trabajo del periodista</returns>
        [HttpGet]
        [Route("/privada/zonaPeriodista")]
        [Authorize(Roles = "ROLE_3, ROLE_4")]
        public IActionResult ZonaPeriodista()
        {
            try
            {
                // Obtener listas de categorías y usuarios desde tus servicios
                var categoriasDTO = _categoria.BuscarTodas();

                // Agregar las listas al ViewBag para que la vista pueda acceder a ellas
                ViewBag.CategoriasDTO = categoriasDTO;

                // Agregar un objeto NoticiaDTO al ViewBag para que la vista pueda mostrar y editar los datos
                ViewBag.NoticiaDTO = new NoticiaDTO();

                return View("~/Views/Home/periodista.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                Console.WriteLine("ErrorZonaPeriodista - " + e);
                return View("~/Views/Home/menu.cshtml"); // Asegúrate de tener una vista llamada "Periodista" para redirigir en caso de error
            }
        }

        /// <summary>
        /// Método HTTP POST para crear una nueva noticia.
        /// </summary>
        /// <param name="noticiaDTO">DTO de la noticia a crear</param>
        /// <param name="idCategoria">ID de la categoría de la noticia</param>
        /// <param name="foto">Imagen asociada a la noticia</param>
        /// <returns>Redirección a la zona de trabajo del periodista si la noticia se guarda correctamente, de lo contrario, redirecciona al menú de inicio de sesión</returns>
        [HttpPost]
        [Authorize(Roles = "ROLE_3, ROLE_4")]
        public IActionResult crearNoticia([FromForm] NoticiaDTO noticiaDTO, [FromForm] string idCategoria, [FromForm] IFormFile foto)
        {
            try
            {
                // Obtener el usuario autenticado
                var usuario = _usuarioServicio.BuscarPorEmailDAO(User.Identity.Name);

                // Convertir la categoría de string a long
                if (!long.TryParse(idCategoria, out long categoriaId))
                {
                    throw new Exception("El ID de categoría no es válido.");
                }

                // Consultar la categoría por ID desde el servicio
                var categoria = _categoria.BuscarPorId(categoriaId);
                noticiaDTO.FchPublicacion = Convert.ToString(DateTime.Now);

                // Convertir DTO a entidad utilizando ConversionDao
                var noticia = _toDao.noticiaToDao(noticiaDTO, usuario, categoria);

                // Si se proporcionó una imagen, procesarla
                if (foto != null && foto.Length > 0)
                {
                    byte[] fotoBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        // Copiar la imagen al flujo de memoria
                        foto.CopyTo(memoryStream);
                        // Asignar la imagen al campo de imagen de la noticia
                        noticia.ImagenNoticia = memoryStream.ToArray();
                        fotoBytes = memoryStream.ToArray();
                        noticiaDTO.ImagenNoticia = fotoBytes;
                    }
                }

                // Guardar la noticia en la base de datos
                _noticia.GuardarNoticia(noticia);

                // Redirigir al usuario a la página de zona de periodista si la noticia se guardó correctamente
                return RedirectToAction("ZonaPeriodista", "Periodista");
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
