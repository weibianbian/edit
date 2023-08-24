using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{

    public class FModuleManager
    {
        public static FModuleManager Singleton;

        public static FModuleManager Get()
        {
            return Singleton;
        }
        public Dictionary<string, IModuleInterface> Modules = new Dictionary<string, IModuleInterface>();

        public void Init()
        {

        }

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

        }
    }
}

