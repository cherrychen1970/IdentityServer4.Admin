using System.ComponentModel.DataAnnotations;
using Skoruba.AspNetIdentity.Models.Base;
using Skoruba.AspNetIdentity.Models.Interfaces;

namespace Skoruba.AspNetIdentity.Models
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
