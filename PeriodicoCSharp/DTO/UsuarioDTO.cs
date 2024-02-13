namespace PeriodicoCSharp.DTO
{
    public class UsuarioDTO
    {
        public long Id { get; set; }
        public bool CuentaConfirmada { get; set; }
        public bool? EstadoSuscripcion { get; set; }
        public DateTime? ExpiracionToken { get; set; }
        public DateTime? FchAltaUsuario { get; set; }
        public DateTime? FchBajaUsuario { get; set; }
        public long IdUsuario { get; set; }
        public string DniUsuario { get; set; }
        public string? TlfUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string? ApellidosUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string? TokenRecuperacion { get; set; }
        public string? Rol { get; set; }
    }
}
