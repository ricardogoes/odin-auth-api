namespace Odin.Auth.UnitTests.Application.Customers.UpdateCustomer
{
    public class UpdateCustomerTestDataGenerator
    {

        public static IEnumerable<object[]> GetCustomersToUpdate(int times = 10)
        {
            var fixture = new UpdateCustomerTestFixture();
            for (int indice = 0; indice < times; indice++)
            {
                var validCustomer = fixture.GetValidCustomer();
                var validInpur = fixture.GetValidUpdateCustomerInput(validCustomer.Id);
                yield return new object[] {
                validCustomer, validInpur
            };
            }
        }

        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new UpdateCustomerTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateCustomerInputWithEmptyName(),
                        "Name should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateCustomerInputWithEmptyDocument(),
                        "Document should be a valid CPF or CNPJ"
                    });
                        break;
                    case 2:
                        invalidInputsList.Add(new object[] {
                        fixture.GetUpdateCustomerInputWithInvalidDocument(),
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
