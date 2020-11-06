using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.EntityFramework.Repositories;

using Skoruba.IdentityServer4.Models;

namespace Skoruba.Admin.Api.Controllers
{
    [Route("_api/[controller]")]    
    public class ClientsController: RestController<ClientRepository, ClientDto,int>
    {
        public ClientsController(ClientRepository repository,ILogger<BaseController> logger)
        :base(repository, logger)
        {
        }
    } 
}