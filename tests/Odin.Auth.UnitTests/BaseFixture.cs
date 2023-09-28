using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.EntityFrameworkCore;
using Moq;
using Odin.Auth.Domain.Entities;
using Odin.Auth.Domain.ValueObjects;
using Odin.Auth.Domain.ValuesObjects;
using Odin.Auth.Infra.Data.EF;
using Odin.Auth.Infra.Data.EF.Models;
using Odin.Auth.Infra.Keycloak.Interfaces;

namespace Odin.Auth.UnitTests
{
    public abstract class BaseFixture
    {
        public Faker Faker { get; set; }

        protected BaseFixture()
            => Faker = new Faker("pt_BR");

        public OdinMasterDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new OdinMasterDbContext(
                new DbContextOptionsBuilder<OdinMasterDbContext>()
                .UseInMemoryDatabase("integration-tests-db")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options
            );

            if (preserveData == false)
                context.Database.EnsureDeleted();
            return context;
        }

        public Mock<IAuthKeycloakRepository> GetAuthKeycloakRepositoryMock()
            => new();

        public Mock<IUserKeycloakRepository> GetUserKeycloakRepositoryMock()
            => new();

        public User GetValidUser(Guid? tenantId = null)
        {
            Faker = new Faker("pt_BR");

            var randomTest = new Random();

            var startDate = new DateTime(2023, 1, 1);
            var endDate = DateTime.Now;

            TimeSpan timeSpan = endDate - startDate;
            TimeSpan newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
            DateTime newDate = startDate + newSpan;

            var user = new User
            (
                Guid.NewGuid(),
                Faker.Person.UserName,
                Faker.Person.FirstName,
                Faker.Person.LastName,
                Faker.Person.Email,
                new Random().NextDouble() < 0.5
            );

            user.AddCredentials(new UserCredential("password", true));

            user.AddGroup(new UserGroup(Guid.NewGuid(), "odin-group", "/odin-group"));

            user.AddAttribute(new KeyValuePair<string, string>("tenant_id", (tenantId ?? Guid.NewGuid()).ToString()));
            user.AddAttribute(new KeyValuePair<string, string>("created_at", newDate.ToString("o")));
            user.AddAttribute(new KeyValuePair<string, string>("created_by", $"user_{randomTest.Next(0, 100)}"));
            user.AddAttribute(new KeyValuePair<string, string>("last_updated_at", newDate.ToString("o")));
            user.AddAttribute(new KeyValuePair<string, string>("last_updated_by", $"user_{randomTest.Next(0, 100)}"));

            user.SetAuditLog(newDate, $"user_{randomTest.Next(0, 100)}", newDate, $"user_{randomTest.Next(0, 100)}");

            return user;
        }

        public string GetValidUsername()
            => Faker.Person.UserName;

        public string GetValidCustomerName()
            => Faker.Company.CompanyName(1);

        public string GetValidCustomerDocument()
            => Faker.Company.Cnpj();

        public Customer GetValidCustomer()
        {
            var customer = new Customer(GetValidCustomerName(), GetValidCustomerDocument());
            customer.Create("unit.testing");
            return customer;
        }

        public CustomerModel GetValidCustomerModel()
        {
            return new CustomerModel
            (
                id: Guid.NewGuid(),
                name: GetValidCustomerName(),
                document: GetValidCustomerDocument(),
                isActive: true,
                createdAt: DateTime.Now,
                createdBy: "unit.test",
                lastUpdatedAt: DateTime.Now,
                lastUpdatedBy: "unit.test"
            );
        }

        public Address GetValidAddress()
        {
            var address = new Address(
                Faker.Address.StreetName(),
                int.Parse(Faker.Address.BuildingNumber()),
                Faker.Address.SecondaryAddress(),
                Faker.Address.CardinalDirection(),
                Faker.Address.ZipCode(),
                Faker.Address.City(),
                Faker.Address.StateAbbr()
            );
            return address;
        }

        public List<Customer> GetValidCustomersList(int length = 10)
            => Enumerable.Range(1, length)
                .Select(_ => GetValidCustomer()).ToList();
    }
}
