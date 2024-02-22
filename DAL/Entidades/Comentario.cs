using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Comentario
{
    public long IdComentario { get; set; }

    public string? DescComentario { get; set; }

    public string? FchPublicacionComentario { get; set; }

    public long IdUsuario { get; set; } // ID del usuario que publica el comentario
    public virtual Usuario Usuario { get; set; } // Propiedad de navegación para el usuario

    // Nuevo campo para la relación con Noticia
    public long Id_Noticia { get; set; }
    public virtual Noticia IdNoticiaNavigation { get; set; }
}
