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
        private string[] IgnoredAssemblies =
        {
            "Marcidia.Core"
        };

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
            string[] assemblyNames = GetAssemblyNames();

            List<MarcidiaComponent> loadedComponents = new List<MarcidiaComponent>();

            foreach (var assemblyName in assemblyNames.Where(a => !IgnoredAssemblies.Contains(a)))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                
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

        private static string[] GetAssemblyNames()
        {
            string[] assemblyFiles = Directory.GetFiles(".\\", "*.dll");

            for (int i = 0; i < assemblyFiles.Length; i++)
            {
                assemblyFiles[i] = Path.GetFileName(assemblyFiles[i]).Replace(".dll", string.Empty);
            }

            return assemblyFiles;
        }
        private IEnumerable<MarcidiaComponent> LoadComponentsFromAssembly(Assembly assembly)
        {
            List<MarcidiaComponent> loadedComponents = new List<MarcidiaComponent>();

            IEnumerable<Type> componentTypes = GetComponentTypes(assembly);

            logger.Log(LogLevels.Standard, "=== Auto Component Loader ===");

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

            logger.Log(LogLevels.Standard, "Auto component loading completed");

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
