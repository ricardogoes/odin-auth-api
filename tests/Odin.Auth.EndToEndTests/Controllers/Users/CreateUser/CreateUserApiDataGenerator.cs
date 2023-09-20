namespace Odin.Auth.EndToEndTests.Controllers.Users.CreateUser
{
    public class CreateUserApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreateUserApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 12;

            for (int index = 0; index < totalInvalidCases; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        var input1 = fixture.GetInputWithUsernameEmpty();
                        invalidInputsList.Add(new object[] {
                        input1,
                        "Username",
                        "'Username' must not be empty."
                    });
                        break;
                    case 1:
                        var input2 = fixture.GetInputWithPasswordEmpty();
                        invalidInputsList.Add(new object[] {
                        input2,
                        "Password",
                        "'Password' must not be empty."
                    });
                        break;
                    case 2:
                        var input3 = fixture.GetInputWithFirstNameEmpty();
                        invalidInputsList.Add(new object[] {
                        input3,
                        "FirstName",
                        "'First Name' must not be empty."
                    });
                        break;
                    case 3:
                        var input4 = fixture.GetInputWithLastNameEmpty();
                        invalidInputsList.Add(new object[] {
                        input4,
                        "LastName",
                        "'Last Name' must not be empty."
                    });
                        break;
                    case 4:
                        var input5 = fixture.GetInputWithEmailEmpty();
                        invalidInputsList.Add(new object[] {
                        input5,
                        "Email",
                         "'Email' is not a valid email address."
                    });
                        break;
                    case 5:
                        var input6 = fixture.GetInputWithGroupsEmpty();
                        invalidInputsList.Add(new object[] {
                        input6,
                        "Groups",
                        "'Groups' must not be empty."
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
