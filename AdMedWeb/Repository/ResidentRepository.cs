using System.Net.Http;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;

namespace AdMedWeb.Repository
{
    public class ResidentRepository : Repository<Resident>, IResidentRepository
    {

        private readonly IHttpClientFactory _clientFactory;

        public ResidentRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

    }
}
