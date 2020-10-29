using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using Skoruba.Models;
using Skoruba.Linq.Extensions;
using Skoruba.IdentityServer4.Models;

namespace Skoruba.IdentityServer4.Services
{
    public class ClientBaseService //: IClientRepository    
    {
        //protected readonly ConfDbContext DbContext;        
        public ClientBaseService()
        {
            //DbContext = dbContext;
        }

        public virtual List<string> GetGrantTypes(string grant, int limit = 0)
        {
            var filteredGrants = ClientConsts.GetGrantTypes()
                .WhereIf(!string.IsNullOrWhiteSpace(grant), x => x.Contains(grant))
                .TakeIf(x => x, limit > 0, limit)
                .ToList();

            return filteredGrants;
        }

        public virtual List<SelectItem> GetProtocolTypes()
        {
            return ClientConsts.GetProtocolTypes();
        }

        public virtual List<SelectItem> GetSecretTypes()
        {
            var secrets = new List<SelectItem>();
            secrets.AddRange(ClientConsts.GetSecretTypes().Select(x => new SelectItem(x, x)));

            return secrets;
        }

        public virtual List<string> GetStandardClaims(string claim, int limit = 0)
        {
            var filteredClaims = ClientConsts.GetStandardClaims()
                .WhereIf(!string.IsNullOrWhiteSpace(claim), x => x.Contains(claim))
                .TakeIf(x => x, limit > 0, limit)
                .ToList();

            return filteredClaims;
        }

        public virtual List<SelectItem> GetAccessTokenTypes()
        {
            var accessTokenTypes =  SelectList.From<AccessTokenType>();
            return accessTokenTypes;
        }

        public virtual List<SelectItem> GetTokenExpirations()
        {
            var tokenExpirations = typeof(TokenCreationRequest).ToSelectList();
            return tokenExpirations;
        }

        public virtual List<SelectItem> GetTokenUsage()
        {
            var tokenUsage = typeof(TokenUsage).ToSelectList();
            return tokenUsage;
        }

        public virtual List<SelectItem> GetHashTypes()
        {
            var hashTypes = typeof(HashType).ToSelectList();
            return hashTypes;
        }
    }
}