using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Moq;
using Moq.Contrib.HttpClient;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Models.AppSettings;
using Odin.Auth.Infra.Data.EF.Repositories;
using Odin.Auth.Infra.Keycloak.Exceptions;
using Odin.Auth.Infra.Keycloak.Models;
using Odin.Auth.Infra.Keycloak.Repositories;
using Odin.Auth.Infra.Messaging.Policies;
using System.Net;
using System.Text.Json;

namespace Odin.Auth.UnitTests.Keycloak.Repositories.Auth
{
    [Collection(nameof(AuthKeycloakRepositoryTestFixtureCollection))]
    public class AuthKeycloakRepositoryTest
    {
        private readonly AuthKeycloakRepositoryTestFixture _fixture;
        private readonly AppSettings _appSettings;

        public AuthKeycloakRepositoryTest(AuthKeycloakRepositoryTestFixture fixture)
        {
            _fixture = fixture;
            _appSettings = _fixture.GetAppSettings();
        }

        [Fact(DisplayName = "AuthAsync() should authenticate with valid data")]
        [Trait("Infra.Keycloak", "Repositories / AuthKeycloakRepository")]
        public async Task AuthAsync()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            var expectedAuthResponse = new KeycloakAuthResponse("access-token", "id-token", "refresh-token", 360);

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.Keycloak!.AuthServerUrl!);
                    return client;
                });

            var keycloakUrlRealm = $"{_appSettings.Keycloak!.AuthServerUrl}/realms/{_appSettings.Keycloak!.Realm}/protocol/openid-connect/token";

            var collection = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "password"),
                new("client_id", _appSettings.Keycloak.Resource!),
                new("client_secret", _appSettings.Keycloak.Credentials!.Secret!),
                new("username", "admin"),
                new("password", "admin")
            };

            handler.SetupRequest(HttpMethod.Post, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/x-www-form-urlencoded");
                request.Content = new FormUrlEncodedContent(collection);

                return true;
            }).ReturnsJsonResponse(expectedAuthResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true
            });

            var authRepository = new AuthKeycloakRepository(clientFactory, _appSettings);

            var authResponse = await authRepository.AuthAsync("admin", "admin", CancellationToken.None);

            authResponse.Should().NotBeNull();
            authResponse.AccessToken.Should().Be(expectedAuthResponse.AccessToken);
            authResponse.IdToken.Should().Be(expectedAuthResponse.IdToken);
            authResponse.RefreshToken.Should().Be(expectedAuthResponse.RefreshToken);
            authResponse.ExpiresIn.Should().Be(expectedAuthResponse.ExpiresIn);
        }

        [Fact(DisplayName = "AuthAsync() should throw an error with valid data")]
        [Trait("Infra.Keycloak", "Repositories / AuthKeycloakRepository")]
        public async Task AuthAsync_ThrowError()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.Keycloak!.AuthServerUrl!);
                    return client;
                });

            var keycloakUrlRealm = $"{_appSettings.Keycloak!.AuthServerUrl}/realms/{_appSettings.Keycloak!.Realm}/protocol/openid-connect/token";

            var collection = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "password"),
                new("client_id", _appSettings.Keycloak.Resource!),
                new("client_secret", _appSettings.Keycloak.Credentials!.Secret!),
                new("username", "admin"),
                new("password", "error")
            };

            handler.SetupRequest(HttpMethod.Post, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/x-www-form-urlencoded");
                request.Content = new FormUrlEncodedContent(collection);

                return true;
            }).ReturnsJsonResponse(HttpStatusCode.BadRequest, new KeycloakResponseError("Error", "ErrorDescription"), new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true
            });

            var authRepository = new AuthKeycloakRepository(clientFactory, _appSettings);

            var task = async () => await authRepository.AuthAsync("admin", "error", CancellationToken.None);

            await task.Should().ThrowAsync<KeycloakException>()
                .WithMessage($"Error. ERROR DETAIL: ErrorDescription");
        }

        [Fact(DisplayName = "LogoutAsync() should logout successfully")]
        [Trait("Infra.Keycloak", "Repositories / AuthKeycloakRepository")]
        public async Task LogoutAsync()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.Keycloak!.AuthServerUrl!);
                    return client;
                });

            var userId = Guid.NewGuid();
            var keycloakUrlRealm = $"{_appSettings.Keycloak!.AuthServerUrl}/admin/realms/{_appSettings.Keycloak!.Realm}/users/{userId}/logout";

            handler.SetupRequest(HttpMethod.Post, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/x-www-form-urlencoded");
                return true;
            }).ReturnsResponse(HttpStatusCode.OK);

            var authRepository = new AuthKeycloakRepository(clientFactory, _appSettings);

            await authRepository.LogoutAsync(userId, CancellationToken.None);            
        }

        [Fact(DisplayName = "LogoutAsync() should throw an error with invalid data")]
        [Trait("Infra.Keycloak", "Repositories / AuthKeycloakRepository")]
        public async Task LogoutAsync_ThrowError()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.Keycloak!.AuthServerUrl!);
                    return client;
                });

            var userId = Guid.Empty;
            var keycloakUrlRealm = $"{_appSettings.Keycloak!.AuthServerUrl}/admin/realms/{_appSettings.Keycloak!.Realm}/users/{userId}/logout";

            handler.SetupRequest(HttpMethod.Post, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/x-www-form-urlencoded");
                return true;
            }).ReturnsJsonResponse(HttpStatusCode.BadRequest, new KeycloakResponseError("Error", "ErrorDescription"), new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true
            });

            var authRepository = new AuthKeycloakRepository(clientFactory, _appSettings);

            var task = async () => await authRepository.LogoutAsync(userId, CancellationToken.None);

            await task.Should().ThrowAsync<KeycloakException>()
                .WithMessage($"Error. ERROR DETAIL: ErrorDescription");
        }

        [Fact(DisplayName = "ChangePasswordAsync() should change password successfully")]
        [Trait("Infra.Keycloak", "Repositories / AuthKeycloakRepository")]
        public async Task ChangePasswordAsync()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.Keycloak!.AuthServerUrl!);
                    return client;
                });

            var user = _fixture.GetValidUser();
            var keycloakUrlRealm = $"{_appSettings.Keycloak!.AuthServerUrl}/admin/realms/{_appSettings.Keycloak!.Realm}/users/{user.Id}/reset-password";

            handler.SetupRequest(HttpMethod.Put, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(user.Credentials));

                return true;
            }).ReturnsResponse(HttpStatusCode.OK);

            var authRepository = new AuthKeycloakRepository(clientFactory, _appSettings);

            await authRepository.ChangePasswordAsync(user, CancellationToken.None);
        }

        [Fact(DisplayName = "ChangePasswordAsync() should throw an error with invalid data")]
        [Trait("Infra.Keycloak", "Repositories / AuthKeycloakRepository")]
        public async Task ChangePasswordAsync_ThrowError()
        {
            var handler = new Mock<HttpMessageHandler>();
            var clientFactory = handler.CreateClientFactory();

            Mock.Get(clientFactory).Setup(x => x.CreateClient("Keycloak"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    client.BaseAddress = new Uri(_appSettings.Keycloak!.AuthServerUrl!);
                    return client;
                });

            var user = _fixture.GetValidUser();
            var keycloakUrlRealm = $"{_appSettings.Keycloak!.AuthServerUrl}/admin/realms/{_appSettings.Keycloak!.Realm}/users/{user.Id}/reset-password";

            handler.SetupRequest(HttpMethod.Put, keycloakUrlRealm, request =>
            {
                request.Headers.Add(HeaderNames.Accept, "application/json");
                request.Content = new StringContent(JsonSerializer.Serialize(user.Credentials));
                return true;
            }).ReturnsJsonResponse(HttpStatusCode.BadRequest, new KeycloakResponseError("Error", "ErrorDescription"), new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true
            });

            var authRepository = new AuthKeycloakRepository(clientFactory, _appSettings);

            var task = async () => await authRepository.ChangePasswordAsync(user, CancellationToken.None);

            await task.Should().ThrowAsync<KeycloakException>()
                .WithMessage($"Error. ERROR DETAIL: ErrorDescription");
        }
    }
}
