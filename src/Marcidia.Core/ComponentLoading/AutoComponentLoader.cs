using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marcidia.ComponentModel;
using System.IO;
using System.Reflection;
using Marcidia.Logging;

namespace Marcidia.ComponentLoading
{
    public class AutoComponentLoader : MarcidiaComponent
    {
        private ILogger logger;
        private Mud mud;
        private const string ComponentsFolder = "Components";

        public AutoComponentLoader(Mud mud)
            : base(mud)
        {
            this.mud = mud;
        }

        public override void Initialize()
        {
            logger = mud.Services.GetService<ILogger>();

            LoadComponents();
        }

        private  void LoadComponents()
        {
            if (!Directory.Exists(ComponentsFolder))
                return;

            string[] assemblyFiles = Directory.GetFiles(ComponentsFolder, "*.dll");

            List<MarcidiaComponent> loadedComponents = new List<MarcidiaComponent>();

            foreach (var assemblyFile in assemblyFiles)
            {
                Assembly assembly = Assembly.LoadFile(assemblyFile);
                
                loadedComponents.AddRange(LoadComponentsFromAssembly(assembly));                
            }
            
            foreach (var component in loadedComponents)
            {
                mud.Components.Add(component);
            }
            
            foreach (var component in loadedComponents)
            {
                component.Initialize();
            }
        }

        private IEnumerable<MarcidiaComponent> LoadComponentsFromAssembly(Assembly assembly)
        {
            List<MarcidiaComponent> loadedComponents = new List<MarcidiaComponent>();

            IEnumerable<Type> componentTypes = GetComponentTypes(assembly);

            foreach (var componentType in componentTypes)
            {
                MarcidiaComponentAttribute componentAttrib = GetComponentAttributeFromComponentType(componentType);

                if (componentAttrib == null)
                    continue;

                if (!componentAttrib.AutoLoad)
                    continue;
                
                MarcidiaComponent component = Activator.CreateInstance(componentType, mud) as MarcidiaComponent;

                loadedComponents.Add(component);

                logger.Log(
                    LogLevels.Standard, 
                    "Found Component: {0}, {1}. Written by: {2}", 
                    componentAttrib.Name, 
                    componentAttrib.Version, 
                    componentAttrib.Author);

            }

            return loadedComponents;
        }

        private static MarcidiaComponentAttribute GetComponentAttributeFromComponentType(Type componentType)
        {
            MarcidiaComponentAttribute componentAttrib = componentType.GetCustomAttributes(typeof(MarcidiaComponentAttribute), false)
                                                                                      .OfType<MarcidiaComponentAttribute>()
                                                                                      .FirstOrDefault();
            return componentAttrib;
        }

        private static IEnumerable<Type> GetComponentTypes(Assembly assembly)
        {
            // Get component types
            IEnumerable<Type> componentTypes = assembly.GetExportedTypes()
                                                       .Where(t => typeof(MarcidiaComponent).IsAssignableFrom(t));
            return componentTypes;
        }
    }
}
