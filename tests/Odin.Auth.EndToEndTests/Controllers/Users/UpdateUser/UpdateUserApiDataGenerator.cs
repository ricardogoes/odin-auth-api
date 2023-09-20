namespace Odin.Auth.EndToEndTests.Controllers.Users.UpdateUser
{
    public class UpdateUserApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new UpdateUserApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 12;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        var input2 = fixture.GetInputWithFirstNameEmpty(Guid.NewGuid());
                        invalidInputsList.Add(new object[] {
                        input2,
                        "FirstName",
                        "'First Name' must not be empty."
                    });
                        break;
                    case 1:
                        var input3 = fixture.GetInputWithLastNameEmpty(Guid.NewGuid());
                        invalidInputsList.Add(new object[] {
                        input3,
                        "LastName",
                        "'Last Name' must not be empty."
                    });
                        break;
                    case 2:
                        var input4 = fixture.GetInputWithEmailEmpty(Guid.NewGuid());
                        invalidInputsList.Add(new object[] {
                        input4,
                        "Email",
                        "'Email' is not a valid email address."
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
