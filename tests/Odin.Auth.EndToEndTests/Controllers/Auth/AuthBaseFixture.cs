using Odin.Auth.Application.Auth.Login;

namespace Odin.Auth.EndToEndTests.Controllers.Auth
{
    public class AuthBaseFixture : BaseFixture
    {
        public AuthBaseFixture()
            : base()
        {
        }

        public LoginInput GetValidLoginRequest()
        {
            return new LoginInput("admin", "Odin@123!");
        }
    }
}
