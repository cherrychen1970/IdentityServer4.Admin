using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Skoruba.Admin.EntityFramework.Identity
{
    public class IdentityDbContext<TUser, TKey> : IdentityDbContext<
        TUser, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>,
        IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
            where TUser : IdentityUser<TKey>
            where TKey : IEquatable<TKey>
    {
    }
}