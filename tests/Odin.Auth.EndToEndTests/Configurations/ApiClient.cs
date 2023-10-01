using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Odin.Auth.Domain.Models.AppSettings;
using Odin.Auth.EndToEndTests.Models;
using Odin.Auth.Infra.Keycloak.Models;
using Odin.Auth.Infra.Messaging.Policies;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Odin.Auth.EndToEndTests.Configurations
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _defaultSerializeOptions;
        private readonly AppSettings _appSettings;

        private readonly Guid _tenantId; 

        private const string USERNAME = "admin.sinapse";
        private const string PASSWORD = "Odin@123!";

        public ApiClient(HttpClient httpClient, AppSettings appSettings, Guid tenantId)
        {
            _httpClient = httpClient;
            _defaultSerializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true
            };
            _appSettings = appSettings;
            _tenantId = tenantId;

            AddAuthorizationHeader(_tenantId, USERNAME, PASSWORD);
        }

        private void AddAuthorizationHeader(Guid tenantId, string user, string password)
        {
            var accessToken = GetAccessTokenAsync(user, password).Result;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Add("X-TENANT-ID", tenantId.ToString());
        }

        public async Task<string> GetAccessTokenAsync(string user, string password)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!)
            };
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/x-www-form-urlencoded");

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}realms/odin-realm";

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{keycloakUrlRealm}/protocol/openid-connect/token");

            var collection = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "password"),
                new("client_id", _appSettings.KeycloakSettings!.Resource!),
                new("client_secret", _appSettings.KeycloakSettings!.Credentials!.Secret!),
                new("username", user),
                new("password", password)
            };

            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);
            var credentials = await GetOutputAsync<Credentials>(response);
            return credentials!.AccessToken;
        }

        public async Task<IEnumerable<UserRepresentation>> GetUsers(string user, string password)
        {
            var accessToken = GetAccessTokenAsync(user, password).Result; 
            
            var client = new HttpClient
            {
                BaseAddress = new Uri(_appSettings.KeycloakSettings!.AuthServerUrl!)
            };
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var keycloakUrlRealm = $"{_appSettings.KeycloakSettings!.AuthServerUrl}/admin/realms/odin-realm";

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{keycloakUrlRealm}/users?q=tenant_id:{_tenantId}");

            var response = await client.SendAsync(request);

            var users = await GetOutputAsync<IEnumerable<UserRepresentation>>(response);

            return users;
        }

        public async Task<(HttpResponseMessage, TOutput)> PostAsync<TOutput>(string route, object? request) where TOutput : class
        {
            var response = await _httpClient.PostAsJsonAsync(route, request, _defaultSerializeOptions);
            var output = await GetOutputAsync<TOutput>(response);

            return (response, output);
        }

        public async Task<(HttpResponseMessage, TOutput)> PutAsync<TOutput>(string route, object? request) where TOutput : class
        {
            var response = await _httpClient.PutAsJsonAsync(route, request, _defaultSerializeOptions);
            var output = await GetOutputAsync<TOutput>(response);

            return (response, output);
        }

        public async Task<(HttpResponseMessage, TOutput)> GetByIdAsync<TOutput>(string route, object? queryStringParametersObject = null) where TOutput : class
        {
            var urlToCall = PrepareGetRoute(route, queryStringParametersObject);

            var response = await _httpClient.GetAsync(urlToCall);
            var output = await GetOutputAsync<TOutput>(response);

            return (response, output);
        }

        public async Task<(HttpResponseMessage, TOutput)> GetAsync<TOutput>(string route, object? queryStringParametersObject = null) where TOutput : class
        {
            var urlToCall = PrepareGetRoute(route, queryStringParametersObject);

            var response = await _httpClient.GetAsync(urlToCall);
            var output = await GetOutputAsync<TOutput>(response);

            return (response, output);
        }

        private async Task<TOutput> GetOutputAsync<TOutput>(HttpResponseMessage response) where TOutput : class
        {
            var outputString = await response.Content.ReadAsStringAsync();

            TOutput? output = null;

            if (!string.IsNullOrWhiteSpace(outputString))
                output = JsonSerializer.Deserialize<TOutput>(outputString, _defaultSerializeOptions);

            return output!;
        }

        private string PrepareGetRoute(string route, object? queryStringParametersObject)
        {
            if (queryStringParametersObject is null)
                return route;

            var parametersJson = JsonSerializer.Serialize(queryStringParametersObject, _defaultSerializeOptions);
            var parametersDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(parametersJson);

            return QueryHelpers.AddQueryString(route, parametersDictionary!);
        }
    }
}
