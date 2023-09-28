namespace Odin.Auth.EndToEndTests.Controllers.Customers.ChangeAddressCustomer
{
    public class ChangeAddressCustomerApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new ChangeAddressCustomerApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 12;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        var input1 = fixture.GetAddressInputWithoutStreetName();
                        invalidInputsList.Add(new object[] {
                        input1,
                        "StreetName",
                        "'Street Name' must not be empty."
                    });
                        break;
                    case 1:
                        var input2 = fixture.GetAddressInputWithoutStreetNumber();
                        invalidInputsList.Add(new object[] {
                        input2,
                        "StreetNumber",
                        "'Street Number' must be greater than '0'."
                    });
                        break;
                    case 2:
                        var input3 = fixture.GetAddressInputWithoutNeighborhood();
                        invalidInputsList.Add(new object[] {
                        input3,
                        "Neighborhood",
                        "'Neighborhood' must not be empty."
                    });
                        break;
                    case 3:
                        var input4 = fixture.GetAddressInputWithoutZipCode();
                        invalidInputsList.Add(new object[] {
                        input4,
                        "ZipCode",
                        "'Zip Code' must not be empty."
                    });
                        break;
                    case 4:
                        var input5 = fixture.GetAddressInputWithoutCity();
                        invalidInputsList.Add(new object[] {
                        input5,
                        "City",
                        "'City' must not be empty."
                    });
                        break;
                    case 5:
                        var input6 = fixture.GetAddressInputWithoutState();
                        invalidInputsList.Add(new object[] {
                        input6,
                        "State",
                        "'State' must not be empty."
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
