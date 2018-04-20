using Data.Sources.Common;
using Data.Sources.Common.Redmine;

namespace Data.Sources.Redmine
{
    public class RedmineAutentificationContext : IAutentificationContext
    {
        public RedmineAutentificationContext(IRedmineRepository redmineRepository)
        {
            RedmineRepository = redmineRepository;
        }

        public bool Login(string username, string password)
        {
            try
            {
                RedmineRepository.InitCredentials(username, password);

                var user = RedmineRepository.GetCurrentUser();

                return user != null;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        private IRedmineRepository RedmineRepository;
    }
}
