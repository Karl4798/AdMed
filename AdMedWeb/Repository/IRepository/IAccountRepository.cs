using System.Threading.Tasks;
using AdMedWeb.Models;
using AdMedWeb.Models.ViewModels;

namespace AdMedWeb.Repository.IRepository
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User objToLogin);
        Task<bool?> RegisterAsync(string url, User objToCreate);
        Task<bool> ResetPasswordAsync(string url, ResetPasswordViewModel objToCreate, string token);
    }
}