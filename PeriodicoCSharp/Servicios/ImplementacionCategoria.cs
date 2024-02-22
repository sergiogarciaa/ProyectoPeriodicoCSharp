using DAL.Entidades;
using PeriodicoCSharp.DTO;
using PeriodicoCSharp.Utils;

namespace PeriodicoCSharp.Servicios
{
    public class ImplementacionCategoria : InterfazCategoria
    {
        private readonly PeriodicoContext _contexto;
        private readonly ConversionDao _toDao;
        private readonly ConversionDTO _toDto;


        public ImplementacionCategoria(PeriodicoContext contexto, ConversionDao toDao, ConversionDTO toDto)
        {
            _contexto = contexto;
            _toDao = toDao;
            _toDto = toDto;
        }
        /// <summary>
        /// Busca una categoría por su ID.
        /// </summary>
        /// <param name="categoria">Categoría con el ID a buscar.</param>
        /// <returns>La categoría encontrada.</returns>
        public Categoria BuscarPorIdCategoria(Categoria categoria)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método BuscarPorIdCategoria() en ImplementacionNoticia");

                return _contexto.Categorias.FirstOrDefault(c => c.IdCategoria == categoria.IdCategoria);
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - BuscarPorIdCategoria()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Busca una categoría por su ID.
        /// </summary>
        /// <param name="idCategoria">ID de la categoría a buscar.</param>
        /// <returns>La categoría encontrada.</returns>
        public Categoria BuscarPorId(long idCategoria)
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método BuscarPorId() en ImplementacionNoticia");

                Categoria? categoria = _contexto.Categorias.FirstOrDefault(n => n.IdCategoria == idCategoria);
                return categoria;
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionNoticia - BuscarPorId()] {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Busca todas las categorías.
        /// </summary>
        /// <returns>Lista de todas las categorías.</returns>
        public List<CategoriaDTO> BuscarTodas()
        {
            try
            {
                 Log.escribirEnFicheroLog("Entrando al método BuscarTodas() en ImplementacionCategoria");

                return _toDto.listaCategoriaToDto(_contexto.Categorias.ToList());
            }
            catch (Exception e)
            {
                 Log.escribirEnFicheroLog($"[Error ImplementacionCategoria - BuscarTodos()] {e.Message}");
                return new List<CategoriaDTO>();
            }
        }
    }
}
