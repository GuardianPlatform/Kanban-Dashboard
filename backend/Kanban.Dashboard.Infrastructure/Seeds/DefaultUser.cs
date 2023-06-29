using Kanban.Dashboard.Core.Entities;
using Kanban.Dashboard.Core.Settings;

namespace Kanban.Dashboard.Infrastructure.Seeds
{
    public static class DefaultUser
    {
        public static List<User> IdentityBasicUserList()
        {
            return new List<User>()
            {
                new User()
                {
                    Id = Constants.Users.SuperAdmin,
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FirstName = "Super",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "admin@gmail.com",
                    NormalizedUserName="admin",
                    Login = "Admin"
                },
                new User()
                {
                    Id = Constants.Users.Basic,
                    UserName = "basicuser",
                    Email = "basicuser@gmail.com",
                    FirstName = "Basic",
                    LastName = "User",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "basicuser@GMAIL.COM",
                    NormalizedUserName="basicuser",
                    Login = "BasicUser",
                },
            };
        }
    }
}
