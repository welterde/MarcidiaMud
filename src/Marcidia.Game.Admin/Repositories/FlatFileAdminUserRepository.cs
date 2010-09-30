using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Marcidia.Game.Admin.Serialization;

namespace Marcidia.Game.Admin.Repositories
{
    class FlatFileAdminUserRepository : IAdminUserRepository
    {
        string dataDirPath;
        Dictionary<string, AdminUser> cache;

        public FlatFileAdminUserRepository(string dataDirPath)
        {
            if (String.IsNullOrEmpty(dataDirPath))
                throw new ArgumentException("dataDir is null or empty.", "dataDir");

            this.dataDirPath = dataDirPath;

            cache = new Dictionary<string, AdminUser>();
        }

        public AdminUser Find(string username)
        {
            username = username.ToLower();            
            
            return CheckCacheFor(username) ?? ReadUserFromFile(username);
        }

        public void Save(AdminUser user)
        {
            WriteUserToFile(user);

            AddToCache(user);
        }

        private void AddToCache(AdminUser user)
        {
            lock (cache)
            {
                cache[user.Name.ToLower()] = user;
            }
        }

        private AdminUser CheckCacheFor(string username)
        {        
            lock (cache)
            {
                if (cache.ContainsKey(username))
                {                    
                    return cache[username];
                }
            }

            return null;
        }

        private void WriteUserToFile(AdminUser user)
        {
            string path = UsernameToFilename(user.Name.ToLower());

            using (AdminUserWriter writer = new AdminUserWriter(path))
            {
                writer.Write(user);
            }
        }

        private AdminUser ReadUserFromFile(string username)
        {
            string path = UsernameToFilename(username);

            if (File.Exists(path))
            {
                using (AdminUserReader userReader = new AdminUserReader(path))
                {
                    return userReader.Read();
                }
            }

            return null;
        }

        private string UsernameToFilename(string username)
        {
            return Path.Combine(dataDirPath, string.Format("adminuser-{0}.xml", username.ToLower()));
        }

    }
}
