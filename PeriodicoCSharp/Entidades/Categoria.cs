﻿using System;
using System.Collections.Generic;

namespace PeriodicoCSharp.Entidades;

public partial class Categoria
{
    public long IdCategoria { get; set; }

    public string? DescCategoria { get; set; }

    public string TipoCategoria { get; set; } = null!;

    public virtual ICollection<Noticia> Noticia { get; set; } = new List<Noticia>();
}