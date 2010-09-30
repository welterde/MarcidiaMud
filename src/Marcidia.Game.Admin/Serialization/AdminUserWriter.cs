using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace Marcidia.Game.Admin.Serialization
{
    class AdminUserWriter : IDisposable
    {
        private StreamWriter streamWriter;

        public AdminUserWriter(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("path is null or empty.", "path");

            streamWriter = new StreamWriter(path);
        }

        public void Write(AdminUser user)
        {
            XDocument xDoc = new XDocument(
                new XElement("AdminUser",
                    new XElement("Name", user.Name),
                    new XElement("PasswordHash", user.PasswordHash)));

            xDoc.Save(streamWriter);
        }

        public void Dispose()
        {
            streamWriter.Dispose();
        }
    }
}
