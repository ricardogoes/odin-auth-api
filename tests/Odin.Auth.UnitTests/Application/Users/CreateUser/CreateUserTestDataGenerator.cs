namespace Odin.Auth.UnitTests.Application.Users.CreateUser
{
    public class CreateUserTestDataGenerator
    {


        public static IEnumerable<object[]> GetInvalidInputs(int times = 15)
        {
            var fixture = new CreateUserTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 5;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyUsername(),
                        "Username should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyPassword(),
                        "Value should not be empty or null"
                    });
                        break;
                    case 2:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyFirstName(),
                        "FirstName should not be empty or null"
                    });
                        break;
                    case 3:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyLastName(),
                        "LastName should not be empty or null"
                    });
                        break;
                    case 4:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyEmail(),
                        "Email should be a valid email"
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
