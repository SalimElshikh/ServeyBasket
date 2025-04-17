using SurveyBasket.Contracts.Users;

namespace SurveyBasket.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetPorfileAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);


}
