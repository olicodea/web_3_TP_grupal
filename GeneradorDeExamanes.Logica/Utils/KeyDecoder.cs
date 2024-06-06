using System.Text;

namespace GeneradorDeExamanes.Logica.Utils;

public class KeyDecoder
{
    public string Decode(string base64EncodedPassword)
    {
        if (string.IsNullOrEmpty(base64EncodedPassword))
        {
            throw new ArgumentNullException(nameof(base64EncodedPassword));
        }

        try
        {
            byte[] data = Convert.FromBase64String(base64EncodedPassword);
            return Encoding.UTF8.GetString(data);
        }
        catch (FormatException ex)
        {
            throw new Exception("Error al decodificar la contraseña. Asegúrate de que la contraseña esté codificada en base64.", ex);
        }
    }
}