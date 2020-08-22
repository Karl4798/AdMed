using System.Net.Http;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;

namespace AdMedWeb.Repository
{
    public class EmergencyContactRepository : Repository<EmergencyContact>, IEmergencyContactRepository
    {

        private readonly IHttpClientFactory _clientFactory;

        public EmergencyContactRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

    }

}
