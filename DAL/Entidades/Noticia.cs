using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Noticia
{
    public Noticia()
    {
        Comentarios = new List<Comentario>(); // Inicializar la colección en el constructor
    }

    public bool? RequiereSuscripcion { get; set; }

    public long IdCategoriaNoticia { get; set; }

    public long IdNoticia { get; set; }

    public long IdUsuarioNoticia { get; set; }

    public string TituloNoticia { get; set; } = null!;

    public string? DescNoticia { get; set; }

    public string? FchPublicacion { get; set; }

    public byte[]? ImagenNoticia { get; set; }

    public virtual Categoria IdCategoriaNoticiaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNoticiaNavigation { get; set; } = null!;

    public virtual ICollection<Comentario> Comentarios { get; set; }
}
