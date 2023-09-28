using FluentAssertions;
using Moq;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Models;
using Odin.Auth.Infra.Keycloak.Interfaces;
using App = Odin.Auth.Application.Users.GetUsers;

namespace Odin.Auth.UnitTests.Application.Users.GetUsers
{
    [Collection(nameof(GetUsersTestFixtureCollection))]
    public class GetUsersTest
    {
        private readonly GetUsersTestFixture _fixture;

        private readonly Mock<IUserKeycloakRepository> _keycloakRepositoryMock;
        private readonly PaginatedListOutput<User> _expectedUsers;

        private readonly Guid _tenant1 = Guid.NewGuid();
        private readonly Guid _tenant2 = Guid.NewGuid();

        public GetUsersTest(GetUsersTestFixture fixture)
        {
            _fixture = fixture;
            _keycloakRepositoryMock = _fixture.GetUserKeycloakRepositoryMock();

            _expectedUsers = new PaginatedListOutput<User>
            (
                totalItems: 15,
                items: new List<User>
                {
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant1),
                    _fixture.GetValidUser(_tenant2),
                    _fixture.GetValidUser(_tenant2),
                    _fixture.GetValidUser(_tenant2)
                }
            );
        }

        [Fact(DisplayName = "Handle() should return paginated data")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10), CancellationToken.None);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(12);
            users.TotalPages.Should().Be(2);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(10);
        }

        [Fact(DisplayName = "Handle() should return paginated data ordered by username")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_OrderedByUsername()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var orderedList = _expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString()).OrderBy(x => x.Username);

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, sort: "username"), CancellationToken.None);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(12);
            users.TotalPages.Should().Be(2);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(10);

            for (int i = 0; i < 10; i++)
            {
                users.Items.ToList()[i].Username.Should().Be(orderedList.ToList()[i].Username);
            }
        }

        [Fact(DisplayName = "Handle() should return data filtered by username")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_FilteredByUsername()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, username: _expectedUsers.Items.First().Username), CancellationToken.None);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(1);
            users.TotalPages.Should().Be(1);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(1);
            users.Items.First().Username.Should().Be(_expectedUsers.Items.First().Username);            
        }

        [Fact(DisplayName = "Handle() should return data filtered by first name")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_FilteredByFirstName()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, firstName: _expectedUsers.Items.First(x => x.Attributes["tenant_id"] == _tenant1.ToString()).FirstName), CancellationToken.None);

            var filteredUsers = _expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString() && x.FirstName == _expectedUsers.Items.First().FirstName);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(filteredUsers.Count());
            users.TotalPages.Should().Be(1);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(filteredUsers.Count() > 10 ? 10 : filteredUsers.Count());
        }

        [Fact(DisplayName = "Handle() should return data filtered by last name")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_FilteredByLastName()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, lastName: _expectedUsers.Items.First(x => x.Attributes["tenant_id"] == _tenant1.ToString()).LastName), CancellationToken.None);

            var filteredUsers = _expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString() && x.LastName == _expectedUsers.Items.First().LastName);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(filteredUsers.Count());
            users.TotalPages.Should().Be(1);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(filteredUsers.Count() > 10 ? 10 : filteredUsers.Count());
        }

        [Fact(DisplayName = "Handle() should return data filtered by email")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_FilteredByEmail()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, email: _expectedUsers.Items.First(x => x.Attributes["tenant_id"] == _tenant1.ToString()).Email), CancellationToken.None);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(1);
            users.TotalPages.Should().Be(1);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(1);
            users.Items.First().Username.Should().Be(_expectedUsers.Items.First().Username);
        }

        [Fact(DisplayName = "Handle() should return data filtered by status")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_FilteredByIsActive()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, isActive: true), CancellationToken.None);

            var activeUsers = _expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString() && x.IsActive);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(activeUsers.Count());
            users.TotalPages.Should().Be(1);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(activeUsers.Count());
        }

        [Fact(DisplayName = "Handle() should return data filtered by created by")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_FilteredByCreatedBy()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, createdBy: _expectedUsers.Items.First(x => x.Attributes["tenant_id"] == _tenant1.ToString()).Attributes["created_by"]), CancellationToken.None);

            var filteredUsers = _expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString() && x.CreatedBy == _expectedUsers.Items.First().CreatedBy);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(filteredUsers.Count());
            users.TotalPages.Should().Be(1);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(filteredUsers.Count());

            foreach (var user in users.Items)
                user.CreatedBy.Should().Be(_expectedUsers.Items.First().CreatedBy);
        }

        [Fact(DisplayName = "Handle() should return data filtered by last updated by")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_FilteredByLastUpdatedBy()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, lastUpdatedBy: _expectedUsers.Items.First(x => x.Attributes["tenant_id"] == _tenant1.ToString()).Attributes["last_updated_by"]), CancellationToken.None);

            var filteredUsers = _expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString() && x.LastUpdatedBy == _expectedUsers.Items.First().LastUpdatedBy);

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(filteredUsers.Count());
            users.TotalPages.Should().Be(1);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(filteredUsers.Count());

            foreach (var user in users.Items)
                user.LastUpdatedBy.Should().Be(_expectedUsers.Items.First().LastUpdatedBy);
        }

        [Fact(DisplayName = "Handle() should return data filtered by created dates")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_FilteredByCratedAt()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var startDate = new DateTime(2023, 9, 1);
            var endDate = DateTime.Now;

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, createdAtStart: startDate, createdAtEnd: endDate), CancellationToken.None);

            var filteredUsers = _expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString() && (x.CreatedAt >= startDate && x.CreatedAt <= endDate));

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(filteredUsers.Count());
            users.TotalPages.Should().Be(1);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(filteredUsers.Count());

            foreach (var user in users.Items)
            { 
                user.CreatedAt.Should().BeAfter(startDate);
                user.CreatedAt.Should().BeBefore(endDate);
            }
        }

        [Fact(DisplayName = "Handle() should return data filtered by last updated dates")]
        [Trait("Application", "GetUsers / GetUsers")]
        public async Task GetUsers_FilteredByLastUpdatedAt()
        {
            _keycloakRepositoryMock.Setup(s => s.FindUsersAsync(_tenant1, CancellationToken.None))
                .Returns(() => Task.FromResult(_expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString())));

            var startDate = new DateTime(2023, 9, 1);
            var endDate = DateTime.Now;

            var useCase = new App.GetUsers(_keycloakRepositoryMock.Object);
            var users = await useCase.Handle(new App.GetUsersInput(_tenant1, 1, 10, lastUpdatedAtStart: startDate, lastUpdatedAtEnd: endDate), CancellationToken.None);

            var filteredUsers = _expectedUsers.Items.Where(x => x.Attributes["tenant_id"] == _tenant1.ToString() && (x.LastUpdatedAt >= startDate && x.LastUpdatedAt <= endDate));

            users.Should().NotBeNull();
            users.TotalItems.Should().Be(filteredUsers.Count());
            users.TotalPages.Should().Be(1);
            users.PageNumber.Should().Be(1);
            users.PageSize.Should().Be(10);
            users.Items.Should().HaveCount(filteredUsers.Count());

            foreach (var user in users.Items)
            {
                user.LastUpdatedAt.Should().BeAfter(startDate);
                user.LastUpdatedAt.Should().BeBefore(endDate);
            }
        }
    }
}
