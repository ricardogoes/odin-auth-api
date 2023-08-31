using MediatR;

namespace Odin.Auth.Application.AddUser
{
    public class AddUserInput : IRequest<AddUserOutput>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
