using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UEngine.Core
{
    public class FModuleManager
    {
        public static FModuleManager Singleton;

        public static FModuleManager Get()
        {
            if (Singleton == null)
            {
                Singleton = new FModuleManager();
            }
            return Singleton;
        }
        public Dictionary<string, IModuleInterface> Modules = new Dictionary<string, IModuleInterface>();

        public static T LoadModuleChecked<T>(string InModuleName) where T : class, IModuleInterface
        {
            IModuleInterface ModuleInterface = FModuleManager.Get().LoadModuleChecked(InModuleName);
            return ModuleInterface as T;
        }

        public IModuleInterface LoadModuleChecked(string InModuleName)
        {
            return LoadModule(InModuleName);
        }
        public IModuleInterface LoadModule(string InModuleName)
        {
            IModuleInterface LoadedModule = FindModule(InModuleName);
            if (LoadedModule != null)
            {
                return LoadedModule;
            }
            AddModule(InModuleName);
            LoadedModule = FindModule(InModuleName);
            LoadedModule.StartupModule();
            return LoadedModule;
        }
        public IModuleInterface FindModule(string InModuleName)
        {
            if (Modules.TryGetValue(InModuleName, out IModuleInterface module))
            {
                return module;
            };
            return null;
        }
        public void AddModule(string InModuleName)
        {
            Assembly[] Assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type moduleType = null;
            foreach (var assembly in Assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract || type.IsSealed)
                    {
                        continue;
                    }
                    var att = type.GetCustomAttribute<ModuleNameAttribute>(true);
                    if (att != null && att.ModuleName == InModuleName && type.GetInterface(typeof(IModuleInterface).Name) != null)
                    {
                        moduleType = type;
                        break;
                    }
                }
                if (moduleType != null)
                {
                    break;
                }
            }
            if (moduleType != null)
            {
                IModuleInterface module = Activator.CreateInstance(moduleType) as IModuleInterface;
                Modules.Add(InModuleName, module);
            }
        }
    }
}

