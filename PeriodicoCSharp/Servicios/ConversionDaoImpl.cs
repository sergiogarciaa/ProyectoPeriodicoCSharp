using DAL.Entidades;
using PeriodicoCSharp.DTO;

namespace PeriodicoCSharp.Servicios
{
    public class ConversionDaoImpl : ConversionDao
    {

        public Noticia noticiaToDao(NoticiaDTO noticiaDTO, Usuario usuario, Categoria categoria)
        {
            try
            {
                Noticia noticiaToDao = new Noticia();
                noticiaToDao.IdNoticia = noticiaDTO.IdNoticia;
                noticiaToDao.TituloNoticia = noticiaDTO.TituloNoticia;
                noticiaToDao.DescNoticia = noticiaDTO.DescNoticia;
                noticiaToDao.RequiereSuscripcion = noticiaDTO.RequiereSuscripcion;
                noticiaToDao.FchPublicacion = noticiaDTO.FchPublicacion;

                noticiaToDao.IdUsuarioNoticiaNavigation = usuario;
                noticiaToDao.IdCategoriaNoticiaNavigation = categoria;

                return noticiaToDao;

            }
            catch (Exception e)
            {
                Console.WriteLine("\n[ERROR UsuarioToDaoImpl - toUsuarioDao()] - Al convertir usuarioDTO a usuarioDAO (return null): " + e);
                return null;
            }
        }

        public List<Usuario> listUsuarioToDao(List<UsuarioDTO> listaUsuarioDTO)
        {
            List<Usuario> listaUsuarioDao = new List<Usuario>();

            try
            {
                foreach (UsuarioDTO usuarioDTO in listaUsuarioDTO)
                {
                    listaUsuarioDao.Add(usuarioToDao(usuarioDTO));
                }

                return listaUsuarioDao;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n[ERROR UsuarioToDaoImpl - toListUsuarioDao()] - Al convertir lista de usuarioDTO a lista de usuarioDAO (return null): " + e);
                return null;
            }
        }

        public Usuario usuarioToDao(UsuarioDTO usuarioDTO)
        {
            Usuario usuarioDao = new Usuario();

            try
            {
                usuarioDao.IdUsuario = usuarioDTO.Id;
                usuarioDao.NombreUsuario = usuarioDTO.NombreUsuario;
                usuarioDao.ApellidosUsuario = usuarioDTO.ApellidosUsuario;
                usuarioDao.EmailUsuario = usuarioDTO.EmailUsuario;
                usuarioDao.ClaveUsuario = usuarioDTO.ClaveUsuario;
                usuarioDao.TlfUsuario = usuarioDTO.TlfUsuario;
                usuarioDao.DniUsuario = usuarioDTO.DniUsuario;
                usuarioDao.Rol = usuarioDTO.Rol;
                usuarioDao.CuentaConfirmada = usuarioDTO.CuentaConfirmada;

                return usuarioDao;

            }
            catch (Exception e)
            {
                Console.WriteLine("\n[ERROR UsuarioToDaoImpl - toUsuarioDao()] - Al convertir usuarioDTO a usuarioDAO (return null): " + e);
                return null;
            }
        }
    }
}
