using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.Admin.Configuration.Interfaces;
using Skoruba.Admin.Configuration.Test;
using Skoruba.Admin.IntegrationTests.Common;
using Xunit;

namespace Skoruba.Admin.IntegrationTests.Tests.Base
{
    public class BaseClassFixture : IClassFixture<WebApplicationFactory<StartupTest>>
    {
        protected readonly WebApplicationFactory<StartupTest> Factory;
        protected readonly HttpClient Client;

        public BaseClassFixture(WebApplicationFactory<StartupTest> factory)
        {
            Factory = factory;
            Client = factory.SetupClient();
            Factory.CreateClient();
        }

        protected virtual void SetupAdminClaimsViaHeaders()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IRootConfiguration>();
                Client.SetAdminClaimsViaHeaders(configuration.AdminConfiguration);
            }
        }
    }
}