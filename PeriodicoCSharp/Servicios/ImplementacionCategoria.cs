using DAL.Entidades;
using PeriodicoCSharp.DTO;

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
        public Categoria BuscarPorIdCategoria(Categoria categoria)
        {
            try
            {
                return _contexto.Categorias.FirstOrDefault(c => c.IdCategoria == categoria.IdCategoria);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - BuscarPorIdCategoria()] {e.Message}");
                return null;
            }
        }

        public Categoria BuscarPorId(long idCategoria)
        {
            try
            {
                Categoria? categoria = _contexto.Categorias.FirstOrDefault(n => n.IdCategoria == idCategoria);
                return categoria;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionNoticia - BuscarPorId()] {e.Message}");
                return null;
            }
        }

        public List<CategoriaDTO> BuscarTodas()
        {
            try
            {
                return _toDto.listaCategoriaToDto(_contexto.Categorias.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionCategoria - BuscarTodos()] {e.Message}");
                return new List<CategoriaDTO>();
            }
        }
    }
}
