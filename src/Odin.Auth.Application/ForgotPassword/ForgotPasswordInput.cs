using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Odin.Auth.Application.ForgotPassword
{
    public class ForgotPasswordInput : IRequest<ForgotPasswordOutput>
    {
        public string Username { get; set; }
    }
}
