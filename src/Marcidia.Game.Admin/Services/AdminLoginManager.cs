using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Sessions;

namespace Marcidia.Game.Admin.Services
{
    public class AdminLoginManager : IAdminLoginManager
    {
        Dictionary<AdminUser, Session> adminUsers;

        public AdminLoginManager()
        {
            adminUsers = new Dictionary<AdminUser, Session>();
        }

        public void PerformLogin(AdminUser adminUser, Session session)
        {
            if (UserIsLoggedIn(adminUser))
            {
                Session usersCurrentSession = GetUsersSession(adminUser);

                if (usersCurrentSession != session)
                {
                    session.Output.WriteLine("`cTaking control of your previous session");
                    usersCurrentSession.Output.WriteLine(
                        "`RYou have been logged in from another location, and have been disconnected.\r\n" + 
                        "If this wasn't you, please contact an admin immediately from your registered\r\n" + 
                        "email address.");
                    
                    usersCurrentSession.TakeControlOf(session);

                    usersCurrentSession.Output.WriteLine("`WYou have successfully taken control of your previous session.");
                }
            }
            else
                AddUserToLoggedInList(adminUser, session);
        }

        public Session GetUsersSession(AdminUser adminUser)
        {
            lock (adminUsers)
            {
                if (!adminUsers.ContainsKey(adminUser))
                    return null;

                return adminUsers[adminUser];
            }
        }

        private void AddUserToLoggedInList(AdminUser adminUser, Session session)
        {
            lock (adminUsers)
            {
                adminUsers.Add(adminUser, session);
                session.SessionClosed += OnSessionClosed;
            }
        }

        private void RemoveUserFromLoggedInList(AdminUser adminUser)
        {
            lock (adminUsers)
            {
                Session session = GetUsersSession(adminUser);
                session.SessionClosed -= OnSessionClosed;
                adminUsers.Remove(adminUser);
            }
        }

        private bool UserIsLoggedIn(AdminUser adminUser)
        {
            lock (adminUsers)
            {
                return adminUsers.Any(entry => entry.Key == adminUser);
            }
        }

        private AdminUser GetOwnerOfSession(Session session)
        {
            lock (adminUsers)
            {
                return adminUsers.Where(entry => entry.Value == session)
                                 .Select(entry => entry.Key)
                                 .FirstOrDefault();
            }
        }

        private void OnSessionClosed(object sender, EventArgs e)
        {
            Session session = (Session)sender;

            AdminUser adminUser = GetOwnerOfSession(session);

            if (adminUser != null)
                RemoveUserFromLoggedInList(adminUser);
        }
    }
}
