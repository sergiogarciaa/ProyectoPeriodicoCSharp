using System;
using System.Collections.Generic;

namespace PeriodicoCSharp.Entidades;

public partial class Comentario
{
    public long IdComentario { get; set; }

    public string? DescComentario { get; set; }

    public string? FchPublicacionComentario { get; set; }
}
