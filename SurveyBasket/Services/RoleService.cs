using Azure.Core;
using Microsoft.Identity.Client;
using SurveyBasket.Contracts.Roles;
using SurveyBasket.Errors;

namespace SurveyBasket.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    public async Task<IEnumerable<RoleResponse>> GetAllRoles(bool? includeDisabled = false, CancellationToken cancellationToken = default) =>
        await _roleManager.Roles
            .Where(x => !x.IsDefault && (!x.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);

    public async Task<Result<RoleResponseDetails>> GetById(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if(role is null )
            return Result.Failure<RoleResponseDetails>(RolesErrors.RoleNotFound);
        var permission = await _roleManager.GetClaimsAsync(role);
        var response = new RoleResponseDetails(role.Id, role.Name!, role.IsDeleted, permission.Select(x => x.Value));

        return Result.Success(response );
    }






        


}
