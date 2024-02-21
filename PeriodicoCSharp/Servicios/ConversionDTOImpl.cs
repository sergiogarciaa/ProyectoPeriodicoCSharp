using DAL.Entidades;
using PeriodicoCSharp.DTO;

namespace PeriodicoCSharp.Servicios
{
    public class ConversionDTOImpl : ConversionDTO
    {
        public List<UsuarioDTO> listaUsuarioToDto(List<Usuario> listaUsuario)
        {
            try
            {
                List<UsuarioDTO> listaDto = new List<UsuarioDTO>();
                foreach (Usuario u in listaUsuario)
                {
                    listaDto.Add(usuarioToDto(u));
                }
                return listaDto;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n[ERROR UsuarioToDtoImpl - ListaUsuarioToDto()] - Error al convertir lista de usuario DAO a lista de usuarioDTO (return null): " + e);
                return null;
            }
        }

        public UsuarioDTO usuarioToDto(Usuario u)
        {
            try
            {
                UsuarioDTO dto = new UsuarioDTO();
                dto.NombreUsuario = u.NombreUsuario;
                dto.ApellidosUsuario = u.ApellidosUsuario;
                dto.DniUsuario = u.DniUsuario;
                dto.TlfUsuario = u.TlfUsuario;
                dto.EmailUsuario = u.EmailUsuario;
                dto.ClaveUsuario = u.ClaveUsuario;
                dto.TokenRecuperacion = u.TokenRecuperacion;
                dto.ExpiracionToken = u.ExpiracionToken;
                dto.FchAltaUsuario = u.FchAltaUsuario;
                dto.Rol = u.Rol;
                dto.Id = u.IdUsuario;
                dto.CuentaConfirmada = u.CuentaConfirmada;
                return dto;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n[ERROR UsuarioToDtoImpl - usuarioToDto()] - Error al convertir usuario DAO a usuarioDTO (return null): " + e);
                return null;
            }
        }

        public NoticiaDTO noticiaToDto(Noticia noticia)
        {
            try
            {
                NoticiaDTO dto = new NoticiaDTO();
                dto.IdNoticia = noticia.IdNoticia;
                dto.TituloNoticia = noticia.TituloNoticia;
                dto.DescNoticia = noticia.DescNoticia;
                dto.RequiereSuscripcion = noticia.RequiereSuscripcion;
                dto.FchPublicacion = noticia.FchPublicacion;
                dto.IdCategoriaNoticia = noticia.IdCategoriaNoticia;
                dto.IdUsuarioNoticia = noticia.IdUsuarioNoticia;

                return dto;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR ImplementacionNoticiaToDto - noticiaToDto()] - Error al convertir noticia DAO a noticiaDTO (return null): " + e);
                return null;
            }
        }

        public List<NoticiaDTO> listaNoticiasToDto(List<Noticia> listaNoticia)
        {
            try
            {
                List<NoticiaDTO> listaNoticiaDto = new List<NoticiaDTO>();
                foreach (Noticia n in listaNoticia)
                {
                    listaNoticiaDto.Add(noticiaToDto(n));
                }
                return listaNoticiaDto;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n[ERROR UsuarioToDtoImpl - ListaUsuarioToDto()] - Error al convertir lista de usuario DAO a lista de usuarioDTO (return null): " + e);
                return null;
            }
        }
    }
}
