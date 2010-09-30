using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Game.Admin.Repositories;

namespace Marcidia.Game.Admin.Services
{
    interface IAdminUserService
    {
        AdminUser CreateAdminUser(string username, string password);
        AdminUser Login(string username, string password);
        bool ValidUsername(string username);
    }
}
