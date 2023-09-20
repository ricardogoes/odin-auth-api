using MediatR;
using Odin.Auth.Domain.Models;

namespace Odin.Auth.Application.ChangeStatusUser
{
    public class ChangeStatusUserInput : IRequest<UserOutput>
    {
        public Guid UserId { get; private set; }
        public ChangeStatusAction? Action { get; private set; }
        public string LoggedUsername { get; private set; }

        public ChangeStatusUserInput(Guid id, ChangeStatusAction? action, string loggedUsername)
        {
            UserId = id;
            Action = action;
            LoggedUsername = loggedUsername;
        }
    }

    public enum ChangeStatusAction
    {
        ACTIVATE, DEACTIVATE, INVALID
    }
}
