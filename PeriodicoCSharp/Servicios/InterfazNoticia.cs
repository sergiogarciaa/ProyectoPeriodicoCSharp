using DAL.Entidades;
using PeriodicoCSharp.DTO;
using System.Runtime.InteropServices;

namespace PeriodicoCSharp.Servicios
{
    public interface InterfazNoticia
    {
        /**
	     * Busca a una noticia por ID
	     * @param id de la noticia a buscar
	     * @return La noticia
	     */
        public Noticia buscarNoticiaPorID(long id);
        /**
         * Busca todas las noticias
         * @return la lista de todas las noticias
         */
        public List<NoticiaDTO> buscarTodas();
        public String resumirNoticia(String texto);
        public String resumirNoticia2(String texto)
            ;
        List<NoticiaDTO> buscarPorCategoria(long idCategoria);

        public String convertToBase64(byte[] bytes);

        public byte[] convertToByteArray(String foto);
        public Noticia obtenerNoticiaMasReciente();
    }
}
