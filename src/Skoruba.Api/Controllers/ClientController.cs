using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.EntityFramework.Repositories;

using Skoruba.IdentityServer4.Models;

namespace Skoruba.Admin.Api.Controllers
{
    [Route("api/[controller]")]    
    public class ClientsController: RestController<ClientRepository, Client>
    {
        public ClientsController(ClientRepository repository,ILogger<BaseController> logger)
        :base(repository, logger)
        {
        }
    } 
}