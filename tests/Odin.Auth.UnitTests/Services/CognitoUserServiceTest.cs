using FluentAssertions;
using Odin.Auth.Domain.Interfaces.Services;
using Odin.Auth.Domain.Models.UpdateProfile;
using Odin.Auth.Domain.Models.UserLogin;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Odin.Auth.UnitTests.Services
{
    public class CognitoUserServiceTest : TestBed<TestFixture>
    {
        public CognitoUserServiceTest(ITestOutputHelper testOutputHelper, TestFixture fixture)
            : base(testOutputHelper, fixture)
        {
        }

        [Fact(DisplayName = "InsertUserAsync() should return OK with valid data")]
        [Trait("Category", "Cognito User Service Tests")]
        public async Task InsertUserAsync_Ok()
        {
            var cognitoUserService = _fixture.GetService<ICognitoUserService>(_testOutputHelper);
            var response = await cognitoUserService.InsertUserAsync(new InsertUserRequest
            {
                Username = "unit.testing",
                FirstName = "Unit",
                LastName = "Testing",
                EmailAddress = "unit.testing@email.com",
                Password = "testing123!"
            });

            response.Username.Should().Be("unit.testing");
            response.EmailAddress.Should().Be("unit.testing@email.com");
        }

        [Fact(DisplayName = "UpdateUserAttributesAsync() should return OK with valid data")]
        [Trait("Category", "Cognito User Service Tests")]
        public async Task UpdateUserAttributesAsync_OK()
        {
            var cognitoUserService = _fixture.GetService<ICognitoUserService>(_testOutputHelper);
            var response = await cognitoUserService.UpdateUserAttributesAsync(new UpdateProfileRequest
            {
                Username = "unit.testing",
                FirstName = "Unit",
                LastName = "Testing",
                EmailAddress = "unit.testing@email.com"
            });

            response.Username.Should().Be("unit.testing");
        }

        [Fact(DisplayName = "UpdateUserAttributesAsync() should throw UserNotFoundException when user does not exist")]
        [Trait("Category", "Cognito User Service Tests")]
        public void UpdateUserAttributesAsync_throws_UserNotFoundException()
        {
            var cognitoUserService = _fixture.GetService<ICognitoUserService>(_testOutputHelper);
            var ex = Assert.Throws<AggregateException>(() => cognitoUserService.UpdateUserAttributesAsync(new UpdateProfileRequest
            {
                Username = "user.not.found",
                FirstName = "User",
                LastName = "Not Found",
                EmailAddress = "user.not.found@email.com"
            }).Result);

            ex.Message.Should().Contain("User not found");
        }

        [Fact(DisplayName = "EnableUserAsync() should return OK with valid data")]
        [Trait("Category", "Cognito User Service Tests")]
        public async Task EnableUserAsync_OK()
        {
            var cognitoUserService = _fixture.GetService<ICognitoUserService>(_testOutputHelper);
            var response = await cognitoUserService.EnableUserAsync("unit.testing");

            response.Username.Should().Be("unit.testing");
        }

        [Fact(DisplayName = "EnableUserAsync() should throw UserNotFoundException when user does not exist")]
        [Trait("Category", "Cognito User Service Tests")]
        public void EnableUserAsync_throws_UserNotFoundException()
        {
            var cognitoUserService = _fixture.GetService<ICognitoUserService>(_testOutputHelper);
            var ex = Assert.Throws<AggregateException>(() => cognitoUserService.EnableUserAsync("user.not.found").Result);

            ex.Message.Should().Contain("User not found");
        }

        [Fact(DisplayName = "DisableUserAsync() should return OK with valid data")]
        [Trait("Category", "Cognito User Service Tests")]
        public async Task DisableUserAsync_OK()
        {
            var cognitoUserService = _fixture.GetService<ICognitoUserService>(_testOutputHelper);
            var response = await cognitoUserService.DisableUserAsync("unit.testing");

            response.Username.Should().Be("unit.testing");
        }

        [Fact(DisplayName = "DisableUserAsync() should throw UserNotFoundException when user does not exist")]
        [Trait("Category", "Cognito User Service Tests")]
        public void DisableUserAsync_throws_UserNotFoundException()
        {
            var cognitoUserService = _fixture.GetService<ICognitoUserService>(_testOutputHelper);
            var ex = Assert.Throws<AggregateException>(() => cognitoUserService.DisableUserAsync("user.not.found").Result);

            ex.Message.Should().Contain("User not found");
        }

        [Fact(DisplayName = "GetUserByUsernameAsync() should return OK with valid data")]
        [Trait("Category", "Cognito User Service Tests")]
        public async Task GetUserByUsernameAsync_OK()
        {
            var cognitoUserService = _fixture.GetService<ICognitoUserService>(_testOutputHelper);
            var response = await cognitoUserService.GetUserByUsernameAsync("unit.testing");

            response.Username.Should().Be("unit.testing");
            response.EmailAddress.Should().Be("unit.testing@email.com");
        }

        [Fact(DisplayName = "GetUserByUsernameAsync() should throw UserNotFoundException when user does not exist")]
        [Trait("Category", "Cognito User Service Tests")]
        public void GetUserByUsernameAsync_throws_UserNotFoundException()
        {
            var cognitoUserService = _fixture.GetService<ICognitoUserService>(_testOutputHelper);
            var ex = Assert.Throws<AggregateException>(() => cognitoUserService.GetUserByUsernameAsync("user.not.found").Result);

            ex.Message.Should().Contain("User not found");
        }
    }
}