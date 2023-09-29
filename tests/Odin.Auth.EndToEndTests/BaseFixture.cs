using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.Models.AppSettings;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.EndToEndTests.Configurations;
using Odin.Auth.Infra.Data.EF;
using Odin.Auth.Infra.Data.EF.Models;

namespace Odin.Auth.EndToEndTests
{
    public abstract class BaseFixture
    {
        protected Faker Faker { get; set; }

        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }
        public ApiClient ApiClient { get; set; }

        public Guid TenantSinapseId = Guid.Parse("5F9B7808-803F-4985-9996-6EBA9003F9CD");
        public Guid TenantMerxId = Guid.Parse("BEC7E4B4-2E23-4536-9B01-DC9E8D66ED5A");

        public Guid RealmAdminUserId = Guid.Parse("76097e63-7293-4a0e-866b-eb19f74f5b53"); // admin
        public Guid CommonUserId = Guid.Parse("b05ec6b0-ff33-4090-ad46-e7de1560c440"); // baseline.sinapse

        public BaseFixture()
        {
            Faker = new Faker("pt_BR");

            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();

            var configuration = WebAppFactory.Services.GetRequiredService<IConfiguration>();

            var connectionStrings = new ConnectionStringsSettings(Environment.GetEnvironmentVariable("OdinSettings:ConnectionStrings:OdinMasterDB")!);
            
            var keycloakSettings = configuration.GetSection("Keycloak").Get<KeycloakSettings>()!;
            keycloakSettings.Credentials!.Secret = Environment.GetEnvironmentVariable("OdinSettings:Keycloak:Credentials:Secret")!;

            var appSettings = new AppSettings(connectionStrings, keycloakSettings);

            ApiClient = new ApiClient(HttpClient, appSettings!, TenantSinapseId);
            ArgumentNullException.ThrowIfNull(configuration);
        }

        public async Task<OdinMasterDbContext> CreateDbContextAsync(bool preserveData = false)
        {
            var context = new OdinMasterDbContext(
                new DbContextOptionsBuilder<OdinMasterDbContext>()
                .UseInMemoryDatabase("e2e-tests-db")
                .Options
            );

            if (preserveData == false)
            { 
                context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
            }

            return await Task.FromResult(context);
        }

        public async Task SeedCustomerDataAsync(OdinMasterDbContext context)
        {
            var sinapseCustomer = new CustomerModel(Guid.Parse("5F9B7808-803F-4985-9996-6EBA9003F9CD"), "Sinapse", "17.440.310/0001-20", true, DateTime.UtcNow, "admin", DateTime.UtcNow, "admin");
            var merxCustomer = new CustomerModel(Guid.Parse("BEC7E4B4-2E23-4536-9B01-DC9E8D66ED5A"), "Merx", "58.985.598/0001-03", true, DateTime.UtcNow, "admin", DateTime.UtcNow, "admin");

            await context.AddRangeAsync(new List<CustomerModel> { sinapseCustomer, merxCustomer });
            await context.SaveChangesAsync(CancellationToken.None);
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

        public bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public string GetValidName()
           => Faker.Company.CompanyName(1);

        public string GetValidDocument()
            => Faker.Company.Cnpj();

        public string GetInvalidDocument()
           => "12.123.123/0002-12";

        public string GetValidUsername()
            => $"{Faker.Name.FirstName().ToLower()}.{Faker.Name.LastName().ToLower()}";

        public Customer GetValidCustomer()
        {
            var customer = new Customer(GetValidName(), GetValidDocument(), isActive: GetRandomBoolean());
            customer.Create("unit.testing");

            return customer;
        }

        public CustomerModel GetValidCustomerModel()
        {
            return new CustomerModel
            (
                id: Guid.NewGuid(),
                name: GetValidName(),
                document: GetValidDocument(),
                isActive: GetRandomBoolean(),
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test"
            );
        }
    }
}

