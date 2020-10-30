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
        public static void AddAdminDbContexts(this IServiceCollection services, string connectionString)            
        {
            // TODO : support other types too..
            services.RegisterNpgSqlDbContexts(connectionString);
        }
    }
}
