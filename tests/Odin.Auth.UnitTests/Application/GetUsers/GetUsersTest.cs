using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Odin.Auth.Application.GetUsers;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;
using App = Odin.Auth.Application.GetUsers;

namespace Odin.Auth.UnitTests.Application.GetUsers
{
    [Collection(nameof(GetUsersTestFixtureCollection))]
    public class GetUsersTest
    {
        private readonly GetUsersTestFixture _fixture;

        private readonly Mock<IKeycloakRepository> _keycloakRepositoryMock;

        public GetUsersTest(GetUsersTestFixture fixture)
        {
            _fixture = fixture;
            _keycloakRepositoryMock = _fixture.GetKeycloakRepositoryMock();
        }

        [Fact(DisplayName = "Handle() should return data filtered")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers()
        {
            var expectedUsers = new PaginatedListOutput<User>
            (
                totalItems: 15,
                items: new List<User>
                {
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser(),
                    _fixture.GetValidUser()
                }
            );

            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(CancellationToken.None))
                .Returns(() => Task.FromResult(expectedUsers.Items));

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(1, 10, "username", "", "", "", "", true), CancellationToken.None);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(15);
            users.TotalPages.Should().Be(2);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(10);
        }
    }
}
