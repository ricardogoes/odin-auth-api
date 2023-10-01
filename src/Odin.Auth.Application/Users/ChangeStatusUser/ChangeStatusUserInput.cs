using MediatR;
using Odin.Auth.Domain.Enums;

namespace Odin.Auth.Application.Users.ChangeStatusUser
{
    public class ChangeStatusUserInput : IRequest<UserOutput>
    {
        public Guid UserId { get; private set; }
        public ChangeStatusAction? Action { get; private set; }

        public ChangeStatusUserInput(Guid id, ChangeStatusAction? action)
        {
            UserId = id;
            Action = action;
        }
    }
}
