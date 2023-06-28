using Kanban.Dashboard.Core.Enums;
using Kanban.Dashboard.Core.Settings;
using Microsoft.AspNetCore.Identity;

namespace Kanban.Dashboard.Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        public static List<IdentityRole> IdentityRoleList()
        {
            return new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = Constants.Roles.Admin,
                    Name = Roles.Admin.ToString(),
                    NormalizedName = Roles.Admin.ToString()
                },
                new IdentityRole
                {
                    Id = Constants.Roles.Moderator,
                    Name = Roles.Moderator.ToString(),
                    NormalizedName = Roles.Moderator.ToString()
                },
                new IdentityRole
                {
                    Id = Constants.Roles.Basic,
                    Name = Roles.Basic.ToString(),
                    NormalizedName = Roles.Basic.ToString()
                }
            };
        }
    }
}
