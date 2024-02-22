using DAL.Entidades;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Utils;
using System;
using System.Collections.Generic;

namespace PeriodicoCSharp.Servicios
{
    public class ConversionDTOImpl : ConversionDTO
    {
        /// <summary>
        /// Convierte una lista de objetos Usuario en una lista de objetos UsuarioDTO para su envío a la capa de presentación.
        /// </summary>
        /// <param name="listaUsuario">Lista de objetos Usuario a convertir.</param>
        /// <returns>Lista de objetos UsuarioDTO convertidos.</returns>
        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método listaUsuarioToDto() en ConversionDTOImpl");

                List<UsuarioDTO> listaDto = new List<UsuarioDTO>();
                foreach (Usuario u in listaUsuario)
                {
                    listaDto.Add(usuarioToDto(u));
                }
                return listaDto;
            }
            catch (Exception e)
            {
                // Registra el error y lanza una excepción
                 Log.escribirEnFicheroLog("[ERROR ConversionDTOImpl - listaUsuarioToDto()] - Error al convertir lista de Usuario DAO a lista de UsuarioDTO: " + e);
                throw;
            }
        }

        /// <summary>
        /// Convierte un objeto Usuario en un objeto UsuarioDTO para su envío a la capa de presentación.
        /// </summary>
        /// <param name="u">Objeto Usuario a convertir.</param>
        /// <returns>Objeto UsuarioDTO convertido.</returns>
        public UsuarioDTO usuarioToDto(Usuario u)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método usuarioToDto() en ConversionDTOImpl");

                UsuarioDTO dto = new UsuarioDTO();
                dto.NombreUsuario = u.NombreUsuario;
                dto.ApellidosUsuario = u.ApellidosUsuario;
                dto.DniUsuario = u.DniUsuario;
                dto.TlfUsuario = u.TlfUsuario;
                dto.EmailUsuario = u.EmailUsuario;
                dto.ClaveUsuario = u.ClaveUsuario;
                dto.TokenRecuperacion = u.TokenRecuperacion;
                dto.ExpiracionToken = u.ExpiracionToken;
                dto.FchAltaUsuario = u.FchAltaUsuario;
                dto.Rol = u.Rol;
                dto.Id = u.IdUsuario;
                dto.CuentaConfirmada = u.CuentaConfirmada;
                return dto;
            }
            catch (Exception e)
            {
                // Registra el error y lanza una excepción
                 Log.escribirEnFicheroLog("[ERROR ConversionDTOImpl - usuarioToDto()] - Error al convertir Usuario DAO a UsuarioDTO: " + e);
                throw;
            }
        }

        /// <summary>
        /// Convierte un objeto Noticia en un objeto NoticiaDTO para su envío a la capa de presentación.
        /// </summary>
        /// <param name="noticia">Objeto Noticia a convertir.</param>
        /// <returns>Objeto NoticiaDTO convertido.</returns>
        public NoticiaDTO noticiaToDto(Noticia noticia)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método noticiaToDto() en ConversionDTOImpl");

                NoticiaDTO dto = new NoticiaDTO();
                dto.IdNoticia = noticia.IdNoticia;
                dto.TituloNoticia = noticia.TituloNoticia;
                dto.DescNoticia = noticia.DescNoticia;
                dto.RequiereSuscripcion = noticia.RequiereSuscripcion;
                dto.FchPublicacion = noticia.FchPublicacion;
                dto.ImagenNoticia = noticia.ImagenNoticia;
                dto.IdCategoriaNoticia = noticia.IdCategoriaNoticia;
                dto.IdUsuarioNoticia = noticia.IdUsuarioNoticia;

                return dto;
            }
            catch (Exception e)
            {
                // Registra el error y lanza una excepción
                 Log.escribirEnFicheroLog("[ERROR ConversionDTOImpl - noticiaToDto()] - Error al convertir Noticia DAO a NoticiaDTO: " + e);
                throw;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos Noticia en una lista de objetos NoticiaDTO para su envío a la capa de presentación.
        /// </summary>
        /// <param name="listaNoticia">Lista de objetos Noticia a convertir.</param>
        /// <returns>Lista de objetos NoticiaDTO convertidos.</returns>
        public List<NoticiaDTO> listaNoticiasToDto(List<Noticia> listaNoticia)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método listaNoticiasToDto() en ConversionDTOImpl");

                List<NoticiaDTO> listaNoticiaDto = new List<NoticiaDTO>();
                foreach (Noticia n in listaNoticia)
                {
                    listaNoticiaDto.Add(noticiaToDto(n));
                }
                return listaNoticiaDto;
            }
            catch (Exception e)
            {
                // Registra el error y lanza una excepción
                 Log.escribirEnFicheroLog("[ERROR ConversionDTOImpl - listaNoticiasToDto()] - Error al convertir lista de Noticia DAO a lista de NoticiaDTO: " + e);
                throw;
            }
        }

        /// <summary>
        /// Convierte un objeto Categoria en un objeto CategoriaDTO para su envío a la capa de presentación.
        /// </summary>
        /// <param name="categoria">Objeto Categoria a convertir.</param>
        /// <returns>Objeto CategoriaDTO convertido.</returns>
        public CategoriaDTO categoriaToDTO(Categoria categoria)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método categoriaToDTO() en ConversionDTOImpl");

                CategoriaDTO dto = new CategoriaDTO();
                dto.IdCategoria = categoria.IdCategoria;
                dto.TipoCategoria = categoria.TipoCategoria;
                dto.DescCategoria = categoria.DescCategoria;
                // Puedes agregar más atributos según sea necesario

                return dto;
            }
            catch (Exception e)
            {
                // Registra el error y lanza una excepción
                 Log.escribirEnFicheroLog("[ERROR ConversionDTOImpl - categoriaToDTO()] - Error al convertir Categoria DAO a CategoriaDTO: " + e);
                throw;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos Categoria en una lista de objetos CategoriaDTO para su envío a la capa de presentación.
        /// </summary>
        /// <param name="listaCategoria">Lista de objetos Categoria a convertir.</param>
        /// <returns>Lista de objetos CategoriaDTO convertidos.</returns>
        public List<CategoriaDTO> listaCategoriaToDto(List<Categoria> listaCategoria)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método listaCategoriaToDto() en ConversionDTOImpl");

                List<CategoriaDTO> listaCategoriaDto = new List<CategoriaDTO>();
                foreach (Categoria c in listaCategoria)
                {
                    listaCategoriaDto.Add(categoriaToDTO(c));
                }
                return listaCategoriaDto;
            }
            catch (Exception e)
            {
                // Registra el error y lanza una excepción
                Log.escribirEnFicheroLog("[ERROR ConversionDTOImpl - listaCategoriaToDto()] - Error al convertir lista de Categoria DAO a lista de CategoriaDTO: " + e);
                throw;
            }
        }
    }
}
