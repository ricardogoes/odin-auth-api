using FluentAssertions;
using Odin.Auth.Application.Helpers;
using Odin.Auth.Domain.Entities;

namespace Odin.Auth.UnitTests.Application.Helpers
{
    [Collection(nameof(SortHelperTestFixtureCollection))]
    public class SortHelperModelMapperTest
    {
        private readonly SortHelperTestFixture _fixture;

        public SortHelperModelMapperTest(SortHelperTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ApplySort() should order list by username (string)")]
        [Trait("Application", "Helpers / SortHelper")]
        public void SortListByName()
        {
            var users = new List<User>
            {
                new User("user.01", "User", "01", "user.01@email.com"),
                new User("user.06", "User", "06", "user.06@email.com"),
                new User("user.05", "User", "05", "user.05@email.com"),
                new User("user.07", "User", "07", "user.07@email.com"),
                new User("user.03", "User", "03", "user.03@email.com"),
                new User("user.02", "User", "02", "user.02@email.com"),
                new User("user.04", "User", "04", "user.04@email.com")
            };

            var orderedList = SortHelper.ApplySort(users, "username").ToList();

            orderedList.Should().NotBeNull();
            orderedList.Should().HaveCount(7);

            orderedList[0].Username.Should().Be("user.01");
            orderedList[1].Username.Should().Be("user.02");
            orderedList[2].Username.Should().Be("user.03");
            orderedList[3].Username.Should().Be("user.04");
            orderedList[4].Username.Should().Be("user.05");
            orderedList[5].Username.Should().Be("user.06");
            orderedList[6].Username.Should().Be("user.07");
        }

        [Fact(DisplayName = "ApplySort() should order list by username (string) desc")]
        [Trait("Application", "Helpers / SortHelper")]
        public void SortListByNameDesc()
        {
            var users = new List<User>
            {
                new User("user.01", "User", "01", "user.01@email.com"),
                new User("user.06", "User", "06", "user.06@email.com"),
                new User("user.05", "User", "05", "user.05@email.com"),
                new User("user.07", "User", "07", "user.07@email.com"),
                new User("user.03", "User", "03", "user.03@email.com"),
                new User("user.02", "User", "02", "user.02@email.com"),
                new User("user.04", "User", "04", "user.04@email.com")
            };

            var orderedList = SortHelper.ApplySort(users, "username desc").ToList();

            orderedList.Should().NotBeNull();
            orderedList.Should().HaveCount(7);

            orderedList[0].Username.Should().Be("user.07");
            orderedList[1].Username.Should().Be("user.06");
            orderedList[2].Username.Should().Be("user.05");
            orderedList[3].Username.Should().Be("user.04");
            orderedList[4].Username.Should().Be("user.03");
            orderedList[5].Username.Should().Be("user.02");
            orderedList[6].Username.Should().Be("user.01");
        }
    }
}
