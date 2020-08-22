using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdMedWeb.Models;

namespace AdMedWeb.Repository.IRepository
{
    public interface IAccountRepository : IRepository<User>
    {

        Task<User> LoginAsync(string url, User objToLogin);
        Task<bool> RegisterAsync(string url, User objToCreate);

    }
}
