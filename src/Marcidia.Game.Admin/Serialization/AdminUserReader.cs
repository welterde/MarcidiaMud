using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace Marcidia.Game.Admin.Serialization
{
    class AdminUserReader : IDisposable
    {
        StreamReader reader;

        public AdminUserReader(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("path is null or empty.", "path");

            reader = new StreamReader(path);
        }

        public AdminUser Read()
        {
            XDocument document = XDocument.Load(reader);
            
            string name = ReadValueFromExpectedElement(document.Root, "Name");
            string passwordHash = ReadValueFromExpectedElement(document.Root, "PasswordHash");

            return new AdminUser(name, passwordHash);
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        private string ReadValueFromExpectedElement(XElement root, XName elementName)
        {
            XElement element = root.Descendants(elementName).FirstOrDefault();

            if (element == null)
                throw new IOException(
                    string.Format("Admin user is missing {0} element", elementName));

            return element.Value;
        }
    }
}
