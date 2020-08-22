using System.Net.Http;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;

namespace AdMedWeb.Repository
{
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {

        private readonly IHttpClientFactory _clientFactory;

        public ApplicationRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

    }
}
