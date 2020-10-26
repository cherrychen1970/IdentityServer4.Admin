using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Id4Models = IdentityServer4.Models;

namespace id4.Api.TestControllers
{
    public class IdentityResourceMapper : IMapper<IdentityResource,Id4Models.IdentityResource> {
        public IdentityResource ToEntity(Id4Models.IdentityResource client) {return client.ToEntity();}
        public Id4Models.IdentityResource ToModel(IdentityResource entity) {return entity.ToModel();}
    }    
    [Route("_api/idResources")]
    public partial class IdentityResourcesController : TestCrudController<IdentityResource, Id4Models.IdentityResource,string>
    {
        public IdentityResourcesController(ConfigurationDbContext context) 
            :base(context, new IdentityResourceMapper())
        {            
        }

        override protected IQueryable<IdentityResource> OnSelect(DbSet<IdentityResource> set)        
            => set.Include(x=>x.UserClaims).Include(x => x.Properties);

        override protected IdentityResource Find(string name)
            => _context.IdentityResources.SingleOrDefault(x=>x.Name==name);
                        
    }
}