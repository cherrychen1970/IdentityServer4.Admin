using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Id4Models = IdentityServer4.Models;

namespace id4.Api.TestControllers
{
    public class ApiResourceMapper : IMapper<ApiResource,Id4Models.ApiResource> {
        public ApiResource ToEntity(Id4Models.ApiResource model) {return model.ToEntity();}
        public Id4Models.ApiResource ToModel(ApiResource entity) {return entity.ToModel();}
    }    

    [Route("_api/apiResources")]
    public partial class ApiResourcesController 
        : TestCrudController<ApiResource, Id4Models.ApiResource,string>
    {
        public ApiResourcesController(ConfigurationDbContext context) 
            :base(context, new ApiResourceMapper())
        {            
        }

        override protected ApiResource Find(string name)
            => _context.ApiResources.SingleOrDefault(x=>x.Name==name);
        override protected IQueryable<ApiResource> OnSelect(DbSet<ApiResource> set)        
            => set.Include(x=>x.UserClaims).Include(x => x.Properties);

    }
}