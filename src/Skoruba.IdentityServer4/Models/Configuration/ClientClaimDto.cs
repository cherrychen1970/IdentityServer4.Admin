using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Skoruba.IdentityServer4.Models
{
    public class ClientClaimsDto
    {
        public int Id { get; set; }
        public ICollection<ClientClaimDto> Claims { get; set; }

    }
    public class ClientClaimDto
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }
    }
}