using System;
using System.Collections.Generic;

namespace PeriodicoCSharp.Entidades;

public partial class UsuarioComentario1
{
    public long IdComentario { get; set; }

    public long IdUsuario { get; set; }

    public virtual Comentario IdComentarioNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
