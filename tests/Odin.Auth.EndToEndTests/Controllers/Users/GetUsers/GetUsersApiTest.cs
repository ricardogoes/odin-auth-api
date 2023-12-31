﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Odin.Auth.Api.Models;
using Odin.Auth.Application.Users;
using Odin.Auth.Application.Users.GetUsers;
using System.Net;

namespace Odin.Auth.EndToEndTests.Controllers.Users.GetUsers
{

    [Collection(nameof(GetUsersApiTestCollection))]
    public class GetUsersApiTest
    {
        private readonly GetUsersApiTestFixture _fixture;

        public GetUsersApiTest(GetUsersApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should get users with default data")]
        [Trait("E2E/Controllers", "Users / [v1]GetUsers")]
        public async Task GetUsers()
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var users = await _fixture.ApiClient.GetUsers("admin.sinapse", "Odin@123!");

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<UserOutput>>("/v1/users");

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output.PageNumber.Should().Be(1);
            output.PageSize.Should().Be(5);
            output.TotalRecords.Should().Be(users.Count());
            output.TotalPages.Should().Be(users.Count() <= 5 ? 1 : users.Count() / 5);
            output.Items.Should().HaveCount(users.Count() < 5 ? users.Count() : 5);
        }

        [Fact(DisplayName = "Should return valid data")]
        [Trait("E2E/Controllers", "Users / [v1]GetUsers")]
        public async Task ListCategoriesAndTotal()
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var users = await _fixture.ApiClient.GetUsers("admin.sinapse", "Odin@123!");

            var input = new GetUsersInput(1, 5);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<UserOutput>>("/v1/users", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(users.Count());
            output.TotalPages.Should().Be(users.Count() <= 5 ? 1 : users.Count() / 5);
            output.Items.Should().HaveCount(users.Count() < 5 ? users.Count() : input.PageSize);

            foreach (var outputItem in output.Items)
            {
                var user = users.FirstOrDefault(x => x.Id == outputItem.Id);
                user.Should().NotBeNull();
                outputItem.Username.Should().Be(user!.Username);
                outputItem.FirstName.Should().Be(user.FirstName);
                outputItem.LastName.Should().Be(user.LastName);
                outputItem.Email.Should().Be(user.Email);
                outputItem.IsActive.Should().Be(user.Enabled!.Value);
            }
        }

        [Theory(DisplayName = "Should return paginated data")]
        [Trait("E2E/Controllers", "Users / [v1]GetUsers")]
        [InlineData(1, 5)]
        [InlineData(2, 5)]        
        [InlineData(3, 5)]
        public async Task ListPaginated(int page, int pageSize)
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var users = await _fixture.ApiClient.GetUsers("admin.sinapse", "Odin@123!"); 
            
            var input = new GetUsersInput(page, pageSize);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<UserOutput>>("/v1/users", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            
            output.Should().NotBeNull();
            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(users.Count());

            foreach (var outputItem in output.Items)
            {
                var user = users.FirstOrDefault(x => x.Id == outputItem.Id);
                user.Should().NotBeNull();
                outputItem.Username.Should().Be(user!.Username);
                outputItem.FirstName.Should().Be(user.FirstName);
                outputItem.LastName.Should().Be(user.LastName);
                outputItem.Email.Should().Be(user.Email);
                outputItem.IsActive.Should().Be(user.Enabled!.Value);
            }
        }

        [Theory(DisplayName = "Should return filtered data by name")]
        [Trait("E2E/Controllers", "Users / [v1]GetUsers")]
        [InlineData("admin.sinapse", 1, 5, 1)]
        [InlineData("baseline.sinapse", 1, 5, 1)]
        public async Task SearchByUsername(string search, int page, int pageSize, int expectedQuantityItemsReturned)
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var users = await _fixture.ApiClient.GetUsers("admin.sinapse", "Odin@123!");

            var input = new GetUsersInput(page, pageSize, username: search);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<UserOutput>>("/v1/users", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(expectedQuantityItemsReturned);
            output.Items.Should().HaveCount(expectedQuantityItemsReturned);

            foreach (var outputItem in output.Items)
            {
                var user = users.FirstOrDefault(x => x.Id == outputItem.Id);
                user.Should().NotBeNull();
                outputItem.Username.Should().Be(user!.Username);
                outputItem.FirstName.Should().Be(user.FirstName);
                outputItem.LastName.Should().Be(user.LastName);
                outputItem.Email.Should().Be(user.Email);
                outputItem.IsActive.Should().Be(user.Enabled!.Value);
            }
        }        

        [Theory(DisplayName = "Should return users ordered by field")]
        [Trait("E2E/Controllers", "Users / [v1]GetUsers")]
        [InlineData("firstname")]
        [InlineData("firstname desc")]
        [InlineData("id")]
        [InlineData("id desc")]
        public async Task ListOrdered(string orderBy)
        {
            var context = await _fixture.CreateDbContextAsync();
            await _fixture.SeedCustomerDataAsync(context);

            var users = await _fixture.ApiClient.GetUsers("admin.sinapse", "Odin@123!"); 
            var input = new GetUsersInput(1, 5, sort: orderBy);

            var (response, output) = await _fixture.ApiClient.GetAsync<PaginatedApiResponse<UserOutput>>("/v1/users", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();

            output.PageNumber.Should().Be(input.PageNumber);
            output.PageSize.Should().Be(input.PageSize);
            output.TotalRecords.Should().Be(users.Count());
            output.TotalPages.Should().Be(users.Count() <= 5 ? 1 : users.Count() / 5);
            output.Items.Should().HaveCount(users.Count() < 5 ? users.Count() : input.PageSize);
        }
    }
}
