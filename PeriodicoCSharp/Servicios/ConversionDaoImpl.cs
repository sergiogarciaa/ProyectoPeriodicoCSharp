using DAL.Entidades;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Utils;
using System;
using System.Collections.Generic;

namespace PeriodicoCSharp.Servicios
{
    public class ConversionDaoImpl : ConversionDao
    {
        /// <summary>
        /// Convierte un objeto NoticiaDTO en un objeto Noticia para su almacenamiento en la base de datos.
        /// </summary>
        /// <param name="noticiaDTO">DTO de la noticia a convertir.</param>
        /// <param name="usuario">Usuario asociado a la noticia.</param>
        /// <param name="categoria">Categoría de la noticia.</param>
        /// <returns>Objeto Noticia convertido.</returns>
        public Noticia noticiaToDao(NoticiaDTO noticiaDTO, Usuario usuario, Categoria categoria)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método noticiaToDao() en ConversionDaoImpl");

                Noticia noticiaToDao = new Noticia();
                noticiaToDao.IdNoticia = noticiaDTO.IdNoticia;
                noticiaToDao.TituloNoticia = noticiaDTO.TituloNoticia;
                noticiaToDao.DescNoticia = noticiaDTO.DescNoticia;
                noticiaToDao.RequiereSuscripcion = noticiaDTO.RequiereSuscripcion;
                noticiaToDao.FchPublicacion = noticiaDTO.FchPublicacion;

                noticiaToDao.IdUsuarioNoticiaNavigation = usuario;
                noticiaToDao.IdCategoriaNoticiaNavigation = categoria;

                return noticiaToDao;
            }
            catch (Exception e)
            {
                // Registra el error y lanza una excepción
                Log.escribirEnFicheroLog("[ERROR ConversionDaoImpl - noticiaToDao()] - Error al convertir NoticiaDTO a NoticiaDAO: " + e);
                throw;
            }
        }

        /// <summary>
        /// Convierte una lista de objetos UsuarioDTO en una lista de objetos Usuario para su almacenamiento en la base de datos.
        /// </summary>
        /// <param name="listaUsuarioDTO">Lista de DTO de usuarios a convertir.</param>
        /// <returns>Lista de objetos Usuario convertidos.</returns>
        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método listUsuarioToDao() en ConversionDaoImpl");

                List<Usuario> listaUsuarioDao = new List<Usuario>();

                foreach (UsuarioDTO usuarioDTO in listaUsuarioDTO)
                {
                    listaUsuarioDao.Add(usuarioToDao(usuarioDTO));
                }

                return listaUsuarioDao;
            }
            catch (Exception e)
            {
                // Registra el error y lanza una excepción
                Log.escribirEnFicheroLog("[ERROR ConversionDaoImpl - listUsuarioToDao()] - Error al convertir lista de UsuarioDTO a lista de UsuarioDAO: " + e);
                throw;
            }
        }

        /// <summary>
        /// Convierte un objeto UsuarioDTO en un objeto Usuario para su almacenamiento en la base de datos.
        /// </summary>
        /// <param name="usuarioDTO">DTO de usuario a convertir.</param>
        /// <returns>Objeto Usuario convertido.</returns>
        public Usuario usuarioToDao(UsuarioDTO usuarioDTO)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método usuarioToDao() en ConversionDaoImpl");

                Usuario usuarioDao = new Usuario();
                usuarioDao.IdUsuario = usuarioDTO.Id;
                usuarioDao.NombreUsuario = usuarioDTO.NombreUsuario;
                usuarioDao.ApellidosUsuario = usuarioDTO.ApellidosUsuario;
                usuarioDao.EmailUsuario = usuarioDTO.EmailUsuario;
                usuarioDao.ClaveUsuario = usuarioDTO.ClaveUsuario;
                usuarioDao.TlfUsuario = usuarioDTO.TlfUsuario;
                usuarioDao.DniUsuario = usuarioDTO.DniUsuario;
                usuarioDao.Rol = usuarioDTO.Rol;
                usuarioDao.CuentaConfirmada = usuarioDTO.CuentaConfirmada;

                return usuarioDao;
            }
            catch (Exception e)
            {
                // Registra el error y lanza una excepción
                Log.escribirEnFicheroLog("[ERROR ConversionDaoImpl - usuarioToDao()] - Error al convertir UsuarioDTO a UsuarioDAO: " + e);
                throw;
            }
        }
    }
}
