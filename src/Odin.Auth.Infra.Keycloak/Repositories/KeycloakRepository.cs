using Keycloak.AuthServices.Authentication;
using Microsoft.Net.Http.Headers;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Keycloak.Exceptions;
using Odin.Auth.Infra.Keycloak.Mappers;
using Odin.Auth.Infra.Keycloak.Models;
using Odin.Auth.Infra.Messaging.Policies;
using System.Net.Http.Json;
using System.Text.Json;

namespace Odin.Auth.Infra.Keycloak.Repositories
{
    public class KeycloakRepository : IKeycloakRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _snakeCaseSerializeOptions;
        private readonly JsonSerializerOptions _camelCaseSerializeOptions;
        private readonly KeycloakAuthenticationOptions _keycloakOptions;

        public KeycloakRepository(IHttpClientFactory httpClientFactory, KeycloakAuthenticationOptions keycloakOptions)
        {
            _httpClientFactory = httpClientFactory;
            _keycloakOptions = keycloakOptions;

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

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{_keycloakOptions.KeycloakUrlRealm}/protocol/openid-connect/token");

            var collection = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "password"),
                new("client_id", _keycloakOptions.Resource),
                new("client_secret", _keycloakOptions.Credentials.Secret),
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

            var adminUrl = _keycloakOptions.KeycloakUrlRealm.Replace("/realms", "/admin/realms");

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{adminUrl}/users/{userId}/logout");


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

            var response = await client.PutAsJsonAsync($"/admin/realms/{_keycloakOptions.Realm}/users/{user.Id}/reset-password", user.Credentials.First().ToCredentialRepresentation(), cancellationToken);

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

        public async Task CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            var response = await client.PostAsJsonAsync($"/admin/realms/{_keycloakOptions.Realm}/users", user.ToUserRepresentation(), cancellationToken);

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

        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            var response = await client.PutAsJsonAsync($"/admin/realms/{_keycloakOptions.Realm}/users/{user.Id}", user.ToUserRepresentation(), cancellationToken);

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

        public async Task<User> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");            

            var adminUrl = _keycloakOptions.KeycloakUrlRealm.Replace("/realms", "/admin/realms");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{adminUrl}/users/{id}");

            var response = await client.SendAsync(request, cancellationToken);

            var outputString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new NotFoundException($"User with ID '{id}' not found");
                else
                {
                    var errorContent = JsonSerializer.Deserialize<KeycloakResponseError>(outputString, _snakeCaseSerializeOptions)!;

                    var message = errorContent.Error;
                    if (!string.IsNullOrWhiteSpace(errorContent.ErrorDescription))
                        message += $". ERROR DETAIL: {errorContent.ErrorDescription}";

                    throw new KeycloakException(message);
                }
            }

            var userRepresentation = JsonSerializer.Deserialize<UserRepresentation>(outputString, _camelCaseSerializeOptions)!;
            var userGroups = await FindGroupsByUserIdAsync(id, cancellationToken);

            return userRepresentation.ToUser(userGroups.ToList());
        }

        public async Task<IEnumerable<UserGroup>> FindGroupsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");

            var adminUrl = _keycloakOptions.KeycloakUrlRealm.Replace("/realms", "/admin/realms");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{adminUrl}/users/{userId}/groups");

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

            return JsonSerializer.Deserialize<IEnumerable<UserGroup>>(outputString, _camelCaseSerializeOptions)!;
        }

        public async Task<IEnumerable<User>> FindUsersAsync(CancellationToken cancellationToken)
        {
            var countUsers = await GetUsersCountAsync(cancellationToken);

            var client = _httpClientFactory.CreateClient("Keycloak");

            var adminUrl = _keycloakOptions.KeycloakUrlRealm.Replace("/realms", "/admin/realms");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{adminUrl}/users?first=0&max={countUsers}");

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

            return JsonSerializer.Deserialize<IEnumerable<User>>(outputString, _camelCaseSerializeOptions)!;
        }

        private async Task<int> GetUsersCountAsync(CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");

            var adminUrl = _keycloakOptions.KeycloakUrlRealm.Replace("/realms", "/admin/realms");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{adminUrl}/users/count");

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

            return JsonSerializer.Deserialize<int>(outputString, _camelCaseSerializeOptions)!;
        }
    }
}
