using Skoruba.AspNetIdentity.Dtos.Interfaces;

namespace Skoruba.AspNetIdentity.Dtos.Base
{
    public class BaseUserProviderDto<TUserId> : IBaseUserProviderDto
    {
        public TUserId UserId { get; set; }

        object IBaseUserProviderDto.UserId => UserId;
    }
}