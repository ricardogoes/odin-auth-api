namespace Odin.Auth.UnitTests.Application.Auth.Login
{
    public class LoginTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs(int times = 8)
        {
            var fixture = new LoginTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 4;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyUsername()
                    });
                        break;
                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInputWithEmptyPassword()
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
