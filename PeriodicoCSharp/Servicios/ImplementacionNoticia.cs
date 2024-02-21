using DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using PeriodicoCSharp.DTO;

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

        public string convertToBase64(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public byte[] convertToByteArray(string foto)
        {
            throw new NotImplementedException();
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
            if (texto != null && texto.Length >= 35)
            {
                // Subcadena que contiene los primeros 35 caracteres
                return texto.Substring(0, 35);
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
