using System;
using System.Collections.Generic;

namespace DAL.Entidades;

public partial class Usuario
{
    public bool CuentaConfirmada { get; set; }

    public bool? EstadoSuscripcion { get; set; }

    public DateTime? ExpiracionToken { get; set; }

    public DateTime? FchAltaUsuario { get; set; }

    public DateTime? FchBajaUsuario { get; set; }

    public long IdUsuario { get; set; }

    public string DniUsuario { get; set; } = null!;

    public string? TlfUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string? ApellidosUsuario { get; set; }

    public string ClaveUsuario { get; set; } = null!;

    public string EmailUsuario { get; set; } = null!;

    public string? TokenRecuperacion { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<Noticia> Noticia { get; set; } = new List<Noticia>();
}
