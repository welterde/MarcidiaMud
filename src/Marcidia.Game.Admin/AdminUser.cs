using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Game.Admin
{
    public class AdminUser
    {
        public string Name { get; private set; }        
        public string PasswordHash { get; private set; }

        public AdminUser(string name, string passwordHash)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name is null or empty.", "name");
            if (String.IsNullOrEmpty(passwordHash))
                throw new ArgumentException("passwordHash is null or empty.", "passwordHash");

            Name = name;
            PasswordHash = passwordHash;
        }
    }
}
