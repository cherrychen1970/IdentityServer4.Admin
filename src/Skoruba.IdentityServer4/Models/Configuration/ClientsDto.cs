﻿using System.Collections.Generic;

namespace Skoruba.IdentityServer4.Models
{
	public class ClientsDto
	{
		public ClientsDto()
		{
			Clients = new List<ClientDto>();
		}

		public List<ClientDto> Clients { get; set; }

		public int TotalCount { get; set; }		

		public int PageSize { get; set; }
	}
}