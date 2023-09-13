using MediatR;

namespace Odin.Auth.Application.ChangeStatusUser
{
    public class ChangeStatusUserInput : IRequest<ChangeStatusUserOutput>
    {
        public string Username { get; private set; }
        public ChangeStatusAction? Action { get; private set; }

        public ChangeStatusUserInput(string username, ChangeStatusAction? action)
        {
            Username = username;
            Action = action;
        }
    }

    public enum ChangeStatusAction
    {
        ACTIVATE, DEACTIVATE, INVALID
    }
}
