﻿using FluentValidation;
using MediatR;
using Odin.Auth.Application.Helpers;
using Odin.Auth.Domain.Exceptions;
using Odin.Auth.Domain.Interfaces;
using Odin.Auth.Domain.Models;

namespace Odin.Auth.Application.GetUsers
{
    public class GetUsers : IRequestHandler<GetUsersInput, PaginatedListOutput<UserOutput>>
    {
        private readonly IKeycloakRepository _keycloakRepository;

        public GetUsers(IKeycloakRepository keycloakRepository)
        {
            _keycloakRepository = keycloakRepository;
        }

        public async Task<PaginatedListOutput<UserOutput>> Handle(GetUsersInput input, CancellationToken cancellationToken)
        {

            var users = await _keycloakRepository.FindUsersAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(input.Username))
                users = users.Where(x => x.Username == input.Username);

            if (!string.IsNullOrWhiteSpace(input.FirstName))
                users = users.Where(x => x.FirstName == input.FirstName);

            if (!string.IsNullOrWhiteSpace(input.LastName))
                users = users.Where(x => x.LastName == input.LastName);

            if (!string.IsNullOrWhiteSpace(input.Email))
                users = users.Where(x => x.Email == input.Email);

            if (input.Enabled.HasValue)
                users = users.Where(x => x.Enabled);

            if (!string.IsNullOrEmpty(input.CreatedBy))
                users = users.Where(x => x.Attributes["created_by"] == input.CreatedBy);

            if (input.CreatedAtStart.HasValue && input.CreatedAtEnd.HasValue)
                users = users.Where(x => DateTime.Parse(x.Attributes["created_at"]) >= input.CreatedAtStart && DateTime.Parse(x.Attributes["created_at"]) <= input.CreatedAtEnd);

            if (!string.IsNullOrEmpty(input.LastUpdatedBy))
                users = users.Where(x => x.Attributes["last_updated_by"] == input.LastUpdatedBy);

            if (input.LastUpdatedAtStart.HasValue && input.LastUpdatedAtEnd.HasValue)
                users = users.Where(x => DateTime.Parse(x.Attributes["last_updated_at"]) >= input.LastUpdatedAtStart && DateTime.Parse(x.Attributes["last_udpdated_at"]) <= input.LastUpdatedAtEnd);

            var sortedUsers = SortHelper.ApplySort(users, input.Sort);

            var usersOutput = UserOutput.FromUser(sortedUsers
                .Skip((input.PageNumber - 1) * input.PageSize)
                .Take(input.PageSize));

            return new PaginatedListOutput<UserOutput>
            (
                pageNumber: input.PageNumber,
                pageSize: input.PageSize,
                totalPages: PaginatedListOutput<UserOutput>.GetTotalPages(users.Count(), input.PageSize),
                totalItems: users.Count(),
                items: usersOutput
            );
        }
    }
}
