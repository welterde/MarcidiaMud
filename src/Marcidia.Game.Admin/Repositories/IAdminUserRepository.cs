using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.Game.Admin.Repositories
{
    public interface IAdminUserRepository
    {
        AdminUser Find(string username);
        void Save(AdminUser user);
    }
}
