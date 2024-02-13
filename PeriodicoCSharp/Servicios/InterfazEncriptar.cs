namespace PeriodicoCSharp.Servicios
{
    public interface InterfazEncriptar
    {
        /// <summary>
        /// Metodo encargado de encriptar el string recibido por argumento
        /// </summary>
        /// <param name="texto">String el cual se quiere encriptar</param>
        /// <returns>String encriptado</returns>
        public string Encriptar(string texto);
    }
}
