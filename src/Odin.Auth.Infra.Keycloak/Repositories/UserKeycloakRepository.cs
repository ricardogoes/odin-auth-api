using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Models.AppSettings;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Keycloak.Exceptions;
using Odin.Auth.Infra.Keycloak.Interfaces;
using Odin.Auth.Infra.Keycloak.Mappers;
using Odin.Auth.Infra.Keycloak.Models;
using Odin.Auth.Infra.Messaging.Policies;
using System.Net.Http.Json;
using System.Text.Json;

namespace Odin.Auth.Infra.Keycloak.Repositories
{
    public class UserKeycloakRepository : BaseRepository, IUserKeycloakRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;


        private readonly JsonSerializerOptions _snakeCaseSerializeOptions;
        private readonly JsonSerializerOptions _camelCaseSerializeOptions;
        private readonly AppSettings _appSettings;

        public UserKeycloakRepository(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, AppSettings appSettings)
            : base(httpContextAccessor)
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

        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}";

            var userRepresentation = user.ToUserRepresentation();
            userRepresentation.Attributes = new Dictionary<string, List<string>>
            {
                { "tenant_id", new List<string> { GetTenantId().ToString() } },
                { "created_at", new List<string> { DateTime.Now.ToString("o") } },
                { "created_by", new List<string> { GetCurrentUsername() } },
                { "last_updated_at", new List<string> { DateTime.Now.ToString("o") } },
                { "last_updated_by", new List<string> { GetCurrentUsername() } }
            };

            var response = await client.PostAsJsonAsync($"{keycloakUrlRealm}/users", userRepresentation, cancellationToken);

            var outputString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = JsonSerializer.Deserialize<KeycloakResponseError>(outputString, _snakeCaseSerializeOptions)!;

                var message = errorContent.Error;
                if (!string.IsNullOrWhiteSpace(errorContent.ErrorDescription))
                    message += $". ERROR DETAIL: {errorContent.ErrorDescription}";

                throw new KeycloakException(message);
            }

            var groups = userRepresentation.Groups!.Select(x => new UserGroup(x)).ToList();
            return userRepresentation.ToUser(groups);
        }

        public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}";

            var userRepresentation = user.ToUserRepresentation();
            userRepresentation.Attributes = new Dictionary<string, List<string>>
            {
                { "created_at", new List<string> { user.CreatedAt!.Value.ToString("o") } },
                { "created_by", new List<string> { user.CreatedBy! } },
                { "last_updated_at", new List<string> { DateTime.Now.ToString("o") } },
                { "last_updated_by", new List<string> { GetCurrentUsername() } }
            };

            var response = await client.PutAsJsonAsync($"{keycloakUrlRealm}/users/{user.Id}", user.ToUserRepresentation(), cancellationToken);

            var outputString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = JsonSerializer.Deserialize<KeycloakResponseError>(outputString, _snakeCaseSerializeOptions)!;

                var message = errorContent.Error;
                if (!string.IsNullOrWhiteSpace(errorContent.ErrorDescription))
                    message += $". ERROR DETAIL: {errorContent.ErrorDescription}";

                throw new KeycloakException(message);
            }

            return userRepresentation.ToUser();
        }

        public async Task<User> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}";

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{keycloakUrlRealm}/users/{id}");

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
            if (userRepresentation.Attributes?["tenant_id"].First().ToLower() != GetTenantId().ToString().ToLower())
                throw new NotFoundException($"User with ID '{id}' not found");

            var userGroups = await FindGroupsByUserIdAsync(id, cancellationToken);

            return userRepresentation.ToUser(userGroups.ToList());
        }

        public async Task<IEnumerable<User>> FindUsersAsync(CancellationToken cancellationToken)
        {
            var countUsers = await GetUsersCountAsync(cancellationToken);

            var client = _httpClientFactory.CreateClient("Keycloak");

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}";

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{keycloakUrlRealm}/users?first=0&max={countUsers}&q=tenant_id:{GetTenantId()}");

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
            
            var usersRepresentation = JsonSerializer.Deserialize<List<UserRepresentation>>(outputString, _camelCaseSerializeOptions)!;
            var dicUsersGroups = new Dictionary<Guid, List<UserGroup>>();

            foreach (var userRepresentation in usersRepresentation) 
            {
                var userGroups = await FindGroupsByUserIdAsync(userRepresentation.Id!.Value, cancellationToken);
                dicUsersGroups.Add(userRepresentation.Id!.Value, userGroups.ToList());
            }

            return usersRepresentation.ToUser(dicUsersGroups);
        }

        public async Task<IEnumerable<UserGroup>> FindGroupsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}";

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{keycloakUrlRealm}/users/{userId}/groups");

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

        private async Task<int> GetUsersCountAsync(CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("Keycloak");

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/{_appSettings.KeycloakSettings!.Realm}";

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{keycloakUrlRealm}/users/count?q=tenant_id:{GetTenantId()}");

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
