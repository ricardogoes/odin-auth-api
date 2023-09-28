namespace Odin.Auth.EndToEndTests.Controllers.Customers.UpdateCustomer
{
    public class UpdateCustomerApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new UpdateCustomerApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 3;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        var input1 = fixture.GetInputWithNameEmpty(Guid.NewGuid());
                        invalidInputsList.Add(new object[] {
                        input1,
                        "Name",
                        "'Name' must not be empty."
                    });
                        break;
                    case 1:
                        var input2 = fixture.GetInputWithDocumentEmpty(Guid.NewGuid());
                        invalidInputsList.Add(new object[] {
                        input2,
                        "Document",
                        "'Document' must be a valid CPF or CNPJ"
                    });
                        break;
                    case 2:
                        var input3 = fixture.GetInputWithInvalidDocument(Guid.NewGuid());
                        invalidInputsList.Add(new object[] {
                        input3,
                        "Document",
                        "'Document' must be a valid CPF or CNPJ"
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
