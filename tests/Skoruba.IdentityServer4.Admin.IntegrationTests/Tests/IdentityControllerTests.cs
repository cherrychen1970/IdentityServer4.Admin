using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Skoruba.Admin.Configuration.Constants;
using Skoruba.Admin.Configuration.Test;
using Skoruba.Admin.IntegrationTests.Common;
using Skoruba.Admin.IntegrationTests.Tests.Base;
using Xunit;

namespace Skoruba.Admin.IntegrationTests.Tests
{
    public class IdentityControllerTests : BaseClassFixture
    {
        public IdentityControllerTests(WebApplicationFactory<StartupTest> factory) : base(factory)
        {
        }

        [Fact]
        public async Task ReturnSuccessWithAdminRole()
        {
            SetupAdminClaimsViaHeaders();

            foreach (var route in RoutesConstants.GetIdentityRoutes())
            {
                // Act
                var response = await Client.GetAsync($"/Identity/{route}");

                // Assert
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task ReturnRedirectWithoutAdminRole()
        {
            //Remove
            Client.DefaultRequestHeaders.Clear();

            foreach (var route in RoutesConstants.GetIdentityRoutes())
            {
                // Act
                var response = await Client.GetAsync($"/Identity/{route}");

                // Assert           
                response.StatusCode.Should().Be(HttpStatusCode.Redirect);

                //The redirect to login
                response.Headers.Location.ToString().Should().Contain(AuthenticationConsts.AccountLoginPage);
            }
        }
    }
}
