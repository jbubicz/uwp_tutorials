using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Windows.Storage;

namespace SRBD_UWP.PackagesLoader
{
    public class XmlFileParser
    {
        private readonly XmlDocument document;

        public XmlFileParser(string path)
        {
            this.document = new XmlDocument();
            this.document.Load(XmlReader.Create(path));
        }

        public ModuleContainer[] GetModulesFromXml()
        {
            List<ModuleContainer> loadedModules =  new List<ModuleContainer>();

            foreach (XmlNode childNode in this.document.GetElementsByTagName("module"))
            {
                ModuleContainer moduleContainer = new ModuleContainer();

                foreach (XmlAttribute attribute in childNode.Attributes)
                {
                        moduleContainer.AddPropery(attribute.Name,attribute.Value);
                }

                loadedModules.Add(moduleContainer);
            }

            return loadedModules.ToArray();
        }
    }
}