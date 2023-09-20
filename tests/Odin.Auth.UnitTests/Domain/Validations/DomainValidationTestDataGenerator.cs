using Bogus;

namespace Odin.Auth.UnitTests.Domain.Validations
{
    public class DomainValidationTestDataGenerator
    {
        public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOftests = 5)
        {
            yield return new object[] { "123456", 5 };
            var faker = new Faker();
            for (int i = 0; i < numberOftests - 1; i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length - new Random().Next(1, 5);
                yield return new object[] { example, maxLength };
            }
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOftests = 5)
        {
            yield return new object[] { "123456", 6 };
            var faker = new Faker();
            for (int i = 0; i < numberOftests - 1; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length - new Random().Next(1, 5);
                yield return new object[] { example, minLength };
            }
        }
        public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOftests = 5)
        {
            yield return new object[] { "123456", 10 };
            var faker = new Faker();
            for (int i = 0; i < numberOftests - 1; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + new Random().Next(1, 20);
                yield return new object[] { example, minLength };
            }
        }

        public static IEnumerable<object[]> GetValuesLessThanMax(int numberOftests = 5)
        {
            yield return new object[] { "123456", 6 };
            var faker = new Faker();
            for (int i = 0; i < numberOftests - 1; i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length + new Random().Next(0, 5);
                yield return new object[] { example, maxLength };
            }
        }
    }
}
