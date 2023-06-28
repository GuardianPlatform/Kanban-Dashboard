using System;

namespace Kanban.Dashboard.Core.Settings
{
    public static class Constants
    {
        public static class Roles
        {
            public static readonly string SuperAdmin = Guid.NewGuid().ToString();
            public static readonly string Admin = Guid.NewGuid().ToString();
            public static readonly string Moderator = Guid.NewGuid().ToString();
            public static readonly string Basic = Guid.NewGuid().ToString();
        }

        public static class Users
        {
            public static readonly string SuperAdmin = Guid.NewGuid().ToString();
            public static readonly string Basic = Guid.NewGuid().ToString();
        }
    }
}
