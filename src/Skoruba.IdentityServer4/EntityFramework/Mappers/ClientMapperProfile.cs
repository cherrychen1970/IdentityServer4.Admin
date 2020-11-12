// Based on the IdentityServer4.EntityFramework - authors - Brock Allen & Dominick Baier.
// https://github.com/IdentityServer/IdentityServer4.EntityFramework

// Modified by Jan Škoruba
using System.Linq;
using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using Skoruba.IdentityServer4.Models;
using Skoruba.Models;


namespace Skoruba.IdentityServer4.EntityFramework.Mappers
{

    public class ClientMapperProfile : Profile
    {
        public ClientMapperProfile()
        {            
            CreateMap<Client,test>();
            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.AllowedScopes, opt => opt.MapFrom(src=>src.AllowedScopes.Select(x=>x.Scope)) )
                .ForMember(dest => dest.AllowedGrantTypes, opt => opt.MapFrom(src=>src.AllowedGrantTypes.Select(x=>x.GrantType)) )
                .ForMember(dest => dest.RedirectUris, opt => opt.MapFrom(src=>src.RedirectUris.Select(x=>x.RedirectUri)) )
                .ForMember(dest => dest.PostLogoutRedirectUris, opt => opt.MapFrom(src=>src.RedirectUris.Select(x=>x.RedirectUri)) )
                .ForMember(dest => dest.AllowedCorsOrigins, opt => opt.MapFrom(src=>src.AllowedCorsOrigins.Select(x=>x.Origin)) )
                .ForMember(dest => dest.IdentityProviderRestrictions, opt => opt.MapFrom(src=>src.IdentityProviderRestrictions.Select(x=>x.Provider)) )
                .ReverseMap();
                ;
            CreateMap<ClientSecret, ClientSecretDto>().ReverseMap();
/*
            CreateMap<ClientSecret, ClientSecretDto>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<ClientClaim, ClientClaimDto>(MemberList.None)
                .ConstructUsing(src => new ClientClaimDto() { Type = src.Type, Value = src.Value })
                .ReverseMap();
*/
            CreateMap<ClientClaim, ClientClaimDto>(MemberList.None).ReverseMap();
            CreateMap<ClientProperty, ClientPropertyDto>()
                .ReverseMap();

            
        }
    }
}