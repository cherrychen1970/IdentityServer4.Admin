using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Skoruba.Admin.BusinessLogic.Identity.Events.Identity
{
    public class RoleUsersRequestedEvent<TUsersDto> : AuditEvent
    {
        public TUsersDto Users { get; set; }

        public RoleUsersRequestedEvent(TUsersDto users)
        {
            Users = users;
        }
    }
}