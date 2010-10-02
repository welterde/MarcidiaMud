using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Sessions;

namespace Marcidia.Game.Admin.Services
{
    public interface IAdminLoginManager
    {
        void PerformLogin(AdminUser adminUser, Session session);
        Session GetUsersSession(AdminUser adminUser);
    }
}
