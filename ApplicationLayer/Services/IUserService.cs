using ApplicationLayer.Abstractions;
using ApplicationLayer.Contracts.Users;

namespace ApplicationLayer.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetPorfileAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);


}
