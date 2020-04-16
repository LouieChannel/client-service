using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ascalon.ClientService.Infrastructure
{
    public class AuthOptions
    {
        /// <summary>
        /// Возвращает или задаёт издатель токена.
        /// </summary>
        public const string ISSUER = "Ascalon.ClientService";

        /// <summary>
        /// Возвращает или задаёт потребитель токена.
        /// </summary>
        public const string AUDIENCE = "Ascalon.ClientWeb";

        /// <summary>
        /// Возвращает или задаёт ключ для шифрации.
        /// </summary>
        const string KEY = "ascalon.clientservice";

        public const int LIFETIME = 60000;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
