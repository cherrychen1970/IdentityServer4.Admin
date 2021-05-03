using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Bluebird.Repositories;
using Bluebird.Linq;
using Skoruba.Helpers;
using Skoruba.IdentityServer4.Models;
using Skoruba.IdentityServer4.Resources;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;

namespace Skoruba.IdentityServer4.Services
{
    public class ClientService : ClientBaseService
    {
        //protected readonly IClientRepository ClientRepository;
        protected readonly IClientServiceResources ClientServiceResources;
        private const string SharedSecret = "SharedSecret";

        public ClientService(IClientServiceResources clientServiceResources,
        AdminConfigurationDbContext dbContext, IMapper mapper,
        ILogger<ClientService> logger)
         : base(dbContext, mapper, logger)
        {
            ClientServiceResources = clientServiceResources;
        }

        private void HashClientSharedSecret(ClientSecretDto clientSecret)
        {
            if (clientSecret.Type != SharedSecret) return;

            if (clientSecret.HashType == HashType.Sha256)
            {
                clientSecret.Value = clientSecret.Value.Sha256();
            }
            else if (clientSecret.HashType == HashType.Sha512)
            {
                clientSecret.Value = clientSecret.Value.Sha512();
            }
        }

        private void PrepareClientTypeForNewClient(ClientDto client)
        {
            switch (client.ClientType)
            {
                case ClientType.Empty:
                    break;
                case ClientType.Web:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = true;
                    break;
                case ClientType.Spa:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case ClientType.Native:
                    client.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    break;
                case ClientType.Machine:
                    client.AllowedGrantTypes.AddRange(GrantTypes.ClientCredentials);
                    break;
                case ClientType.Device:
                    client.AllowedGrantTypes.AddRange(GrantTypes.DeviceFlow);
                    client.RequireClientSecret = false;
                    client.AllowOfflineAccess = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PopulateClientRelations(ClientDto client)
        {
            ComboBoxHelpers.PopulateValuesToList(client.AllowedScopesItems, client.AllowedScopes);
            ComboBoxHelpers.PopulateValuesToList(client.PostLogoutRedirectUrisItems, client.PostLogoutRedirectUris);
            ComboBoxHelpers.PopulateValuesToList(client.IdentityProviderRestrictionsItems, client.IdentityProviderRestrictions);
            ComboBoxHelpers.PopulateValuesToList(client.RedirectUrisItems, client.RedirectUris);
            ComboBoxHelpers.PopulateValuesToList(client.AllowedCorsOriginsItems, client.AllowedCorsOrigins);
            ComboBoxHelpers.PopulateValuesToList(client.AllowedGrantTypesItems, client.AllowedGrantTypes);
        }

        public virtual ClientCloneDto BuildClientCloneViewModel(int id, ClientDto clientDto)
        {
            var client = new ClientCloneDto
            {
                CloneClientCorsOrigins = true,
                CloneClientGrantTypes = true,
                CloneClientIdPRestrictions = true,
                CloneClientPostLogoutRedirectUris = true,
                CloneClientRedirectUris = true,
                CloneClientScopes = true,
                CloneClientClaims = true,
                CloneClientProperties = true,
                ClientIdOriginal = clientDto.ClientId,
                ClientNameOriginal = clientDto.ClientName,
                Id = id
            };

            return client;
        }
        /*
                public virtual ClientSecretsDto BuildClientSecretsViewModel(ClientSecretsDto clientSecrets)
                {
                    clientSecrets.HashTypes = GetHashTypes();
                    clientSecrets.TypeList = GetSecretTypes();

                    return clientSecrets;
                }
        */
        public virtual ClientDto BuildClientViewModel(ClientDto client = null)
        {
            if (client == null)
            {
                var clientDto = new ClientDto
                {
                    AccessTokenTypes = GetAccessTokenTypes(),
                    RefreshTokenExpirations = GetTokenExpirations(),
                    RefreshTokenUsages = GetTokenUsage(),
                    ProtocolTypes = GetProtocolTypes(),
                    Id = 0
                };

                return clientDto;
            }

            client.AccessTokenTypes = GetAccessTokenTypes();
            client.RefreshTokenExpirations = GetTokenExpirations();
            client.RefreshTokenUsages = GetTokenUsage();
            client.ProtocolTypes = GetProtocolTypes();

            PopulateClientRelations(client);

            return client;
        }

        //

        public void UpdateClaims(int id, ClientClaimDto[] claims)
        {
            throw new NotImplementedException();
            /*
            var claimsInput = _mapper.Map<ClientClaim[]>(claims);
            foreach (var item in claimsInput)
                item.ClientId = id;

            var oldClaims = _context.ClientClaims.Where(x => x.ClientId == id).ToHashSet();
            _context.UpdateCollection<ClientClaim>(oldClaims, claimsInput, (x, y) => x.Id == y.Id);
        */
        }

        public dynamic GetRedirectUris(int id)
        {
            var list = _context.ClientRedirectUris.Where(x => x.ClientId == id).Select(x => x.RedirectUri);
            return new { Id = id, RedirectUris = list };

        }
        public void UpdateRedirectUris(int id, string[] redirectUris)
        {
            throw new NotImplementedException();
            var input = redirectUris.Select(x => new ClientRedirectUri { RedirectUri = x, ClientId = id, Id = 0 }).ToArray();
            var old = _context.ClientRedirectUris.Where(x => x.ClientId == id).ToHashSet();
            //_context.UpdateCollection<ClientRedirectUri>(old, input, (x, y) => x.RedirectUri == y.RedirectUri, false);
        }
        public dynamic GetScopes(int id)
        {
            var list = _context.ClientScopes.Where(x => x.ClientId == id).Select(x => x.Scope);
            return new { Id = id, AllowedScopes = list };
        }
        public void UpdateScopes(int id, string[] scopes)
        {
            throw new NotImplementedException();
            var input = scopes.Select(x => new ClientScope { Scope = x, ClientId = id, Id = 0 }).ToArray();
            var old = _context.ClientScopes.Where(x => x.ClientId == id).ToHashSet();
           // _context.UpdateCollection<ClientScope>(old, input, (x, y) => x.Scope == y.Scope, false);
        }
        //allowedGrantTypes
        public dynamic GetGrantTypes(int id)
        {
            var list = _context.ClientGrantTypes.Where(x => x.ClientId == id).Select(x => x.GrantType);
            return new { Id = id, AllowedGrantTypes = list };
        }
        public void UpdateGrantTypes(int id, string[] grantTypes)
        {
            throw new NotImplementedException();
            var input = grantTypes.Select(x => new ClientGrantType { GrantType = x, ClientId = id, Id = 0 }).ToArray();
            var old = _context.ClientGrantTypes.Where(x => x.ClientId == id).ToHashSet();
            //_context.UpdateCollection<ClientGrantType>(old, input, (x, y) => x.GrantType == y.GrantType, false);
        }
        public dynamic GetSecrets(int id)
        {
            var list = _context.ClientSecrets.Where(x => x.ClientId == id)
                .ProjectTo<ClientSecretDto>(_mapperProvider).ToArray();
              return new { Id = id, ClientSecrets = list };
        }
        public void UpdateSecrets(int id, ClientSecretDto[] secrets)
        {
            foreach (var item in secrets) 
                HashClientSharedSecret(item);

            var input = _mapper.Map<ClientSecret[]>(secrets);

            foreach (var item in input) 
                item.ClientId = id;

            var old = _context.ClientSecrets.Where(x => x.ClientId == id).ToHashSet();
            throw new NotImplementedException();
           // _context.UpdateCollection<ClientSecret>(old, input, (x, y) => x.Id == y.Id);
        }


    }
}