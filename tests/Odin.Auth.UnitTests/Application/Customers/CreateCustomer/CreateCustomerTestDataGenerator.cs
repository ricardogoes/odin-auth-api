namespace Odin.Auth.UnitTests.Application.Customers.CreateCustomer
{
    public class CreateCustomerTestDataGenerator
    {


        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new CreateCustomerTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateCustomerInputWithEmptyName(),
                        "Name should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateCustomerInputWithEmptyDocument(),
                        "Document should be a valid CPF or CNPJ"
                    });
                        break;
                    case 2:
                        invalidInputsList.Add(new object[] {
                        fixture.GetCreateCustomerInputWithInvalidDocument(),
                        "Document should be a valid CPF or CNPJ"
                    });
                        break;
                    default:
                        break;
                }
            }

            return invalidInputsList;
        }
    }
}
