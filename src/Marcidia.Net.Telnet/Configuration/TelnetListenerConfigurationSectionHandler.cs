using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Net;

namespace Marcidia.Net.Telnet.Configuration
{
    class TelnetListenerConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            List<TelnetListenerConfiguration> telnetListenerConfiguration = new List<TelnetListenerConfiguration>();

            XmlNodeList connectionSources = section.SelectNodes("ConnectionSource");

            foreach (XmlNode connectionSourceNode in connectionSources)
            {
                telnetListenerConfiguration.Add(
                    ReadTelnetListenerConfiguration(connectionSourceNode));
            }

            return telnetListenerConfiguration;
        }

        private TelnetListenerConfiguration ReadTelnetListenerConfiguration(XmlNode connectionSourceNode)
        {
            string name = ReadExpectedAttribute(connectionSourceNode, "Name");
            IPAddress ipAddress = ReadExpectedIPAddressAttribute(connectionSourceNode, "IPAddress");
            int port = ReadExpectedIntegerAttribute(connectionSourceNode, "Port");

            return new TelnetListenerConfiguration(name, ipAddress, port);
        }

        private int ReadExpectedIntegerAttribute(XmlNode connectionSourceNode, string p)
        {
            int value = 0;

            string valueStr = ReadExpectedAttribute(connectionSourceNode, p);

            if (!int.TryParse(valueStr, out value))
                throw new ConfigurationErrorsException(
                    string.Format("The attribute {0} on the ConnectionSource element is not a valid integer", p),
                    connectionSourceNode);

            return value;
        }

        private IPAddress ReadExpectedIPAddressAttribute(XmlNode connectionSourceNode, string p)
        {
            IPAddress value = IPAddress.Any;

            string valueStr = ReadExpectedAttribute(connectionSourceNode, p);

            if (!IPAddress.TryParse(valueStr, out value))
                throw new ConfigurationErrorsException(
                    string.Format("The attribute {0} on the ConnectionSource element is not valid IP address", p),
                    connectionSourceNode);

            return value;
        }

        private string ReadExpectedAttribute(XmlNode connectionSourceNode, string p)
        {
            XmlAttribute attribute = connectionSourceNode.Attributes[p];

            if (attribute == null)
                throw new ConfigurationErrorsException(
                    string.Format("ConnectionSource element is missing expected attrbiute \"{0}\"", p),
                    connectionSourceNode);

            return attribute.Value;
        }
    }
}
