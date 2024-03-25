using System;
using POC.Artifacts.Domain.Models;
using POC.Artifacts.SQL.Repositories.Builders;
using POC.Artifacts.Helpers;

namespace POC.Domain.Models.Entities
{
    [UseAutoMapBuilder("userinfo", false)]
    public class UserInfo : Entity<Int32>
    {
        public Guid UniqueId { get; set; }
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public bool IsAdmin { get; set; } = false;

        public UserInfo() { }

        public UserInfo(string username, string password, bool isAdmin)
        {
            this.Username = username.OnlyNumbers();
            this.Password = password.Encrypt();
            this.IsAdmin = isAdmin;
            this.UniqueId = Guid.NewGuid();
        }

    }
}

