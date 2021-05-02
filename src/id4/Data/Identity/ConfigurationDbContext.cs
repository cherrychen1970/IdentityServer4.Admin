using System;
using AutoMapper;
using IdEntities = IdentityServer4.EntityFramework.Entities;

using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Bluebird.Entity;

using id4.Models.Entities;
namespace id4.Data
{



    /*
    public class Client : IdEntities.Client, IEntity<int> { }
    public class ApiResource : IdEntities.ApiResource, IEntity<int> { }
    public class ApiScope : IdEntities.ApiScope, IEntity<int> { }
    public class IdentityResource : IdEntities.IdentityResource, IEntity<int> { }

*/
    //
    // Summary:
    //     DbContext for the IdentityServer configuration data.
    public class ConfigurationDbContext : DbContext
    {
        //
        // Summary:
        //     Initializes a new instance of the IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext
        //     class.
        //
        // Parameters:
        //   options:
        //     The options.
        //
        //   storeOptions:
        //     The store options.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     storeOptions
        public ConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=id4.db");
            }
        }
        //
        // Summary:
        //     Gets or sets the clients.
        //
        // Value:
        //     The clients.
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientGrantType> ClientGrantTypes {get;set;}
        public DbSet<ClientSecret> ClientSecrets {get;set;}
        public DbSet<ClientRedirectUri> ClientRedirectUris {get;set;}
        public DbSet<ClientScope> clientScopes {get;set;}
        //
        // Summary:
        //     Gets or sets the clients' CORS origins.
        //
        // Value:
        //     The clients CORS origins.

        //
        // Summary:
        //     Gets or sets the identity resources.
        //
        // Value:
        //     The identity resources.
        //public DbSet<IdentityResource> IdentityResources { get; set; }
        //
        // Summary:
        //     Gets or sets the API resources.
        //
        // Value:
        //     The API resources.
        //public DbSet<ApiResource> ApiResources { get; set; }
        //
        // Summary:
        //     Gets or sets the API scopes.
        //
        // Value:
        //     The API resources.
        //public DbSet<ApiScope> ApiScopes { get; set; }

        //
        // Summary:
        //     Override this method to further configure the model that was discovered by convention
        //     from the entity types exposed in Microsoft.EntityFrameworkCore.DbSet`1 properties
        //     on your derived context. The resulting model may be cached and re-used for subsequent
        //     instances of your derived context.
        //
        // Parameters:
        //   modelBuilder:
        //     The builder being used to construct the model for this context. Databases (and
        //     other extensions) typically define extension methods on this object that allow
        //     you to configure aspects of the model that are specific to a given database.
        //
        // Remarks:
        //     If a model is explicitly set on the options for this context (via Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel))
        //     then this method will not be run.
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}