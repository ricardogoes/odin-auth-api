using Microsoft.Net.Http.Headers;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Models.AppSettings;
using Odin.Auth.Infra.Keycloak.Exceptions;
using Odin.Auth.Infra.Keycloak.Interfaces;
using Odin.Auth.Infra.Keycloak.Mappers;
using Odin.Auth.Infra.Keycloak.Models;
using Odin.Auth.Infra.Messaging.Policies;
using System.Net.Http.Json;
using System.Text.Json;

namespace Odin.Auth.Infra.Keycloak.Repositories
{
    public class AuthKeycloakRepository : IAuthKeycloakRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _snakeCaseSerializeOptions;
        private readonly JsonSerializerOptions _camelCaseSerializeOptions;
        private readonly AppSettings _appSettings;

        public AuthKeycloakRepository(IHttpClientFactory httpClientFactory, AppSettings appSettings)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings;

            _snakeCaseSerializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true
            };

            _camelCaseSerializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<KeycloakAuthResponse> AuthAsync(string username, string password, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");

            var keycloakUrlRealm = $"{_appSettings.Keycloak!.AuthServerUrl}/realms/{_appSettings.Keycloak!.Realm}";

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{keycloakUrlRealm}/protocol/openid-connect/token");

            var collection = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "password"),
                new("client_id", _appSettings.Keycloak.Resource!),
                new("client_secret", _appSettings.Keycloak.Credentials!.Secret!),
                new("username", username),
                new("password", password)
            };

            var content = new FormUrlEncodedContent(collection);
            request.Content = content;

            var response = await client.SendAsync(request, cancellationToken);

            var outputString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = JsonSerializer.Deserialize<KeycloakResponseError>(outputString, _snakeCaseSerializeOptions)!;

                var message = errorContent.Error;
                if (!string.IsNullOrWhiteSpace(errorContent.ErrorDescription))
                    message += $". ERROR DETAIL: {errorContent.ErrorDescription}";

                throw new KeycloakException(message);
            }

            return JsonSerializer.Deserialize<KeycloakAuthResponse>(outputString, _snakeCaseSerializeOptions)!;
        }

        public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/x-www-form-urlencoded");

            var keycloakUrlRealm = $"{_appSettings.Keycloak!.AuthServerUrl}/admin/realms/{_appSettings.Keycloak!.Realm}";

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{keycloakUrlRealm}/users/{userId}/logout");


            var response = await client.SendAsync(request, cancellationToken);

            var outputString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = JsonSerializer.Deserialize<KeycloakResponseError>(outputString, _snakeCaseSerializeOptions)!;

                var message = errorContent.Error;
                if (!string.IsNullOrWhiteSpace(errorContent.ErrorDescription))
                    message += $". ERROR DETAIL: {errorContent.ErrorDescription}";

                throw new KeycloakException(message);
            }
        }

        public async Task ChangePasswordAsync(User user, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            var keycloakUrlRealm = $"{_appSettings.Keycloak!.AuthServerUrl}/admin/realms/{_appSettings.Keycloak!.Realm}";

            var response = await client.PutAsJsonAsync($"{keycloakUrlRealm}/users/{user.Id}/reset-password", user.Credentials.First().ToCredentialRepresentation(), cancellationToken);

            var outputString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = JsonSerializer.Deserialize<KeycloakResponseError>(outputString, _snakeCaseSerializeOptions)!;

                var message = errorContent.Error;
                if (!string.IsNullOrWhiteSpace(errorContent.ErrorDescription))
                    message += $". ERROR DETAIL: {errorContent.ErrorDescription}";

                throw new KeycloakException(message);
            }
        }
    }
}
