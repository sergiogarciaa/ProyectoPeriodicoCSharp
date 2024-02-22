using DAL.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PeriodicoCSharp.DTO;
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

        public List<NoticiaDTO> buscar4Primeras()
        {
            try
            {
                var noticiasRecientes = _contexto.Noticias
                    .OrderByDescending(n => n.FchPublicacion)
                    .Take(4)
                    .ToList();

                return _toDto.listaNoticiasToDto(noticiasRecientes);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - buscar4Primeras()] {e.Message}");
                return new List<NoticiaDTO>();
            }
        }

        public Noticia buscarNoticiaPorID(long id)
        {
            try
            {
                return _contexto.Noticias.FirstOrDefault(n => n.IdNoticia == id);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - buscarNoticiaPorID()] {e.Message}");
                return null;
            }
        }

        public NoticiaDTO buscarNoticiaPorIDDTO(long id)
        {
            try
            {
                var noticia = _contexto.Noticias.FirstOrDefault(n => n.IdNoticia == id);
                return _toDto.noticiaToDto(noticia);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - buscarNoticiaPorIDDTO()] {e.Message}");
                return null;
            }
        }

        public List<NoticiaDTO> buscarPorCategoria(long idCategoria)
        {
            try
            {
                var noticias = _contexto.Noticias
                    .Where(n => n.IdCategoriaNoticia == idCategoria)
                    .ToList();

                return noticias.Select(n => _toDto.noticiaToDto(n)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - buscarPorCategoria()] {e.Message}");
                return new List<NoticiaDTO>();
            }
        }

        public List<NoticiaDTO> buscarTodas()
        {
            try
            {
                return _toDto.listaNoticiasToDto(_contexto.Noticias.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - buscarTodas()] {e.Message}");
                return new List<NoticiaDTO>();
            }
        }

        public string ConvertToBase64(IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - ConvertToBase64()] {e.Message}");
                return string.Empty;
            }
        }

        public byte[] convertToByteArray(string base64String)
        {
            try
            {
                return Convert.FromBase64String(base64String);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - convertToByteArray()] {e.Message}");
                return null;
            }
        }

        public void GuardarNoticia(Noticia noticia)
        {
            try
            {
                _contexto.Noticias.Add(noticia);
                _contexto.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - GuardarNoticia()] {e.Message}");
            }
        }

        public Noticia obtenerNoticiaMasReciente()
        {
            try
            {
                return _contexto.Noticias.OrderByDescending(n => n.FchPublicacion).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - obtenerNoticiaMasReciente()] {e.Message}");
                return null;
            }
        }

        public string resumirNoticia(string texto)
        {
            try
            {
                return texto?.Length > 200 ? texto.Substring(0, 200) + "..." : texto;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - resumirNoticia()] {e.Message}");
                return texto;
            }
        }

        public string resumirNoticia2(string texto)
        {
            throw new NotImplementedException();
        }
    }
}
