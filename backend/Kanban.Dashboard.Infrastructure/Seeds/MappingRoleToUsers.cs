using Kanban.Dashboard.Core.Settings;
using Microsoft.AspNetCore.Identity;

namespace Kanban.Dashboard.Infrastructure.Seeds
{
    public static class MappingRoleToUsers
    {
        public static List<IdentityUserRole<string>> IdentityUserRoleList()
        {
            return new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Roles.Basic,
                    UserId = Constants.Users.Basic
                },
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Roles.Admin,
                    UserId = Constants.Users.SuperAdmin
                },
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Roles.Moderator,
                    UserId = Constants.Users.SuperAdmin
                },
                new IdentityUserRole<string>
                {
                    RoleId = Constants.Roles.Basic,
                    UserId = Constants.Users.SuperAdmin
                }
            };
        }
    }
}
