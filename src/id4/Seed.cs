// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using id4.Data;
using id4.Models;

namespace id4
{
    public class SeedService
    {
        private IConfiguration _config { get; }
        private ILogger<SeedService> _logger { get; }
        private ConfigurationDbContext _configDbContext { get; }
        private UserManager<AppUser> _userManager {get;}

        public SeedService(UserManager<AppUser> userManager, ConfigurationDbContext configDbContext,
           ApplicationDbContext context,
        ILogger<SeedService> logger,
        IConfiguration configuration)
        {
            _config = configuration;
            _logger = logger;
            _userManager = userManager;
            _configDbContext = configDbContext;
        }

        public void Seed()
        {
            var email = _config["AdminEmail"];            
            var result = _userManager.FindByEmailAsync(email).Result;
            if (result == null)
                CreateAdminUser();

            if (!_configDbContext.IdentityResources.Any())
                CreateIdentityResources();

            var clientId = _config["AdminUI:ClientId"];
            if (!_configDbContext.Clients.Where(x => x.ClientId == clientId).Any())
                CreateAdminApp();
        }
        public void CreateIdentityResources()
        {
            var role = new IdentityResource(
                name: "role",
                displayName: "role",
                claimTypes: new[] {
                    JwtClaimTypes.Role
                    });

            var list = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                role,
            };
            var entities = list.Select(x=> x.ToEntity());
            _configDbContext.IdentityResources.AddRange(entities);
            _configDbContext.SaveChanges();
        }

        public void CreateAdminUser()
        {
            var email = _config["AdminEmail"];
            var pass = _config["AdminPassword"];

            var alice = new AppUser
            {
                UserName = "admin",
                Email = email,
                EmailConfirmed = true
            };

            var result = _userManager.CreateAsync(alice, pass).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = _userManager.AddClaimsAsync(alice, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "site admin"),
                            new Claim(JwtClaimTypes.Role, "admin")
                        }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("admin created");
        }

        void CreateAdminApp()
        {
            var client = new Client
            {
                ClientId = _config["AdminUI:ClientId"],
                ClientName = _config["AdminUI:Name"],
                AllowedGrantTypes = GrantTypes.Code,
                RequireConsent = false,
                //AccessTokenLifetime = _config.GetSection("IdentityServer:TokenLifetime"),

                RedirectUris = _config.GetSection("RedirectUris").Get<ICollection<string>>(),
                //PostLogoutRedirectUris = { SkorubaUri + "/" },

                AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },
                ClientSecrets =
                    {
                        new Secret( _config["AdminUI:ClientSecret"].Sha256())
                    }
            };
            _configDbContext.Clients.Add(client.ToEntity());

            client = new Client
            {
                ClientId = _config["AdminUI:ClientId"],
                ClientName = _config["AdminUI:Name"],
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireConsent = false,
                //AccessTokenLifetime = _config.GetSection("IdentityServer:TokenLifetime"),

                RedirectUris = _config.GetSection("RedirectUris").Get<ICollection<string>>(),
                //PostLogoutRedirectUris = { SkorubaUri + "/" },

                AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    }
            };
            _configDbContext.Clients.Add(client.ToEntity());

            _configDbContext.SaveChanges();
        }
    }


}