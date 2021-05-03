using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityServer4.EntityFramework.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bluebird.AspNetCore.Mvc;
using Bluebird.Helpers;
using Skoruba.IdentityServer4.EntityFramework.Repositories;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;
using Skoruba.IdentityServer4.Models;
using Skoruba.IdentityServer4.Services;

namespace Skoruba.Admin.Api.Controllers
{
    [Route("_api/[controller]")]
    public class ClientsController : RestAsyncController<ClientService, ClientDto, Client,int>
    {
        IMapper _mapper1;
        IConfigurationProvider _mapper;
        AdminConfigurationDbContext _context;
        //ClientClaimRepository _claimRepository;
        public ClientsController(
            ClientService repository,
            AdminConfigurationDbContext context,
            Bluebird.Auth.IUserSession session,
            //ClientClaimRepository claimRepository,
        IMapper mapper,
        ILogger<BaseController> logger)
        : base(repository,mapper, session, logger)
        {
            _mapper1 = mapper;
            _mapper = mapper.ConfigurationProvider;
            _context = context;
            //_claimRepository = claimRepository;
        }

        [HttpGet("claims/{id}")]
        public async Task<IActionResult> GetClaims(int id)
        {
            var item = _repository.GetOne<ClientClaimsDto>(id);
            return Ok(item);
        }

        [HttpPut("claims/{id}")]
        public async Task<IActionResult> UpdateClaims(int id, ClientClaimsDto input)
        {
            _repository.UpdateClaims(id, input.Claims.ToArray());
            await _repository.SaveChangesAsync();
            return await GetClaims(id);
        }
        [HttpGet("redirectUris/{id}")]
        public async Task<IActionResult> GetRedirectUris(int id)
        {
            var item = _repository.GetRedirectUris(id);
            return Ok(item);
        }

        [HttpPut("redirectUris/{id}")]
        public async Task<IActionResult> UpdateRedirectUris(int id, ExpandoObject input)
        {
            var data = input.AsDynamic<ClientDto>();
            _repository.UpdateRedirectUris(id, data.RedirectUris.ToArray());
            await _repository.SaveChangesAsync();
            return await GetRedirectUris(id);
        }
        [HttpGet("scopes/{id}")]
        public async Task<IActionResult> GetScopes(int id)
        {
            var item = _repository.GetScopes(id);
            return Ok(item);
        }

        [HttpPut("scopes/{id}")]
        public async Task<IActionResult> UpdateScopes(int id, ExpandoObject input)
        {
            var data = input.AsDynamic<ClientDto>();
            _repository.UpdateScopes(id, data.AllowedScopes.ToArray());
            await _repository.SaveChangesAsync();
            return await GetScopes(id);
        }
        [HttpGet("grantTypes/{id}")]
        public async Task<IActionResult> GetGrantTypes(int id)
        {
            var item = _repository.GetGrantTypes(id);
            return Ok(item);
        }
        [HttpPut("grantTypes/{id}")]
        public async Task<IActionResult> UpdateGrantTypes(int id, ExpandoObject input)
        {
            var data = input.AsDynamic<ClientDto>();
            _repository.UpdateGrantTypes(id, data.AllowedGrantTypes.ToArray());
            await _repository.SaveChangesAsync();
            return await GetGrantTypes(id);
        }
        [HttpGet("secrets/{id}")]
        public async Task<IActionResult> GetSecrets(int id)
        {
            var item = _repository.GetSecrets(id);
            return Ok(item);
        }
        [HttpPut("secrets/{id}")]
        public async Task<IActionResult> UpdateSecrets(int id, ExpandoObject input)
        {
            var data = input.AsDynamic<ClientDto>();
            var secrets = _mapper1.Map<List<ClientSecretDto>>(data.ClientSecrets);
            _repository.UpdateSecrets(id, secrets.ToArray());
            await _repository.SaveChangesAsync();
            return await GetSecrets(id);
        }        
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> GetEdit(int id)
        {
            var item = _repository.GetOne<ClientEditDto>(id);
            return Ok(item);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Update(int id, ClientEditDto input)
        {
            _repository.UpdateScopes(id, input.AllowedScopes.ToArray());
            new NotImplementedException();
            //_repository.Update(id, input);
            await _repository.SaveChangesAsync();
            return await GetEdit(id);
        }
    }
}