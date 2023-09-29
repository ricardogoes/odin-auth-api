using Odin.Auth.Api.Models.Auth;

namespace Odin.Auth.EndToEndTests.Controllers.Auth
{
    public class AuthBaseFixture : BaseFixture
    {
        public AuthBaseFixture()
            : base()
        {
        }

        public LoginRequest GetValidLoginRequest()
        {
            return new LoginRequest("admin", "Odin@123!");
        }
    }
}
