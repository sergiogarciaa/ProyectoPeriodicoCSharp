using DAL.Entidades;
using PeriodicoCSharp.Servicios;

namespace PeriodicoCSharp.DTO
{
    public class NoticiaDTO
    {
        public bool? RequiereSuscripcion { get; set; }
        public long IdCategoriaNoticia { get; set; }
        public Categoria? idCategoria { get; set; }
        public Usuario? idUsuario { get; set; }
        public long IdNoticia { get; set; }
        public long IdUsuarioNoticia { get; set; }
        public string TituloNoticia { get; set; }
        public string? DescNoticia { get; set; }
        public string? resumenNoticia { get; set; }
        public string? FchPublicacion { get; set; }
        public byte[]? ImagenNoticia { get; set; }

    }
}
