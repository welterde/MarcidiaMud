using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marcidia.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class MarcidiaComponentAttribute : Attribute
    {        
        public MarcidiaComponentAttribute(string name, bool autoLoad)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("name is null or empty.", "name");

            Name = name;
            AutoLoad = autoLoad;
            Description = string.Empty;
            Author = string.Empty;
            Version = string.Empty;
            Website = string.Empty;
        }

        public string Name { get; private set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Website { get; set; }
        public bool AutoLoad { get; private set; }

    }
}
