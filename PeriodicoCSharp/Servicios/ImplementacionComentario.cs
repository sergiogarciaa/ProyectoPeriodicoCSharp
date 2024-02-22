using DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeriodicoCSharp.Servicios
{
    public class ImplementacionComentario : InterfazComentario
    {
        public List<Comentario> obtenerComentariosPorNoticia(long idNoticia)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine($"[Error ImplementacionComentario - obtenerComentariosPorNoticia()] {e.Message}");
                throw; 
            }
        }
    }
}
