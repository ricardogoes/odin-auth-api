using MediatR;

namespace Odin.Auth.Application.ChangeStatusUser
{
    public class ChangeStatusUserInput : IRequest<ChangeStatusUserOutput>
    {
        public string Username { get; set; }
        public ChangeStatusAction? Action { get; set; }
    }

    public enum ChangeStatusAction
    {
        ACTIVATE, DEACTIVATE, INVALID
    }
}
