using DAL.Entidades;

namespace PeriodicoCSharp.Servicios
{
    public interface InterfazComentario
    {
        List<Comentario> obtenerComentariosPorNoticia(long idNoticia);
    }
}
