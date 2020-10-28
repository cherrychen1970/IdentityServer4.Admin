using Skoruba.AspNetIdentity.Dtos.Interfaces;
using System.Collections.Generic;

namespace Skoruba.AspNetIdentity.Dtos.Base
{
    public class BaseUserDto<TUserId> : IBaseUserDto
    {
        public TUserId Id { get; set; }

        public bool IsDefaultId() => EqualityComparer<TUserId>.Default.Equals(Id, default(TUserId));

        object IBaseUserDto.Id => Id;
    }
}