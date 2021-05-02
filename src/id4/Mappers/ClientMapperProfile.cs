// Based on the IdentityServer4.EntityFramework - authors - Brock Allen & Dominick Baier.
// https://github.com/IdentityServer/IdentityServer4.EntityFramework

// Modified by Jan Škoruba
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using IdentityServer4.Models;
//using IdentityServer4.EntityFramework.Entities;
//using Dto = IdentityServer4.Models;

namespace id4.Models.Mappers
{
    public class ClientMapperProfile : Profile
    {
        public ClientMapperProfile()
        {
            CreateMap<Entities.Client, Dto.ClientDto>(MemberList.None)
                .ForMember(dest => dest.AccessTokenType, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokenExpiration, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokenUsage, opt => opt.Ignore())
                .ForMember(dest => dest.AllowedScopes, opt => opt.MapFrom(src => src.AllowedScopes.Select(x => x.Scope)))
                .ForMember(dest => dest.AllowedGrantTypes, opt => opt.MapFrom(src => src.AllowedGrantTypes.Select(x => x.GrantType)))
                .ForMember(dest => dest.RedirectUris, opt => opt.MapFrom(src => src.RedirectUris.Select(x => x.RedirectUri)))
                .ReverseMap()
                .ForMember(dest=>dest.ClientSecrets, opt => opt.MapFrom(src=>src.Secrets));

            CreateMap<Entities.ClientSecret, Dto.SecretDto>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();
            CreateMap<string, Entities.ClientSecret>()
                .ConstructUsing(src => new Entities.ClientSecret() { Value= src.Sha256() })
                .ReverseMap();

            CreateMap<Entities.ClientRedirectUri, string>().ConvertUsing(r => r.RedirectUri);
            CreateMap<string, Entities.ClientRedirectUri>()
                .ConstructUsing(src => new Entities.ClientRedirectUri() { RedirectUri = src })
                .ReverseMap();

            CreateMap<Entities.ClientScope, string>().ConvertUsing(r => r.Scope);
            CreateMap<string, Entities.ClientScope>()
                .ConstructUsing(src => new Entities.ClientScope() { Scope = src })
                .ReverseMap();

            //.ReverseMap();
            CreateMap<Entities.ClientGrantType, string>().ConvertUsing(r => r.GrantType);
            CreateMap<string, Entities.ClientGrantType>()
                .ConstructUsing(src => new Entities.ClientGrantType() { GrantType = src })
                .ReverseMap();
    
        }
    }
}