using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.Game.Admin.Repositories;
using System.IO;

namespace Marcidia.Game.Admin.Services
{
    class AdminUserService : IAdminUserService
    {
        IAdminUserRepository adminUserRepository;
        IStringHasher stringHasher;

        public AdminUserService(IStringHasher stringHasher, IAdminUserRepository adminUserRepository)
        {
            if (adminUserRepository == null)
                throw new ArgumentNullException("adminUserRepository", "adminUserRepository is null.");
            if (stringHasher == null)
                throw new ArgumentNullException("stringHasher", "stringHasher is null.");

            this.stringHasher = stringHasher;
            this.adminUserRepository = adminUserRepository;
        }

        public AdminUser CreateAdminUser(string username, string password)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentException("username is null or empty.", "username");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("password is null or empty.", "password");
            if (!IsValidUsername(username))
                throw new ArgumentException("Invalid username", "username");
            if (AdminUserExists(username))
                throw new ArgumentException("Username is already in use.", username);

            AdminUser newUser = new AdminUser(username, password);

            adminUserRepository.Save(newUser);

            return newUser;
        }

        public AdminUser ValidateLogin(string username, string password)
        {
            if (String.IsNullOrEmpty(username))
                throw new ArgumentException("username is null or empty.", "username");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("password is null or empty.", "password");

            if (!IsValidUsername(username))
                return null;

            if (!AdminUserExists(username))
                return null;

            AdminUser user = adminUserRepository.Find(username);

            string hashedPassword = stringHasher.Hash(password);

            if (user.PasswordHash != hashedPassword)
                return null;

            return user;
        }

        public bool IsValidUsername(string username)
        {
            for (int i = 0; i < username.Length; i++)
            {
                if (!Char.IsLetter(username[i]))
                    return false;
            }

            return true;
        }        

        private bool AdminUserExists(string username)
        {
            return adminUserRepository.Find(username) != null;
        }
    }
}
