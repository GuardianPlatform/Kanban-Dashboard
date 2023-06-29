using Kanban.Dashboard.Core.Dtos;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Kanban.Dashboard.Core.Entities
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public List<Board> Boards { get; set; }
        [IgnoreDataMember]
        public List<BoardUsers> BoardsUsers { get; set; }


        public bool OwnsToken(string token)
        {
            return RefreshTokens?.Find(x => x.Token == token) != null;
        }

    }
}
