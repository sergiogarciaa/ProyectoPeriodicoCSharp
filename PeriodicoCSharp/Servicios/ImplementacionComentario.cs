using DAL.Entidades;

namespace PeriodicoCSharp.Servicios
{
    public class ImplementacionComentario : InterfazComentario
    {
        public List<Comentario> obtenerComentariosPorNoticia(long idNoticia)
        {
            using (var contexto = new PeriodicoContext())
            {
                // Consulta para obtener los comentarios asociados a la noticia
                var comentarios = contexto.NoticiaComentarios
                    .Where(nc => nc.IdNoticia == idNoticia)
                    .Select(nc => nc.IdComentarioNavigation)
                    .ToList();

                return comentarios;
            }
        }
    }
}
