using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.ComponentModel
{
    public class ComponentInfo
    {
        public Type ComponentType { get; private set; }        
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; }
        public string Website { get; private set; }

        public ComponentInfo(
            Type componentType,
            string name,
            string description,
            string author,
            string version,
            string website)
        {
            ComponentType = componentType;
            Name = name;
            Description = description;
            Author = author;
            Version = version;
            Website = website;
        }

        public static ComponentInfo GetInformationFor(MarcidiaComponent component)
        {
            Type type = component.GetType();

            object[] attribs = type.GetCustomAttributes(typeof(MarcidiaComponentAttribute), false);

            MarcidiaComponentAttribute attrib = attribs.Cast<MarcidiaComponentAttribute>().FirstOrDefault();
            
            if (attrib == null)
                return null;

            return new ComponentInfo(type, attrib.Name, attrib.Description, attrib.Author, attrib.Version, attrib.Website);

        }
    }
}
