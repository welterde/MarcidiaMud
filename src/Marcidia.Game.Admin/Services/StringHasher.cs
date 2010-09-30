using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Marcidia.Game.Admin.Services
{
    class StringHasher : IStringHasher
    {
        string salt;
        string pepper;

        public StringHasher(string salt, string pepper)
        {
            if (String.IsNullOrEmpty(salt))
                throw new ArgumentException("salt is null or empty.", "salt");
            if (String.IsNullOrEmpty(pepper))
                throw new ArgumentException("pepper is null or empty.", "pepper");

            this.salt = salt;
            this.pepper = pepper;
        }

        public string Hash(string value)
        {
            string strToHash = salt + value + pepper;

            MD5 md5 = MD5.Create();

            byte[] bytesToHash = Encoding.ASCII.GetBytes(strToHash);

            byte[] hashedBytes = md5.ComputeHash(bytesToHash);

            StringBuilder hashedString = new StringBuilder();

            foreach (var item in hashedBytes)
            {
                hashedString.Append(item.ToString("X2"));
            }

            return hashedString.ToString().ToLower();
        }
    }
}
