using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.AspNetIdentity.EntityFramework;
using Skoruba.IdentityServer4.EntityFramework.DbContexts;

using Skoruba.EntityFramework.DbContexts;
using Skoruba.EntityFramework.PostgreSQL.Extensions;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseExtensions
    {
        public static void AddAdminDbContexts<TKey>(this IServiceCollection services, string connectionString)
            where TKey : IEquatable<TKey>
        {
            // TODO : support other types too..
            services.RegisterNpgSqlDbContexts<TKey>(connectionString);
        }
    }
}
