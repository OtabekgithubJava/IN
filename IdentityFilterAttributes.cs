using JobBoard.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace JobBoard.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Enum)]
    public class IdentityFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int _permissionId;

        public IdentityFilterAttribute(Permission permissionId)
        {
            _permissionId = (int)permissionId;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;

            var permissionIdsClaim = identity?.FindFirst("Permissions");

            if (permissionIdsClaim == null || string.IsNullOrEmpty(permissionIdsClaim.Value))
            {
                context.Result = new ForbidResult();
                return;
            }

            var permissionIds = JsonSerializer.Deserialize<List<int>>(permissionIdsClaim.Value);

            var result = permissionIds?.Any(x => _permissionId == x) ?? false;

            if (!result)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}