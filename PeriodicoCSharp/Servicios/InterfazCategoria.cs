using DAL.Entidades;
using PeriodicoCSharp.DTO;

namespace PeriodicoCSharp.Servicios
{
    public interface InterfazCategoria
    {
        List<CategoriaDTO> BuscarTodas();
        Categoria BuscarPorIdCategoria(Categoria categoria);
        Categoria BuscarPorId(long idCategoria);
    }
}
