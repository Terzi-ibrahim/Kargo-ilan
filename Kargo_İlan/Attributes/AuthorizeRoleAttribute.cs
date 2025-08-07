using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Kargo_İlan.Data;


namespace Kargo_İlan.Attributes
{
    public class AuthorizeRoleAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly int[] _roleIds;

        public AuthorizeRoleAttribute(params int[] roleIds)
        {
            _roleIds = roleIds;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userEmail = context.HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var _context = context.HttpContext.RequestServices.GetService<KargoDbContext>();

            var commonUser = await _context.Person
                .FirstOrDefaultAsync(c => c.Email == userEmail);

            if (commonUser == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Person_id == commonUser.Person_id);

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var userRoleIds = await _context.UserRole
                  .Where(ur => ur.User_id == user.User_id)
                  .Select(ur => ur.Role_id)                  
                  .ToListAsync();

            if (!_roleIds.Any(roleId => userRoleIds.Contains(roleId)))
            {
                context.Result = new ForbidResult();
            }
        }

    }
}
