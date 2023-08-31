using Amazon.CognitoIdentityProvider.Model;
using FluentAssertions;
using Moq;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;
using Xunit.Abstractions;
using App = Odin.Auth.Application.UpdateProfile;

namespace Odin.Auth.UnitTests.Application.UpdateProfile
{
    [Collection(nameof(UpdateProfileTestFixtureCollection))]
    public class UpdateProfileTest
    {
        private readonly UpdateProfileTestFixture _fixture;

        private readonly Mock<ICommonService> _commonServiceMock;
        private readonly AmazonCognitoIdentityRepositoryMock _awsIdentityRepository;

        public UpdateProfileTest(UpdateProfileTestFixture fixture)
        {
            _fixture = fixture;

            _commonServiceMock = new();
            _awsIdentityRepository = _fixture.GetAwsIdentityRepository();
        }

        [Fact(DisplayName = "Handle() should return OK with valid data")]
        [Trait("Application", "UpdateProfile / UpdateProfile")]
        public async Task UpdateUserAttributesAsync_OK()
        {
            var expectedOuput = new UserProfileResponse
            {
                FirstName = _fixture.Faker.Person.FirstName,
                LastName = _fixture.Faker.Person.LastName,
                Username = _fixture.Faker.Person.UserName,
                EmailAddress = _fixture.Faker.Person.Email,
                PreferredUsername = _fixture.Faker.Person.UserName,
            };

            _commonServiceMock.Setup(s => s.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedOuput));

            var app = new App.UpdateProfile(_fixture.AppSettings, _awsIdentityRepository, _commonServiceMock.Object);
            var output = await app.Handle(new App.UpdateProfileInput
            {
                Username = expectedOuput.Username,
                FirstName = expectedOuput.FirstName,
                LastName = expectedOuput.LastName,
                EmailAddress = expectedOuput.EmailAddress
            }, CancellationToken.None);

            output.Username.Should().Be(expectedOuput.Username);
        }

        [Fact(DisplayName = "Handle() should throw UserNotFoundException when user does not exist")]
        [Trait("Application", "UpdateProfile / UpdateProfile")]
        public void UpdateUserAttributesAsync_throws_UserNotFoundException()
        {
            _commonServiceMock.Setup(s => s.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new UserNotFoundException("User not found"));

            var app = new App.UpdateProfile(_fixture.AppSettings, _awsIdentityRepository, _commonServiceMock.Object);
            
            var ex = Assert.Throws<AggregateException>(() => app.Handle(new App.UpdateProfileInput
            {
                Username = _fixture.Faker.Person.UserName,
                FirstName = _fixture.Faker.Person.FirstName,
                LastName = _fixture.Faker.Person.LastName,
                EmailAddress = _fixture.Faker.Person.Email
            }, CancellationToken.None).Result);

            ex.Message.Should().Contain("User not found");
        }
    }
}
