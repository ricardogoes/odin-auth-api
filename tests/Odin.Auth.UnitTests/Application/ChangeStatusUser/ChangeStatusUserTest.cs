using Amazon.CognitoIdentityProvider.Model;
using FluentAssertions;
using Moq;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;
using App = Odin.Auth.Application.ChangeStatusUser;

namespace Odin.Auth.UnitTests.Application.ChangeStatusUser
{
    [Collection(nameof(ChangeStatusUserTestFixtureCollection))]
    public class ChangeStatusUserTest
    {
        private readonly ChangeStatusUserTestFixture _fixture;

        private readonly Mock<ICommonService> _commonServiceMock;
        private readonly AmazonCognitoIdentityRepositoryMock _awsIdentityRepository;

        public ChangeStatusUserTest(ChangeStatusUserTestFixture fixture)
        {
            _fixture = fixture;

            _commonServiceMock = new();
            _awsIdentityRepository = _fixture.GetAwsIdentityRepository();
        }

        [Fact(DisplayName = "Handle() should throw UserNotFoundException when user does not exist")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUser")]
        public void EnableUserAsync_throws_UserNotFoundException()
        {
            _commonServiceMock.Setup(s => s.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(new UserNotFoundException("User not found"));

            var app = new App.ChangeStatusUser(_fixture.AppSettings, _awsIdentityRepository, _commonServiceMock.Object);

            var ex = Assert.Throws<AggregateException>(() => app.Handle(new App.ChangeStatusUserInput
            (
                username: _fixture.Faker.Person.UserName,
                action: App.ChangeStatusAction.ACTIVATE
            ), CancellationToken.None).Result);

            ex.Message.Should().Contain("User not found");
        }

        [Fact(DisplayName = "Handle() should return OK when activating a valid user")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUser")]
        public async Task EnableUserAsync_OK()
        {
            var expectedOuput = new UserProfileResponse
            (
                username: _fixture.Faker.Person.UserName, 
                firstName: _fixture.Faker.Person.FirstName,
                lastName: _fixture.Faker.Person.LastName,                
                emailAddress: _fixture.Faker.Person.Email,
                preferredUsername: _fixture.Faker.Person.UserName
            );

            _commonServiceMock.Setup(s => s.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedOuput));

            var app = new App.ChangeStatusUser(_fixture.AppSettings, _awsIdentityRepository, _commonServiceMock.Object);
            
            var output = await app.Handle(new App.ChangeStatusUserInput
            (
                username: expectedOuput.Username,
                action: App.ChangeStatusAction.ACTIVATE
            ), CancellationToken.None);

            output.Username.Should().Be(expectedOuput.Username);
        }

        [Fact(DisplayName = "Handle() should return OK when deactivating a valid user")]
        [Trait("Application", "ChangeStatusUser / ChangeStatusUser")]
        public async Task DisableUserAsync_OK()
        {
            var expectedOuput = new UserProfileResponse
            (
                username: _fixture.Faker.Person.UserName,
                firstName: _fixture.Faker.Person.FirstName,
                lastName: _fixture.Faker.Person.LastName,                
                emailAddress: _fixture.Faker.Person.Email,
                preferredUsername: _fixture.Faker.Person.UserName
            );

            _commonServiceMock.Setup(s => s.GetUserByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedOuput));

            var app = new App.ChangeStatusUser(_fixture.AppSettings, _awsIdentityRepository, _commonServiceMock.Object);

            var output = await app.Handle(new App.ChangeStatusUserInput
            (
                username: expectedOuput.Username,
                action: App.ChangeStatusAction.DEACTIVATE
            ), CancellationToken.None);

            output.Username.Should().Be(expectedOuput.Username);
        }
    }
}
