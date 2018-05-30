namespace Data.Sources.Common
{
    public interface IAuthenticationContext
    {
        // TODO do secure login
        bool Login(string username, string password);
    }
}