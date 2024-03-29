﻿using DAL.Entidades;
using PeriodicoCSharp.DTO;

namespace PeriodicoCSharp.Servicios
{
    public interface ConversionDao
    {

        /// <summary>
        /// Convierte un objeto UsuarioDTO en un objeto Usuario de la capa de acceso a datos.
        /// </summary>
        /// <param name="usuarioDTO">Objeto UsuarioDTO a convertir.</param>
        /// <returns>Objeto Usuario convertido.</returns>
        public Usuario usuarioToDao(UsuarioDTO usuarioDTO);

        /// <summary>
        /// Convierte una lista de objetos UsuarioDTO en una lista de objetos Usuario de la capa de acceso a datos.
        /// </summary>
        /// <param name="listaUsuarioDTO">Lista de objetos UsuarioDTO a convertir.</param>
        /// <returns>Lista de objetos Usuario convertidos.</returns>
        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO);

        public Noticia noticiaToDao(NoticiaDTO noticiaDTO, Usuario usuario, Categoria categoria);

    }
}
