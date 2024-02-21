using DAL.Entidades;
using PeriodicoCSharp.DTO;

namespace PeriodicoCSharp.Servicios
{
    public interface ConversionDTO
    {
        /// <summary>
        /// Convierte un objeto Usuario de la capa de acceso a datos en un objeto UsuarioDTO.
        /// </summary>
        /// <param name="u">Objeto Usuario a convertir.</param>
        /// <returns>Objeto UsuarioDTO convertido.</returns>
        public UsuarioDTO usuarioToDto(Usuario u);

        /// <summary>
        /// Convierte una lista de objetos Usuario de la capa de acceso a datos en una lista de objetos UsuarioDTO.
        /// </summary>
        /// <param name="listaUsuario">Lista de objetos Usuario a convertir.</param>
        /// <returns>Lista de objetos UsuarioDTO convertidos.</returns>
        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario);

        NoticiaDTO noticiaToDto(Noticia noticia);
        public List<NoticiaDTO> listaNoticiasToDto(List<Noticia> listaNoticia);

        CategoriaDTO categoriaToDTO(Categoria categoria);
        public List<CategoriaDTO> listaCategoriaToDto(List<Categoria> listaCategoria);
    }
}
