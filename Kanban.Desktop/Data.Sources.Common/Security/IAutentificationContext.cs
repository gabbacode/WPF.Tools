namespace Data.Sources.Common
{
    public interface IAutentificationContext
    {
        // TODO do secure login
        bool Login(string username, string password);
    }
}