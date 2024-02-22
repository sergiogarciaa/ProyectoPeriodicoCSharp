using DAL.Entidades;
using PeriodicoCSharp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeriodicoCSharp.Servicios
{
    public class ImplementacionComentario : InterfazComentario
    {
        /// <summary>
        /// Obtiene los comentarios asociados a una noticia por su ID.
        /// </summary>
        /// <param name="idNoticia">ID de la noticia.</param>
        /// <returns>Lista de comentarios asociados a la noticia.</returns>
        public List<Comentario> obtenerComentariosPorNoticia(long idNoticia)
        {
            try
            {
                Log.escribirEnFicheroLog("Entrando al método ObtenerComentariosPorNoticia() en ImplementacionComentario");
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
                Log.escribirEnFicheroLog("[Error ImplementacionComentario - ObtenerComentariosPorNoticia()]" +  e.Message);

                throw; 
            }
        }
    }
}
