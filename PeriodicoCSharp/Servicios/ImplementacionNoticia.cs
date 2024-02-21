using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using PeriodicoCSharp.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PeriodicoCSharp.Servicios
{
    public class ImplementacionNoticia : InterfazNoticia
    {
        private readonly PeriodicoContext _contexto;
        private readonly ConversionDao _toDao;
        private readonly ConversionDTO _toDto;


        public ImplementacionNoticia(PeriodicoContext contexto, ConversionDao toDao, ConversionDTO toDto)
        {
            _contexto = contexto;
            _toDao = toDao;
            _toDto = toDto;
        }

        public List<NoticiaDTO> buscar4Primeras()
        {
            try
            {
                // Obtener las 4 noticias más recientes ordenadas por fecha de publicación descendente
                var noticiasRecientes = _contexto.Noticias
                    .OrderByDescending(n => n.FchPublicacion)
                    .Take(4)
                    .ToList();

                // Convertir las noticias a DTOs
                return _toDto.listaNoticiasToDto(noticiasRecientes);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - BuscarCuatroMasRecientes()] {e.Message}");
                return new List<NoticiaDTO>();
            }
        }

        public Noticia buscarNoticiaPorID(long id)
        {
            try
            {
                Noticia? noticia = _contexto.Noticias.FirstOrDefault(n => n.IdNoticia == id);
                return noticia;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - BuscarPorId()] {e.Message}");
                return null;
            }
        }

        public List<NoticiaDTO> buscarPorCategoria(long idCategoria)
        {
            // Utilizando LINQ para buscar noticias por categoría
            var noticias = (from n in _contexto.Noticias
                            where n.IdCategoriaNoticia == idCategoria
                            select n)
                           .ToList();

            // Mapeo a DTO (Data Transfer Object)
            var noticiasDto = noticias.Select(n => _toDto.noticiaToDto(n)).ToList();

            return noticiasDto;
        }

        public List<NoticiaDTO> buscarTodas()
        {
            try
            {
                return _toDto.listaNoticiasToDto(_contexto.Noticias.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionUsuario - BuscarTodos()] {e.Message}");
                return new List<NoticiaDTO>();
            }
        }

        public string ConvertToBase64(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Copiar los bytes del archivo al flujo de memoria
                file.CopyTo(memoryStream);

                // Obtener los bytes del archivo
                var fileBytes = memoryStream.ToArray();

                // Convertir los bytes a base64
                return Convert.ToBase64String(fileBytes);
            }
        }

        public byte[] convertToByteArray(string base64String)
        {
            if (!string.IsNullOrEmpty(base64String))
            {
                return Convert.FromBase64String(base64String);
            }
            return null;
        }

        public void GuardarNoticia(Noticia noticia)
        {
            
            _contexto.Noticias.Add(noticia);
            _contexto.SaveChanges();
        }

        public Noticia obtenerNoticiaMasReciente()
        {
            // Recupera todas las noticias
            List<Noticia> noticias = _contexto.Noticias.ToList();

            // Ordena las noticias por fecha de publicación en orden descendente
            noticias = noticias.OrderByDescending(n => n.FchPublicacion).ToList();
            // Verifica si hay noticias
            if (noticias.Any())
            {
                // La primera noticia en la lista será la más reciente debido al ordenamiento
                return noticias[0];
            }
            else
            {
                // Si no hay noticias, devuelve null o maneja el caso según tus necesidades
                return null;
            }
        }

        public string resumirNoticia(string texto)
        {
            if (texto != null && texto.Length >= 200)
            {
                // Subcadena que contiene los primeros 35 caracteres
                return texto.Substring(0, 200) + "...";
            }
            else
            {
                // Si el texto tiene menos de 50 caracteres o es nulo, devuelve el texto original
                return texto;
            }
        }

        public string resumirNoticia2(string texto)
        {
            throw new NotImplementedException();
        }
    }
}
