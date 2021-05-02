using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using IdModels = IdentityServer4.Models;
using Bluebird.Entity;

namespace id4.Models.Dto
{
   public class ClientDto : IKey<int>
    {
        public ClientDto(){}

        public int AccessTokenLifetime { get; set; }=3600;
        public int AuthorizationCodeLifetime { get; set; }=300;
        public int? ConsentLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }=2592000;
        public int SlidingRefreshTokenLifetime { get; set; }=1296000;
        public int RefreshTokenUsage { get; set; }=1;
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }=false;
        public int RefreshTokenExpiration { get; set; }=1;
        public int AccessTokenType { get; set; }=0;
        public bool EnableLocalLogin { get; set; }=true;
        public bool IncludeJwtId { get; set; }=true;
        public bool AlwaysSendClientClaims { get; set; }=false;
        public string ClientClaimsPrefix { get; set; }="client_";
        public string PairWiseSubjectSalt { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public int? UserSsoLifetime { get; set; }
        public string UserCodeType { get; set; }
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }
        public int IdentityTokenLifetime { get; set; }=300;
        public ICollection<string> AllowedScopes { get; set; }
        public bool AllowOfflineAccess { get; set; }=false;
        public int Id { get; set; }
        public bool Enabled { get; set; }=true;
        public string ClientId { get; set; }
        public string ProtocolType { get; set; }="oidc";
        public ICollection<SecretDto> ClientSecrets { get; set; }
        public ICollection<string> Secrets { get; set; }
        public bool ShouldSerializeSecrets() => false;
        public bool RequireClientSecret { get; set; }=true;
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool RequireConsent { get; set; }=false;
        public int DeviceCodeLifetime { get; set; }=300;
        public bool AllowRememberConsent { get; set; }=true;
        public ICollection<string> AllowedGrantTypes { get; set; }=new[] {"authorization_code"};
        public bool RequirePkce { get; set; }=true;
        public bool AllowPlainTextPkce { get; set; }
        public bool RequireRequestObject { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public ICollection<string> RedirectUris { get; set; }
        public string FrontChannelLogoutUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; }
        public string BackChannelLogoutUri { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; }=true;
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public bool NonEditable { get; set; }
    }

}
