using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class NoticiaComentario
{
    public long IdComentario { get; set; }

    public long IdNoticia { get; set; }

    public virtual Comentario IdComentarioNavigation { get; set; } = null!;

    public virtual Noticia IdNoticiaNavigation { get; set; } = null!;
}
