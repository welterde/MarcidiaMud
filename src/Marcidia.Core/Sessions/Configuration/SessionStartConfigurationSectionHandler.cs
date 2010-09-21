using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Marcidia.Sessions.Configuration
{
    class SessionStartConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            List<SessionStartConfiguration> configurations = new List<SessionStartConfiguration>();

            XmlNodeList sessionStartConfigNodes = section.SelectNodes("SessionStartConfiguration");

            foreach (XmlNode node in sessionStartConfigNodes)
            {
                configurations.Add(
                    ReadSessionStartConfiguration(node));
            }

            return configurations;
        }

        private SessionStartConfiguration ReadSessionStartConfiguration(XmlNode node)
        {
            string connectionSourceName = ReadExpectedAttribute(node, "ConnectionSource");
            string initialSessionStateTypeFullName = ReadExpectedAttribute(node, "InitialState");

            Type initialSessionStateType = Type.GetType(initialSessionStateTypeFullName);

            if (initialSessionStateType == null)
                throw new ConfigurationErrorsException(
                    string.Format(
                        "Initial Session State Type \"{0}\" could not be loaded, please check the containing assembly exists in the applications root directory",
                        initialSessionStateTypeFullName),
                    node);

            return new SessionStartConfiguration(initialSessionStateType, connectionSourceName);
        }

        private string ReadExpectedAttribute(XmlNode node, string attributeName)
        {
            var attribute = node.Attributes[attributeName];

            if (attribute == null)
                throw new ConfigurationErrorsException(
                    string.Format("Expected attribute \"{0}\" does not exist on node", attributeName),
                    node);

            return attribute.Value;
        }
    }
}
