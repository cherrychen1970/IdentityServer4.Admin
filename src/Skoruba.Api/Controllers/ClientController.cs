using Microsoft.Extensions.Logging;
using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Skoruba.IdentityServer4.Models;

namespace Skoruba.Admin.Api.Controllers
{
    public class ClientController: RestController<ClientRepository, ClientDto>
    {
        public ClientController(ClientRepository repository,ILogger<BaseController> logger)
        :base(repository, logger)
        {
        }
    } 
}