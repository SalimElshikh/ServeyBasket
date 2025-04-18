﻿

using Org.BouncyCastle.Security;

namespace SurveyBasket.Authentication.Filters;

public class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    private readonly AuthorizationOptions _authorizationOptions = options.Value;
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        
        var policy = await  base.GetPolicyAsync(policyName);
        if(policy is not null )
            return policy;
        var permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new Permissionrequirment(policyName))
            .Build();
        _authorizationOptions.AddPolicy(policyName, permissionPolicy);
        return permissionPolicy;

    }
}
