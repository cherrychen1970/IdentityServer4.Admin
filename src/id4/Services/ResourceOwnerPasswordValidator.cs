using System;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using id4.Models;

namespace id4.Services
{
    public class ResourceOwnerPasswordValidator<TUser> : IResourceOwnerPasswordValidator
        where TUser : AppUser, new()
    {
        private readonly UserManager<AppUser> _users;
        private readonly ISystemClock _clock;

        public ResourceOwnerPasswordValidator(UserManager<AppUser> users, ISystemClock clock)
        {
            _users = users;
            _clock = clock;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Returns nothing, but update the context.</returns>
        /// <exception cref="ArgumentException">Subject ID not set - SubjectId</exception>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _users.FindByNameAsync(context.UserName);            
            var r = await _users.CheckPasswordAsync(user,context.Password);
            

            if (r)
            {
                var claims = await _users.GetClaimsAsync(user);
                context.Result = new GrantValidationResult(
                    user.Id,
                    OidcConstants.AuthenticationMethods.Password,
                    _clock.UtcNow.UtcDateTime,
                    claims);
            }
            //return Task.CompletedTask;
        }
    }
}
