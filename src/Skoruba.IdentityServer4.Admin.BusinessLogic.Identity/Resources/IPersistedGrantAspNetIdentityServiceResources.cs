using Skoruba.Admin.BusinessLogic.Identity.Helpers;

namespace Skoruba.Admin.BusinessLogic.Identity.Resources
{
    public interface IPersistedGrantAspNetIdentityServiceResources
    {
        ResourceMessage PersistedGrantDoesNotExist();

        ResourceMessage PersistedGrantWithSubjectIdDoesNotExist();
    }
}
