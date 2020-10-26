using Skoruba.AuditLogging.Events;
using Skoruba.Admin.BusinessLogic.Identity.Dtos.Identity;

namespace Skoruba.Admin.BusinessLogic.Identity.Events.Identity
{
    public class UserRequestedEvent<TUserDto> : AuditEvent
    {
        public TUserDto UserDto { get; set; }

        public UserRequestedEvent(TUserDto userDto)
        {
            UserDto = userDto;
        }
    }
}