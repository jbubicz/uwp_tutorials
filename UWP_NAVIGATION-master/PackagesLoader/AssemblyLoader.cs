using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Windows.UI.Notifications;

namespace SRBD_UWP.PackagesLoader
{
    public class AssemblyLoader
    {

        public string LocalDirecroryPath { get; private set; }

        public string FullDirectoryPath => Directory.GetCurrentDirectory() +"/"+ this.LocalDirecroryPath;

        public AssemblyLoader(string localDirecroryPath)
        {
            this.LocalDirecroryPath = localDirecroryPath;
        }

        public List<Assembly> getAssemblies()
        {
            List<Assembly> allAssemblies = new List<Assembly>();

            var modules = this.getModules();

            foreach (ModuleContainer module in modules)
            {
                string assembyName = module.GetDynamicObj().GetProperty("AssemblyName");
                Assembly assembly = Assembly.Load(new AssemblyName(assembyName));
                allAssemblies.Add(assembly);
            }

            return allAssemblies;
        }

        public ModuleContainer[] getModules()
        {
            XmlFileParser parser = new XmlFileParser(this.FullDirectoryPath);
            var modules = parser.GetModulesFromXml();

            return modules;
        }
    }
}