using System.Net.Http;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;

namespace AdMedWeb.Repository
{
    public class MedicationRepository : Repository<Medication>, IMedicationRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public MedicationRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}