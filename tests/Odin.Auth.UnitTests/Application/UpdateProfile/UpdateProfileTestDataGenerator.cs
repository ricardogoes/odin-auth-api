namespace Odin.Auth.UnitTests.Application.UpdateProfile
{
    public class UpdateProfileTestDataGenerator
    {


        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new UpdateProfileTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyFirstName(),
                        "First Name should not be empty or null"
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyLastName(),
                        "Last Name should not be empty or null"
                    });
                        break;
                    case 2:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyEmail(),
                        "Email should not be empty or null"
                    });
                        break;
                    case 3:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyRole(),
                        "Roles should not be empty or null"
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
