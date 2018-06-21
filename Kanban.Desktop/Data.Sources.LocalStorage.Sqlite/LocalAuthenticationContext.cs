using Data.Sources.Common;
using Data.Sources.Common.Redmine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Sources.LocalStorage.Sqlite
{
    class LocalAuthenticationContext : IAuthenticationContext
    {
        public string GetToken()
        {
            throw new System.NotSupportedException();
        }

        public bool Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        private IRepository SqliteRepository;
    }
}
