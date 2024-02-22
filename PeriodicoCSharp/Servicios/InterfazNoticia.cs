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
        * Busca a una noticia por ID
        * @param id de la noticia a buscar
        * @return La noticia
        */
        public NoticiaDTO buscarNoticiaPorIDDTO(long id);
        /**
         * Busca todas las noticias
         * @return la lista de todas las noticias
         */
        public List<NoticiaDTO> buscarTodas();
        public List<NoticiaDTO> buscar4Primeras();
        public String resumirNoticia(String texto);
        List<NoticiaDTO> buscarPorCategoria(long idCategoria);

        public string ConvertToBase64(IFormFile file);

        public byte[] convertToByteArray(String foto);
        public Noticia obtenerNoticiaMasReciente();
        public void GuardarNoticia(Noticia noticia);
    }
}
