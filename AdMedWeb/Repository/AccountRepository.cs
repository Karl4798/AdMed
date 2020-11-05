using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AdMedWeb.Repository.IRepository;
using AdMedWeb.Models;
using AdMedWeb.Models.ViewModels;
using System.Net.Http.Headers;

namespace AdMedWeb.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<User> LoginAsync(string url, User objToLogin)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToLogin != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToLogin), Encoding.UTF8, "application/json");
            }
            else
            {
                return new User();
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);
            }
            return new User();
        }

        public async Task<bool?> RegisterAsync(string url, User objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return null;
            }
            return false;
        }

        public async Task<bool> ResetPasswordAsync(string url, ResetPasswordViewModel objToResetPassword, string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToResetPassword != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToResetPassword), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
