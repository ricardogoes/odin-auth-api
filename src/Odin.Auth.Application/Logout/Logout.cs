using Amazon.CognitoIdentityProvider.Model;
using MediatR;
using Odin.Auth.Infra.Cognito;

namespace Odin.Auth.Application.Logout
{
    public class Logout : IRequestHandler<LogoutInput, LogoutOutput>
    {
        private readonly IAmazonCognitoIdentityRepository _awsIdentityRepository;

        public Logout(IAmazonCognitoIdentityRepository awsIdentityRepository)
        {
            _awsIdentityRepository = awsIdentityRepository;
        }

        public async Task<LogoutOutput> Handle(LogoutInput input, CancellationToken cancellationToken)
        {
            var request = new GlobalSignOutRequest { AccessToken = input.AccessToken };
            await _awsIdentityRepository.GlobalSignOutAsync(request);

            return new LogoutOutput(input.Username, $"User '{input.Username}' logged out successfully");
        }
    }
}
