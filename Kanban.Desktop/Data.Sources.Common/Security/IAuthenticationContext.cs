using System.Threading.Tasks;

namespace Data.Sources.Common
{
    public interface IAuthenticationContext
    {
        bool Login(string username, string password);

        Task<bool> LoginAsync(string username, string password);

        string GetToken();
    }
}