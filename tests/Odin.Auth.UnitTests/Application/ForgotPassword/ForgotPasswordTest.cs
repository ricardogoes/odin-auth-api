using Amazon.CognitoIdentityProvider.Model;
using FluentAssertions;
using Moq;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito;
using System.Net;
using App = Odin.Auth.Application.ForgotPassword;

namespace Odin.Auth.UnitTests.Application.ForgotPassword
{
    [Collection(nameof(ForgotPasswordTestFixtureCollection))]
    public class ForgotPasswordTest
    {
        private readonly ForgotPasswordTestFixture _fixture;
        private readonly AmazonCognitoIdentityRepositoryMock _awsIdentityRepository;
        private readonly Mock<ICommonService> _commonServiceMock;

        public ForgotPasswordTest(ForgotPasswordTestFixture fixture)
        {
            _fixture = fixture;
            _awsIdentityRepository = _fixture.GetAwsIdentityRepository();
            _commonServiceMock = new();
        }

        [Fact(DisplayName = "Handle() should return OK with valid data")]
        [Trait("Application", "ForgotPassword / ForgotPassword")]
        public async Task TryInitForgotPasswordAsync_Ok()
        {
            var expectedResult = new ListUsersResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Users = new List<UserType>
                {
                    new UserType
                    {
                        Username = "unit.testing",
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "unit.testing"},
                            new AttributeType {Name = "email", Value = "unit.testing@email.com"}
                        }
                    }
                }
            };

            _commonServiceMock.Setup(s => s.FindUsersByEmailAddressAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedResult));

            var app = new App.ForgotPassword(_fixture.AppSettings, _commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new App.ForgotPasswordInput
            {
                Username = "unit.testing"
            }, CancellationToken.None);

            response.Username.Should().Be("unit.testing");
        }

        [Fact(DisplayName = "Handle() should return not found when user does not exist")]
        [Trait("Application", "ForgotPassword / ForgotPassword")]
        public async Task TryInitForgotPasswordAsync_UserNotFound()
        {
            var app = new App.ForgotPassword(_fixture.AppSettings, _commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new App.ForgotPasswordInput
            {
                Username = "user.not.found"
            }, CancellationToken.None);

            response.Username.Should().BeEmpty();
            response.Message.Should().Be("Error trying to recover user data");
        }

        [Fact(DisplayName = "Handle() should return not found when username does not exist")]
        [Trait("Application", "ForgotPassword / ForgotPassword")]
        public async Task TryInitForgotPasswordAsync_UserFilteredNotFound()
        {
            var expectedResult = new ListUsersResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Users = new List<UserType>
                {
                    new UserType
                    {
                        Username = "unit.testing",
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "unit.testing"},
                            new AttributeType {Name = "email", Value = "unit.testing@email.com"}
                        }
                    },
                    new UserType
                    {
                        Username = "user.not.confirmed" ,
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "user.not.confirmed"},
                            new AttributeType {Name = "email", Value = "user.not.confirmed@email.com"}
                        }
                    },
                    new UserType
                    {
                        Username = "user.not.confirmed.not-sent" ,
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "user.not.confirmed.not-sent"},
                            new AttributeType {Name = "email", Value = "user.not.confirmed.not-sent@email.com"}
                        }
                    },
                    new UserType
                    {
                        Username = "user.with.error" ,
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "user.with.error"},
                            new AttributeType {Name = "email", Value = "user.with.error@email.com"}
                        }
                    }
                }
            };

            _commonServiceMock.Setup(s => s.FindUsersByEmailAddressAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedResult));

            var app = new App.ForgotPassword(_fixture.AppSettings, _commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new App.ForgotPasswordInput
            {
                Username = "user.filtered.not.found"
            }, CancellationToken.None);

            response.Username.Should().Be("user.filtered.not.found");
            response.Message.Should().Be("No users with the given username found");
        }

        [Fact(DisplayName = "Handle() should not send confirmation code when an error ocurred")]
        [Trait("Application", "ForgotPassword / ForgotPassword")]
        public async Task TryInitForgotPasswordAsync_NotSendConfirmationCodeWhenError()
        {
            var expectedResult = new ListUsersResponse
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Users = new List<UserType>
                {                    
                    new UserType
                    {
                        Username = "user.with.error" ,
                        Attributes = new List<AttributeType>
                        {
                            new AttributeType {Name = "preferred_username", Value = "user.with.error"},
                            new AttributeType {Name = "email", Value = "user.with.error@email.com"}
                        }
                    }
                }
            };

            _commonServiceMock.Setup(s => s.FindUsersByEmailAddressAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult(expectedResult));

            var app = new App.ForgotPassword(_fixture.AppSettings, _commonServiceMock.Object, _awsIdentityRepository);
            var response = await app.Handle(new App.ForgotPasswordInput
            {
                Username = "user.with.error"
            }, CancellationToken.None);

            response.Username.Should().Be("user.with.error");
            response.Message.Should().Be("ListUsers Response: BadRequest");
        }
    }
}
