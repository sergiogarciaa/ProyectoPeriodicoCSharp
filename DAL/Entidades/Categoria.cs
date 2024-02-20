using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Categoria
{
    public long IdCategoria { get; set; }

    public string? DescCategoria { get; set; }

    public string TipoCategoria { get; set; }

    public virtual ICollection<Noticia> Noticia { get; set; } = new List<Noticia>();
}
