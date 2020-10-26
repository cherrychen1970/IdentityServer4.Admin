using System;
using FluentAssertions;
using Skoruba.Admin.BusinessLogic.Mappers;
using Skoruba.Admin.UnitTests.Mocks;
using Xunit;

namespace Skoruba.Admin.UnitTests.Mappers
{
    public class PersistedGrantMappers
    {
        [Fact]
        public void CanMapPersistedGrantToModel()
        {
            var persistedGrantKey = Guid.NewGuid().ToString();

            //Generate entity
            var persistedGrant = PersistedGrantMock.GenerateRandomPersistedGrant(persistedGrantKey);

            //Try map to DTO
            var persistedGrantDto = persistedGrant.ToModel();

            //Asert
            persistedGrantDto.Should().NotBeNull();

            persistedGrant.ShouldBeEquivalentTo(persistedGrantDto);
        }
    }
}