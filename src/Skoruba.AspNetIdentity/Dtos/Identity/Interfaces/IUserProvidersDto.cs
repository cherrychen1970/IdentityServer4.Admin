using System.Collections.Generic;

namespace Skoruba.AspNetIdentity.Dtos.Interfaces
{
    public interface IUserProvidersDto : IUserProviderDto
    {
        List<IUserProviderDto> Providers { get; }
    }
}
