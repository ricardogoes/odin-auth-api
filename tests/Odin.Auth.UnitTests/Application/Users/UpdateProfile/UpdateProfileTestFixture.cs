using Bogus;
using Odin.Auth.Application.Users.UpdateProfile;

namespace Odin.Auth.UnitTests.Application.Users.UpdateProfile
{
    [CollectionDefinition(nameof(UpdateProfileTestFixtureCollection))]
    public class UpdateProfileTestFixtureCollection : ICollectionFixture<UpdateProfileTestFixture>
    { }

    public class UpdateProfileTestFixture : BaseFixture
    {
        public UpdateProfileTestFixture()
            : base() { }

        public UpdateProfileInput GetValidUpdateProfileInput(Guid? userId = null)
        {
            return new UpdateProfileInput
            (
               tenantId: Guid.NewGuid(),
                userId: userId ?? Guid.NewGuid(),
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public UpdateProfileInput GetInputWithEmptyFirstName()
        {
            return new UpdateProfileInput
            (
               tenantId: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                firstName: "",
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public UpdateProfileInput GetInputWithEmptyLastName()
        {
            return new UpdateProfileInput
            (
               tenantId: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                firstName: Faker.Person.FirstName,
                lastName: "",
                email: Faker.Person.Email,
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public UpdateProfileInput GetInputWithEmptyEmail()
        {
            return new UpdateProfileInput
            (
               tenantId: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: "",
                groups: new List<string> { "role-01 " },
                loggedUsername: "admin"
            );
        }

        public UpdateProfileInput GetInputWithEmptyRole()
        {
            return new UpdateProfileInput
            (
               tenantId: Guid.NewGuid(),
                userId: Guid.NewGuid(),
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: "",
                groups: new List<string>(),
                loggedUsername: "admin"
            );
        }
    }
}
