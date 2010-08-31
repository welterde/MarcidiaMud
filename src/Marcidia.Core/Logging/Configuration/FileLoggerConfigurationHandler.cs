using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Marcidia.Logging.Configuration
{
    public class FileLoggerConfigurationHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            XmlAttribute attrib = section.Attributes["LogFolder"];

            if (attrib == null)
                throw new ConfigurationErrorsException("Missing LogFolder attribute", section);

            return attrib.Value;
        }
    }
}
