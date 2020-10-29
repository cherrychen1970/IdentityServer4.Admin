using System.Collections.Generic;

namespace Skoruba.AspNetIdentity.Models.Interfaces
{
    public interface IUserProvidersDto : IUserProviderDto
    {
        List<IUserProviderDto> Providers { get; }
    }
}
