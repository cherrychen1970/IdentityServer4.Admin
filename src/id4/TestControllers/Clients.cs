using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Id4Entities=IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Id4Models = IdentityServer4.Models;
using Bluebird.Linq;
using Bluebird.Repositories;
using Bluebird.AspNetCore.Mvc;


namespace id4.Api.TestControllers
{
    [Route("_api/[controller]")]
    [ApiController]
    public partial class ClientsController : RestAsyncController<Id4Entities.Client,int>
    {
        public ClientsController(ClientRepo repo, ILogger<ClientsController> logger) 
            : base(repo,logger)
        {
        }
    }
}