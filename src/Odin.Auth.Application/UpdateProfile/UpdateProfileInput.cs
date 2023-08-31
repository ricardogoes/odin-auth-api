using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Application.UpdateProfile
{
    public class UpdateProfileInput : IRequest<UpdateProfileOutput>
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }
}
