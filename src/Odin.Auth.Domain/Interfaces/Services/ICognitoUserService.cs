using Odin.Auth.Domain.Models.ChangeStatusUser;
using Odin.Auth.Domain.Models.UpdateProfile;
using Odin.Auth.Domain.Models.UserLogin;
using Odin.Auth.Domain.Models.UserProfile;

namespace Odin.Auth.Domain.Interfaces.Services
{
    public interface ICognitoUserService
    {
        Task<InsertUserResponse> InsertUserAsync(InsertUserRequest request);
        Task<UpdateProfileResponse> UpdateUserAttributesAsync(UpdateProfileRequest request);
        Task<UpdateProfileResponse> EnableUserAsync(string username);
        Task<UpdateProfileResponse> DisableUserAsync(string username);
        Task<UserProfileResponse> GetUserByUsernameAsync(string username);
    }
}
