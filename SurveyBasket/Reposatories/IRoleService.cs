
namespace SurveyBasket.Reposatories;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllRoles(bool? includeDisabled = false,CancellationToken cancellationToken = default);
    Task<Result<RoleResponseDetails>> GetById(string id);
}
