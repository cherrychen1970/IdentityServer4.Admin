using Skoruba.AspNetIdentity.Dtos.Base;
using Skoruba.AspNetIdentity.Dtos.Interfaces;

namespace Skoruba.AspNetIdentity.Dtos
{
    public class UserProviderDto<TKey> : BaseUserProviderDto<TKey>, IUserProviderDto
    {
        public string UserName { get; set; }

        public string ProviderKey { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderDisplayName { get; set; }
    }
}
