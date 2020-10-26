using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.EntityFramework;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Id4Models = IdentityServer4.Models;


namespace id4.Api.TestControllers
{
    public class ClientMapper : IMapper<Client,Id4Models.Client> {
        public Client ToEntity(Id4Models.Client client) {return client.ToEntity();}
        public Id4Models.Client ToModel(Client entity) {return entity.ToModel();}
    }

    public partial class ClientsController : TestCrudController<Client,Id4Models.Client,string>
    {        
        public ClientsController(ConfigurationDbContext context) : base(context,new ClientMapper())
        {            
        }

        override protected IQueryable<Client> OnSelect(DbSet<Client> set)        
            => set.Include(x=>x.AllowedScopes).Include(x => x.AllowedScopes)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris);

        override protected Client Find(string id)
        {
            return _context.Clients.SingleOrDefault(x=>x.ClientId==id);
        }
    }
}