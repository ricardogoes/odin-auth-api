using FluentAssertions;
using Odin.Auth.Domain.Exceptions;
using ValueObject = Odin.Auth.Domain.ValueObjects;

namespace Odin.Auth.UnitTests.Domain.ValuesObjects.Address
{
    [Collection(nameof(AddressTestFixtureCollection))]
    public class AddressTest
    {
        private readonly AddressTestFixture _fixture;

        public AddressTest(AddressTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = "ctor() should instantiate a new address")]
        [Trait("Domain", "Value Objects / Address")]
        public void Instantiate()
        {
            var validAddress = _fixture.GetValidAddress();
            var address = new ValueObject.Address(validAddress.StreetName!, validAddress.StreetNumber, validAddress.Complement, validAddress.Neighborhood!, validAddress.ZipCode!, validAddress.City!, validAddress.State!);

            address.Should().NotBeNull();
            address.StreetName.Should().Be(validAddress.StreetName);
            address.StreetNumber.Should().Be(validAddress.StreetNumber);
            address.Complement.Should().Be(validAddress.Complement);
            address.Neighborhood.Should().Be(validAddress.Neighborhood);
            address.ZipCode.Should().Be(validAddress.ZipCode);
            address.City.Should().Be(validAddress.City);
            address.State.Should().Be(validAddress.State);
        }

        [Theory(DisplayName = "ctor() should throw an error when street name is invalid")]
        [Trait("Domain", "Value Objects / Address")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenStreetNameIsEmpty(string? name)
        {
            var validAddress = _fixture.GetValidAddress();

            Action action = () => new ValueObject.Address(name!, validAddress.StreetNumber, validAddress.Complement, validAddress.Neighborhood!, validAddress.ZipCode!, validAddress.City!, validAddress.State!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("StreetName should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when street number is invalid")]
        [Trait("Domain", "Value Objects / Address")]
        [InlineData(-1)]
        public void InstantiateErrorWhenStreetNumberIsEmpty(int number)
        {
            var validAddress = _fixture.GetValidAddress();

            Action action = () => new ValueObject.Address(validAddress.StreetName!, number, validAddress.Complement, validAddress.Neighborhood!, validAddress.ZipCode!, validAddress.City!, validAddress.State!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("StreetNumber should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when neighborhood is empty")]
        [Trait("Domain", "Value Objects / Address")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenNeighborhoodIsEmpty(string? name)
        {
            var validAddress = _fixture.GetValidAddress();

            Action action = () => new ValueObject.Address(validAddress.StreetName!, validAddress.StreetNumber, validAddress.Complement, name!, validAddress.ZipCode!, validAddress.City!, validAddress.State!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Neighborhood should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when zip code is empty")]
        [Trait("Domain", "Value Objects / Address")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenZipCodeIsEmpty(string? name)
        {
            var validAddress = _fixture.GetValidAddress();

            Action action = () => new ValueObject.Address(validAddress.StreetName!, validAddress.StreetNumber, validAddress.Complement, validAddress.Neighborhood!, name!, validAddress.City!, validAddress.State!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("ZipCode should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when city is empty")]
        [Trait("Domain", "Value Objects / Address")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenCityIsEmpty(string? name)
        {
            var validAddress = _fixture.GetValidAddress();

            Action action = () => new ValueObject.Address(validAddress.StreetName!, validAddress.StreetNumber, validAddress.Complement, validAddress.Neighborhood!, validAddress.ZipCode!, name!, validAddress.State!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("City should not be empty or null");
        }

        [Theory(DisplayName = "ctor() should throw an error when state is empty")]
        [Trait("Domain", "Value Objects / Address")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void InstantiateErrorWhenStateIsEmpty(string? name)
        {
            var validAddress = _fixture.GetValidAddress();

            Action action = () => new ValueObject.Address(validAddress.StreetName!, validAddress.StreetNumber, validAddress.Complement, validAddress.Neighborhood!, validAddress.ZipCode!, validAddress.City!, name!);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("State should not be empty or null");
        }
    }
}
