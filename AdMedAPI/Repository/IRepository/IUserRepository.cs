using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdMedAPI.Models;

namespace AdMedAPI.Repository.IRepository
{
    public interface IUserRepository
    {

        bool IsUniqueUser(string username);
        User Authenticate(string username, string password);
        User Register(string username, string password);

    }
}
