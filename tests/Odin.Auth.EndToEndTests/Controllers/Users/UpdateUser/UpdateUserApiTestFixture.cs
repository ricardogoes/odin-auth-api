using Odin.Auth.Application.Users.UpdateProfile;

namespace Odin.Auth.EndToEndTests.Controllers.Users.UpdateUser
{
    [CollectionDefinition(nameof(UpdateUserApiTestCollection))]
    public class UpdateUserApiTestCollection : ICollectionFixture<UpdateUserApiTestFixture>
    { }

    public class UpdateUserApiTestFixture : UserBaseFixture
    {
        public UpdateUserApiTestFixture()
            : base()
        { }

        public UpdateProfileInput GetValidInput(Guid id)
            => new
            (
                userId: id, 
                firstName: "Common", 
                lastName: "User", 
                email: "common-user@testemail.com", 
                groups: new List<string> { "test-group-01", "test-group-02" }
            );

        public UpdateProfileInput GetInputWithIdEmpty()
             => new
            (
                userId: Guid.Empty,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "aaa" }
            );


        public UpdateProfileInput GetInputWithFirstNameEmpty(Guid id)
             => new
            (
                userId: id,
                firstName: "",
                lastName: Faker.Person.LastName,
                email: Faker.Person.Email,
                groups: new List<string> { "aaa" }
            );

        public UpdateProfileInput GetInputWithLastNameEmpty(Guid id)
             => new
            (
                userId: id,
                firstName: Faker.Person.FirstName,
                lastName: "",
                email: Faker.Person.Email,
                groups: new List<string> { "aaa" }
            );

        public UpdateProfileInput GetInputWithEmailEmpty(Guid id)
             => new
            (
                userId: id,
                firstName: Faker.Person.FirstName,
                lastName: Faker.Person.LastName,
                email: "",
                groups: new List<string> { "aaa" }
            );
    }
}
