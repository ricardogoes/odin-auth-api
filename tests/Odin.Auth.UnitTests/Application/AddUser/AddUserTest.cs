using FluentAssertions;
using Odin.Auth.Infra.Cognito;
using Xunit.Abstractions;
using App = Odin.Auth.Application.AddUser;

namespace Odin.Auth.UnitTests.Application.AddUser
{
    [Collection(nameof(AddUserTestFixtureCollection))]
    public class AddUserTest
    {
        private readonly AddUserTestFixture _fixture;
        private readonly AmazonCognitoIdentityRepositoryMock _awsIdentityRepository;

        public AddUserTest(AddUserTestFixture fixture)
        {
            _fixture = fixture;
            _awsIdentityRepository = _fixture.GetAwsIdentityRepository();
        }

        [Fact(DisplayName = "Handle() should return OK with valid data")]
        [Trait("Application", "AddUser / AddUser")]
        public async Task InsertUserAsync_Ok()
        {
            var app = new App.AddUser(_fixture.AppSettings, _awsIdentityRepository);
            var output = await app.Handle(new App.AddUserInput
            {
                Username = "unit.testing",
                FirstName = "Unit",
                LastName = "Testing",
                EmailAddress = "unit.testing@email.com",
                Password = "testing123!"
            }, CancellationToken.None);

            output.Username.Should().Be("unit.testing");
            output.EmailAddress.Should().Be("unit.testing@email.com");
        }
    }
}
