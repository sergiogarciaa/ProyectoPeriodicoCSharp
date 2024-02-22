using DAL.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        /// <summary>
        /// Busca las 4 primeras noticias más recientes.
        /// </summary>
        /// <returns>Lista de las 4 primeras noticias más recientes en forma de DTO.</returns>
        public List<NoticiaDTO> buscar4Primeras()
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método buscar4Primeras() en ImplementacionNoticia");

                var noticiasRecientes = _contexto.Noticias
                    .OrderByDescending(n => n.FchPublicacion)
                    .Take(4)
                    .ToList();

                return _toDto.listaNoticiasToDto(noticiasRecientes);
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - buscar4Primeras()] {e.Message}");
                return new List<NoticiaDTO>();
            }
        }

        /// <summary>
        /// Busca una noticia por su ID.
        /// </summary>
        /// <param name="id">ID de la noticia.</param>
        /// <returns>La noticia encontrada.</returns>
        public Noticia buscarNoticiaPorID(long id)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método buscarNoticiaPorID() en ImplementacionNoticia");

                return _contexto.Noticias.FirstOrDefault(n => n.IdNoticia == id);
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - buscarNoticiaPorID()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Busca una noticia por su ID y la convierte en un DTO.
        /// </summary>
        /// <param name="id">ID de la noticia.</param>
        /// <returns>La noticia encontrada en forma de DTO.</returns>
        public NoticiaDTO buscarNoticiaPorIDDTO(long id)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método buscarNoticiaPorIDDTO() en ImplementacionNoticia");

                var noticia = _contexto.Noticias.FirstOrDefault(n => n.IdNoticia == id);
                return _toDto.noticiaToDto(noticia);
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - buscarNoticiaPorIDDTO()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Busca todas las noticias de una categoría específica.
        /// </summary>
        /// <param name="idCategoria">ID de la categoría.</param>
        /// <returns>Lista de noticias de la categoría especificada en forma de DTO.</returns>
        public List<NoticiaDTO> buscarPorCategoria(long idCategoria)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método buscarPorCategoria() en ImplementacionNoticia");

                var noticias = _contexto.Noticias
                    .Where(n => n.IdCategoriaNoticia == idCategoria)
                    .ToList();

                return noticias.Select(n => _toDto.noticiaToDto(n)).ToList();
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - buscarPorCategoria()] {e.Message}");
                return new List<NoticiaDTO>();
            }
        }

        /// <summary>
        /// Busca todas las noticias y las convierte en DTO.
        /// </summary>
        /// <returns>Lista de todas las noticias en forma de DTO.</returns>
        public List<NoticiaDTO> buscarTodas()
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método buscarTodas() en ImplementacionNoticia");

                return _toDto.listaNoticiasToDto(_contexto.Noticias.ToList());
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - buscarTodas()] {e.Message}");
                return new List<NoticiaDTO>();
            }
        }

        /// <summary>
        /// Convierte un archivo de tipo IFormFile a una cadena Base64.
        /// </summary>
        /// <param name="file">Archivo a convertir.</param>
        /// <returns>Cadena Base64 del archivo.</returns>
        public string ConvertToBase64(IFormFile file)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método ConvertToBase64() en ImplementacionNoticia");

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - ConvertToBase64()] {e.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Convierte una cadena Base64 a un arreglo de bytes.
        /// </summary>
        /// <param name="base64String">Cadena Base64 a convertir.</param>
        /// <returns>Arreglo de bytes.</returns>
        public byte[] convertToByteArray(string base64String)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método convertToByteArray() en ImplementacionNoticia");

                return Convert.FromBase64String(base64String);
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - convertToByteArray()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Guarda una nueva noticia en la base de datos.
        /// </summary>
        /// <param name="noticia">La noticia a guardar.</param>
        public void GuardarNoticia(Noticia noticia)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método GuardarNoticia() en ImplementacionNoticia");

                _contexto.Noticias.Add(noticia);
                _contexto.SaveChanges();
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - GuardarNoticia()] {e.Message}");
            }
        }

        /// <summary>
        /// Obtiene la noticia más reciente.
        /// </summary>
        /// <returns>La noticia más reciente.</returns>
        public Noticia obtenerNoticiaMasReciente()
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método obtenerNoticiaMasReciente() en ImplementacionNoticia");

                return _contexto.Noticias.OrderByDescending(n => n.FchPublicacion).FirstOrDefault();
            }
            catch (Exception e)
            {   
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - obtenerNoticiaMasReciente()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Resume el texto de una noticia a 200 caracteres.
        /// </summary>
        /// <param name="texto">Texto de la noticia.</param>
        /// <returns>Texto resumido.</returns>
        public string resumirNoticia(string texto)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método resumirNoticia() en ImplementacionNoticia");

                return texto?.Length > 200 ? texto.Substring(0, 200) + "..." : texto;
            }
            catch (Exception e)
            {
                Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - resumirNoticia()] {e.Message}");
                return texto;
            }
        }
    }
}
