using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.Login;
using System.Net;
using System.Text.Json;

namespace Odin.Auth.EndToEndTests.Controllers.Auth.SignIn
{
    [Collection(nameof(SignInTestFixtureCollection))]
    public class SignInApiTest
    {
        private readonly SignInApiTestFixture _fixture;

        public SignInApiTest(SignInApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should authenticate with valid username and password")]
        [Trait("E2E/Controllers", "Auth / [v1]SignIn")]
        public async Task AuthValid()
        {
            var input = _fixture.GetValidLoginInput();

            var (response, output) = await _fixture.ApiClient.PostAsync<LoginOutput>($"/v1/auth/sign-in", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output.AccessToken.Should().NotBeEmpty();
            output.IdToken.Should().NotBeEmpty();
            output.RefreshToken.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Should throw an error with invalid username and password")]
        [Trait("E2E/Controllers", "Auth / [v1]SignIn")]
        public async Task AuthInValid()
        {
            var input = _fixture.GetInvalidLoginInput();

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/auth/sign-in", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            output.Should().NotBeNull();
            output.Title.Should().Be("Invalid Authentication");
            output.Type.Should().Be("InvalidAuth");
            output.Status.Should().Be(StatusCodes.Status401Unauthorized);
            output.Detail.Should().Contain("Invalid user credentials");
        }

        [Fact(DisplayName = "Should throw an error with empty username")]
        [Trait("E2E/Controllers", "Auth / [v1]SignIn")]
        public async Task Auth_ThrowErrorWithEmptyUsername()
        {
            var input = new LoginInput("", "Odin@123!");

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/auth/sign-in", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);

            output.Extensions["errors"].Should().NotBeNull();
            var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(JsonSerializer.Serialize(output.Extensions["errors"]))!;
            errors.ContainsKey("Username").Should().BeTrue();
            errors["Username"].First().Should().Be("'Username' must not be empty.");

        }

        [Fact(DisplayName = "Should throw an error with empty password")]
        [Trait("E2E/Controllers", "Auth / [v1]SignIn")]
        public async Task Auth_ThrowErrorWithEmptyPassword()
        {
            var input = new LoginInput("admin", "");

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/auth/sign-in", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);

            output.Extensions["errors"].Should().NotBeNull();
            var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(JsonSerializer.Serialize(output.Extensions["errors"]))!;
            errors.ContainsKey("Password").Should().BeTrue();
            errors["Password"].First().Should().Be("'Password' must not be empty.");

        }
    }
}
