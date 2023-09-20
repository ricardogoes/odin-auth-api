using Bogus;
using Keycloak.AuthServices.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.EndToEndTests.Configurations;

namespace Odin.Auth.EndToEndTests
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }

        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }
        public ApiClient ApiClient { get; set; }

        public Guid AdminUserId = Guid.Parse("e4ce732d-1c3d-4384-8634-a1f1d7af112c");
        public Guid CommonUserId = Guid.Parse("7f678f36-53cf-484a-af16-f9bb2d706626");

        public BaseFixture()
        {
            Faker = new Faker("pt_BR");

            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();

            var configuration = WebAppFactory.Services.GetRequiredService<IConfiguration>();

            var keycloakOptions = configuration
                .GetSection(KeycloakAuthenticationOptions.Section)
                .Get<KeycloakAuthenticationOptions>();

            ApiClient = new ApiClient(HttpClient, keycloakOptions!);
            ArgumentNullException.ThrowIfNull(configuration);
        }

        public User GetValidUser()
        {
            var user = new User
            (
                Faker.Person.UserName,
                Faker.Person.FirstName,
                Faker.Person.LastName,
                Faker.Person.Email
            );

            user.AddGroup(new UserGroup("odin-group"));

            return user;
        }
    }
}

