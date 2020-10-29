using Skoruba.AspNetIdentity.Models.Interfaces;

namespace Skoruba.AspNetIdentity.Models.Base
{
    public class BaseUserChangePasswordDto<TUserId> : IBaseUserChangePasswordDto
    {
        public TUserId UserId { get; set; }

        object IBaseUserChangePasswordDto.UserId => UserId;
    }
}