using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Skoruba.Admin.BusinessLogic.Identity.Events.Identity
{
    public class ClaimUsersRequestedEvent<TUsersDto> : AuditEvent
    {
        public TUsersDto Users { get; set; }

        public ClaimUsersRequestedEvent(TUsersDto users)
        {
            Users = users;
        }
    }
}