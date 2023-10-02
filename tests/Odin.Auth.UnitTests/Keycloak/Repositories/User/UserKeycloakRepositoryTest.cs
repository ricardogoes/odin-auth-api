using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Moq;
using Moq.Contrib.HttpClient;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.Models.AppSettings;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Keycloak.Exceptions;
using Odin.Auth.Infra.Keycloak.Mappers;
using Odin.Auth.Infra.Keycloak.Models;
using Odin.Auth.Infra.Keycloak.Repositories;
using Odin.Auth.Infra.Messaging.Policies;
using System.Net;
using System.Text.Json;

namespace Odin.Auth.UnitTests.Keycloak.Repositories.User
{
    [Collection(nameof(UserKeycloakRepositoryTestFixtureCollection))]
    public class UserKeycloakRepositoryTest
    {
        private readonly UserKeycloakRepositoryTestFixture _fixture;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly AppSettings _appSettings;

        private readonly List<UserRepresentation> _expectedUsers;

        private readonly Guid _tenant1 = Guid.NewGuid();
        private readonly Guid _tenant2 = Guid.NewGuid();

        public UserKeycloakRepositoryTest(UserKeycloakRepositoryTestFixture fixture)
        {
            _fixture = fixture;

            _httpContextAccessorMock = new();
            _httpContextAccessorMock.Setup(s => s.HttpContext!.User.Identity!.Name).Returns("unit.testing");

            _appSettings = _fixture.GetAppSettings();

            _expectedUsers = new List<UserRepresentation>
            {
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant1).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant2).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant2).ToUserRepresentation(),
                _fixture.GetValidUser(_tenant2).ToUserRepresentation()
            };
        }

        [Fact(DisplayName = "CreateUserasync() should create a new valid user")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task CreateUserAsync()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users";

            var user = _fixture.GetValidUser();

            handler.SetupRequest(HttpMethod.Post, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(user));

                return true;
            }).ReturnsResponse(HttpStatusCode.OK);

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            await userRepository.CreateUserAsync(user, CancellationToken.None);
        }

        [Fact(DisplayName = "CreateUserAsync() should throw an error with invalid data")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task CreateUserAsync_ThrowError()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users";

            var user = _fixture.GetValidUser();
                        
            handler.SetupRequest(HttpMethod.Post, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/x-www-form-urlencoded");
                request.Content = new StringContent(JsonSerializer.Serialize(user));

                return true;
            }).ReturnsJsonResponse(HttpStatusCode.BadRequest, new KeycloakResponseError("Error", "ErrorDescription"), new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true
            });

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var task = async () => await userRepository.CreateUserAsync(user, CancellationToken.None);

            await task.Should().ThrowAsync<KeycloakException>()
                .WithMessage($"Error. ERROR DETAIL: ErrorDescription");
        }

        [Fact(DisplayName = "UpdateUserasync() should update a new valid user")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task UpdateUserAsync()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var user = _fixture.GetValidUser(_fixture.TenantSinapseId);
            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}";

            handler.SetupRequest(HttpMethod.Put, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(user));

                return true;
            }).ReturnsResponse(HttpStatusCode.OK);

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var userUpdated = await userRepository.UpdateUserAsync(user, CancellationToken.None);

            userUpdated.Should().NotBeNull();
            userUpdated!.Username.Should().Be(user.Username);
        }

        [Fact(DisplayName = "UpdateUserAsync() should throw an error with invalid data")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task UpdateUserAsync_ThrowError()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var user = _fixture.GetValidUser();
            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}";
                        
            handler.SetupRequest(HttpMethod.Put, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(user));

                return true;
            }).ReturnsJsonResponse(HttpStatusCode.BadRequest, new KeycloakResponseError("Error", "ErrorDescription"), new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true
            });

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var task = async () => await userRepository.UpdateUserAsync(user, CancellationToken.None);

            await task.Should().ThrowAsync<KeycloakException>()
                .WithMessage($"Error. ERROR DETAIL: ErrorDescription");
        }

        [Fact(DisplayName = "FindByIdAsync() should return a user by valid id")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task FindByIdAsync()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var user = _fixture.GetValidUser(_fixture.TenantSinapseId);
            
            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}")
                .ReturnsJsonResponse(HttpStatusCode.OK, user.ToUserRepresentation());

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}/groups")
                .ReturnsJsonResponse(HttpStatusCode.OK, user.Groups);

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var userRepresentation = await userRepository.FindByIdAsync(user.Id, CancellationToken.None);

            userRepresentation.Should().NotBeNull();
            userRepresentation.Id.Should().Be(user.Id);
            userRepresentation.Username.Should().Be(user.Username);
            userRepresentation.FirstName.Should().Be(user.FirstName);
            userRepresentation.LastName.Should().Be(user.LastName);
            userRepresentation.Email.Should().Be(user.Email);

            userRepresentation.Groups.Should().NotBeNull();
            userRepresentation.Groups.Should().HaveCount(user.Groups.Count());

        }

        [Fact(DisplayName = "FindByIdAsync() should throw error with invalid data")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task FindByIdAsync_ThrowError()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var tenantId = Guid.NewGuid();
            var user = _fixture.GetValidUser(tenantId);

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}")
                .ReturnsJsonResponse(HttpStatusCode.BadRequest, new KeycloakResponseError("Error", "ErrorDescription"), new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                    PropertyNameCaseInsensitive = true
                });

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var task = async () => await userRepository.FindByIdAsync(user.Id, CancellationToken.None);

            await task.Should().ThrowAsync<KeycloakException>()
                .WithMessage($"Error. ERROR DETAIL: ErrorDescription");
        }

        [Fact(DisplayName = "FindByIdAsync() should throw error when user not found")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task FindByIdAsync_ThrowErrorNotFound()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var user = _fixture.GetValidUser(_fixture.TenantSinapseId);

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}")
                .ReturnsResponse(HttpStatusCode.NotFound);

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var task = async () => await userRepository.FindByIdAsync(user.Id, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"User with ID '{user.Id}' not found");
        }

        [Fact(DisplayName = "FindByIdAsync() should throw error when different tenants")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task FindByIdAsync_ThrowErrorNotFoundTenants()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var tenantId = Guid.NewGuid();
            var user = _fixture.GetValidUser();

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}")
                .ReturnsResponse(HttpStatusCode.NotFound);

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var task = async () => await userRepository.FindByIdAsync(user.Id, CancellationToken.None);

            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"User with ID '{user.Id}' not found");
        }

        [Fact(DisplayName = "FindUsersAsync() should return users")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task FindUsersAsync()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/count?q=tenant_id:{_fixture.TenantSinapseId}")
                .ReturnsResponse("10");

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users?first=0&max={10}&q=tenant_id:{_fixture.TenantSinapseId}")
                .ReturnsJsonResponse(HttpStatusCode.OK, _expectedUsers.Take(10));

            foreach (var user in _expectedUsers.Take(10))
            {
                handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}/groups")
                .ReturnsJsonResponse(HttpStatusCode.OK, new List<UserGroup> { new UserGroup(Guid.NewGuid(), user.Groups!.First(), $"/{user.Groups!.First()}")});
            }

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var users = await userRepository.FindUsersAsync(CancellationToken.None);

            users.Should().NotBeNull();
            users.Should().HaveCount(10);
        }

        [Fact(DisplayName = "FindUsersAsync() should throw an error with invalid data")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task FindUsersAsync_ThrowError()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/count?q=tenant_id:{_fixture.TenantSinapseId}")
                .ReturnsResponse("10");

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users?first=0&max={10}&q=tenant_id:{_fixture.TenantSinapseId}")
                .ReturnsJsonResponse(HttpStatusCode.BadRequest, new KeycloakResponseError("Error", "ErrorDescription"), new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                    PropertyNameCaseInsensitive = true
                });

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var task = async () => await userRepository.FindUsersAsync(CancellationToken.None);

            await task.Should().ThrowAsync<KeycloakException>()
                .WithMessage($"Error. ERROR DETAIL: ErrorDescription");
        }

        [Fact(DisplayName = "FindGroupsByUserIdAsync() should return the groups of a valid user")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task FindGroupsByUserIdAsync()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var user = _fixture.GetValidUser(_fixture.TenantSinapseId);

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}/groups")
                .ReturnsJsonResponse(HttpStatusCode.OK, user.Groups);

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var groups = await userRepository.FindGroupsByUserIdAsync(user.Id, CancellationToken.None);

            groups.Should().NotBeNull();
            groups.Should().HaveCount(user.Groups.Count());
        }

        [Fact(DisplayName = "FindGroupsByUserIdAsync() should throw an error with invalid data")]
        [Trait("Infra.Keycloak", "Repositories / UserKeycloakRepository")]
        public async Task FindGroupsByUserIdAsync_ThrowError()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!);
                    return client;
                });

            var user = _fixture.GetValidUser(_fixture.TenantSinapseId);

            handler.SetupRequest(HttpMethod.Get, $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}/users/{user.Id}/groups")
                .ReturnsJsonResponse(HttpStatusCode.BadRequest, new KeycloakResponseError("Error", "ErrorDescription"), new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                    PropertyNameCaseInsensitive = true
                });

            var userRepository = new UserKeycloakRepository(clientFactory, _httpContextAccessorMock.Object, _appSettings);

            var task = async () => await userRepository.FindGroupsByUserIdAsync(user.Id, CancellationToken.None);

            await task.Should().ThrowAsync<KeycloakException>()
                .WithMessage($"Error. ERROR DETAIL: ErrorDescription");
        }
    }
}
