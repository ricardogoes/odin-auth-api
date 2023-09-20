using Odin.Auth.Application.Login;

namespace Odin.Auth.EndToEndTests.Controllers.Users
{
    public class UserBaseFixture : BaseFixture
    {
        public UserBaseFixture()
            : base()
        {
        }

        public LoginInput GetValidLoginInput()
        {
            return new LoginInput("admin", "Odin@123!");
        }
    }
}
