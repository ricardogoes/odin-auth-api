using Odin.Auth.Application.Login;

namespace Odin.Auth.EndToEndTests.Controllers.Auth
{
    public class AuthBaseFixture : BaseFixture
    {
        public AuthBaseFixture()
            : base()
        {
        }

        public LoginInput GetValidLoginInput()
        {
            return new LoginInput("admin", "Odin@123!");
        }
    }
}
