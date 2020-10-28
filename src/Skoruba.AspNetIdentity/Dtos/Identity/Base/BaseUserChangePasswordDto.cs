using Skoruba.AspNetIdentity.Dtos.Interfaces;

namespace Skoruba.AspNetIdentity.Dtos.Base
{
    public class BaseUserChangePasswordDto<TUserId> : IBaseUserChangePasswordDto
    {
        public TUserId UserId { get; set; }

        object IBaseUserChangePasswordDto.UserId => UserId;
    }
}