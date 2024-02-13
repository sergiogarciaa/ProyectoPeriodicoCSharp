using System.Security.Cryptography;
using System.Text;

namespace PeriodicoCSharp.Servicios
{
    public class ImplementacionEncriptar : InterfazEncriptar
    {
        public string Encriptar(string texto)
        {
            try
            {

                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(texto);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return hash;
                }
            }
            catch (ArgumentException e)
            {
                return null;
            }
        }
    }
}
