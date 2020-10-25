using System.Net.Http;
using AdMedWeb.Models;
using AdMedWeb.Repository.IRepository;

namespace AdMedWeb.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {

        private readonly IHttpClientFactory _clientFactory;

        public PostRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

    }
}
