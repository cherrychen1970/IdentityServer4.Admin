using System.ComponentModel.DataAnnotations;
using Skoruba.Identity.Dtos.Identity.Base;
using Skoruba.Identity.Dtos.Identity.Interfaces;

namespace Skoruba.Identity.Dtos.Identity
{
    public class UserChangePasswordDto<TKey> : BaseUserChangePasswordDto<TKey>, IUserChangePasswordDto
    {
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
