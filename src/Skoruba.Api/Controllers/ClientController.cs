using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using Skoruba.IdentityServer4.Models;

namespace Skoruba.Admin.Api.Controllers
{
    [Route("_api/[controller]")]
    public class ClientsController : RestController<ClientRepository, ClientDto, int>
    {
        IMapper _mapper1;
        IConfigurationProvider _mapper;
        AdminConfigurationDbContext _context;
        public ClientsController(
            ClientRepository repository,
            AdminConfigurationDbContext context,
        IMapper mapper,
        ILogger<BaseController> logger)
        : base(repository, logger)
        {
            _mapper1 = mapper;
            _mapper = mapper.ConfigurationProvider;
            _context = context;
        }

        [HttpGet("test/{id}")]
        public IActionResult GetOne(int id)
        {
            var item =_repository.GetOne(id,new[] {"id"});

            return Ok(item);
        }
    }
}