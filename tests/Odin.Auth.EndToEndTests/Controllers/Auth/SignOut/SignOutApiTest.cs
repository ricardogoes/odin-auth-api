using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.Auth.Login;
using Odin.Auth.Application.Auth.Logout;
using System.Net;
using System.Text.Json;

namespace Odin.Auth.EndToEndTests.Controllers.Auth.SignOut
{
    [Collection(nameof(SignOutTestFixtureCollection))]
    public class SignOutApiTest
    {
        private readonly SignOutAPiTestFixture _fixture;

        public SignOutApiTest(SignOutAPiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should logoff successfully with valid session")]
        [Trait("E2E/Controllers", "Auth / [v1]SignOut")]
        public async Task LogoutValid()
        {
            var input = _fixture.GetValidLogoutInput();

            var (response, _) = await _fixture.ApiClient.PostAsync<LoginOutput>($"/v1/auth/sign-out", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "Should throw an error with empty user id")]
        [Trait("E2E/Controllers", "Auth / [v1]SignOut")]
        public async Task Auth_ThrowErrorWithEmptyUserId()
        {
            var input = new LogoutInput(Guid.Empty);

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/auth/sign-out", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);

            output.Extensions["errors"].Should().NotBeNull();
            var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(JsonSerializer.Serialize(output.Extensions["errors"]))!;
            errors.ContainsKey("UserId").Should().BeTrue();
            errors["UserId"].First().Should().Be("'User Id' must not be empty.");

        }
    }
}
