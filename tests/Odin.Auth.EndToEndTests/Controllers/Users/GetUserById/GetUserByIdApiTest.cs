﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var (response, output) = await _fixture.ApiClient.GetByIdAsync<UserOutput>($"/v1/users/{_fixture.CommonUserId}");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Id.Should().Be(_fixture.CommonUserId);
            output.Username.Should().Be("common_user");
            output.FirstName.Should().Be("Common");
            output.LastName.Should().Be("User");
            output.Enabled.Should().BeTrue();

            output.Attributes.Should().NotBeNull();
            output.Attributes.Should().HaveCount(4);            
            output.Attributes.ContainsKey("created_at").Should().BeTrue();
            output.Attributes.ContainsKey("created_by").Should().BeTrue();
            output.Attributes.ContainsKey("last_updated_at").Should().BeTrue();
            output.Attributes.ContainsKey("last_updated_by").Should().BeTrue();

            output.Groups.Should().NotBeNull();
            output.Groups.Should().HaveCount(1);
            output.Groups.First().Name.Should().Be("common-user");
        }

        [Fact(DisplayName = "Should throw an error when user not found")]
        [Trait("E2E/Controllers", "Users / [v1]GetUserById")]
        public async Task ErrorWhenNotFound()
        {
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