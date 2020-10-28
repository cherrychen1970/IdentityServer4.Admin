using System.ComponentModel.DataAnnotations;
using Skoruba.AspNetIdentity.Dtos.Base;
using Skoruba.AspNetIdentity.Dtos.Interfaces;

namespace Skoruba.AspNetIdentity.Dtos
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
