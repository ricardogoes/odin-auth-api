using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odin.Auth.Application.Users;
using Odin.Auth.Domain.Models;
using System.Net;

namespace Odin.Auth.EndToEndTests.Controllers.Users.GetUserById
{

    [Collection(nameof(GetUserByIdApiTestCollection))]
    public class GetUserByIdApiTest
    {
        private readonly GetUserByIdApiTestFixture _fixture;

        public GetUserByIdApiTest(GetUserByIdApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get a user by valid id")]
        [Trait("E2E/Controllers", "Users / [v1]GetUserById")]
        public async Task GetUserById()
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<UserOutput>($"/v1/users/{_fixture.CommonUserId}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Id.Should().Be(_fixture.CommonUserId);
            output.Username.Should().Be("baseline.sinapse");
            output.FirstName.Should().Be("Baseline");
            output.LastName.Should().Be("Sinapse");
            output.IsActive.Should().BeTrue();
                        
            output.Groups.Should().NotBeNull();
            output.Groups.Should().HaveCount(1);
            output.Groups.First().Name.Should().Be("odin.baseline");
        }

        [Fact(DisplayName = "Should throw an error when user not found")]
        [Trait("E2E/Controllers", "Users / [v1]GetUserById")]
        public async Task ErrorWhenNotFound()
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var idToQuery = Guid.NewGuid();

            var (response, output) = await _fixture.ApiClient.GetByIdAsync<ProblemDetails>($"/v1/users/{idToQuery}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Type.Should().Be("NotFound");
            output.Title.Should().Be("Not found");
            output.Detail.Should().Be($"User with ID '{idToQuery}' not found");
        }
    }
}
