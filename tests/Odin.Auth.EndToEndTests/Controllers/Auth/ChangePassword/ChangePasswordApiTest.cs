using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.Auth.Login;
using Odin.Auth.Application.Auth.ChangePassword;
using System.Net;
using System.Text.Json;

namespace Odin.Auth.EndToEndTests.Controllers.Auth.ChangePassword
{
    [Collection(nameof(ChangePasswordTestFixtureCollection))]
    public class ChangePasswordApiTest
    {
        private readonly ChangePasswordApiTestFixture _fixture;
                
        public ChangePasswordApiTest(ChangePasswordApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should change password successfully")]
        [Trait("E2E/Controllers", "Auth / [v1]ChangePassword")]
        public async Task ChangePasswordValid()
        {
            try
            {
                var context = await _fixture.CreateDbContextAsync();
                await _fixture.SeedCustomerDataAsync(context);

                var input = _fixture.GetValidChangePasswordInput(_fixture.TenantSinapseId);
                var (response, _) = await _fixture.ApiClient.PostAsync<LoginOutput>($"/v1/auth/change-password", input);

                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);

                var inputLogin = new LoginInput("baseline.sinapse", "admin");

                var (responseLogin, outputLogin) = await _fixture.ApiClient.PostAsync<LoginOutput>($"/v1/auth/sign-in", inputLogin);

                responseLogin.Should().NotBeNull();
                responseLogin.StatusCode.Should().Be(HttpStatusCode.OK);

                outputLogin.Should().NotBeNull();
                outputLogin.AccessToken.Should().NotBeEmpty();
                outputLogin.IdToken.Should().NotBeEmpty();
                outputLogin.RefreshToken.Should().NotBeEmpty();
            }
            finally
            {
                var changePasswordInput = new ChangePasswordInput(_fixture.TenantSinapseId, _fixture.CommonUserId, "Odin@123!", temporary: false);
                await _fixture.ApiClient.PostAsync<LoginOutput>($"/v1/auth/change-password", changePasswordInput);
            }
        }

        [Fact(DisplayName = "Should throw an error with empty user id")]
        [Trait("E2E/Controllers", "Auth / [v1]ChangePassword")]
        public async Task Auth_ThrowErrorWithEmptyUserId()
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            ValidatorOptions.Global.LanguageManager.Enabled = false;
            var input = new ChangePasswordInput(_fixture.TenantSinapseId, Guid.Empty, "new-password", temporary: true);

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/auth/change-password", input);

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

        [Fact(DisplayName = "Should throw an error with empty new password")]
        [Trait("E2E/Controllers", "Auth / [v1]ChangePassword")]
        public async Task Auth_ThrowErrorWithEmptyNewPassword()
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var input = new ChangePasswordInput(_fixture.TenantSinapseId, Guid.NewGuid(), "", temporary: true);

            var (response, output) = await _fixture.ApiClient.PostAsync<ProblemDetails>($"/v1/auth/change-password", input);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            output.Should().NotBeNull();
            output.Title.Should().Be("Unprocessable entity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);

            output.Extensions["errors"].Should().NotBeNull();
            var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(JsonSerializer.Serialize(output.Extensions["errors"]))!;
            errors.ContainsKey("NewPassword").Should().BeTrue();
            errors["NewPassword"].First().Should().Be("'New Password' must not be empty.");

        }
    }
}
