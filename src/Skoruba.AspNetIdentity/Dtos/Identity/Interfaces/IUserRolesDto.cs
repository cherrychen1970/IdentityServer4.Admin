using Skoruba.Core.Dtos.Common;
using System.Collections.Generic;

namespace Skoruba.Core.Dtos.Common
{
	public class SelectItemDto
	{
		public SelectItemDto(string id, string text)
		{
			Id = id;
			Text = text;
		}

		public string Id { get; set; }

		public string Text { get; set; }
	}
}


namespace Skoruba.AspNetIdentity.Dtos.Interfaces
{
    public interface IUserRolesDto : IBaseUserRolesDto
    {
        string UserName { get; set; }
        List<SelectItemDto> RolesList { get; set; }
        List<IRoleDto> Roles { get; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
    }
}
