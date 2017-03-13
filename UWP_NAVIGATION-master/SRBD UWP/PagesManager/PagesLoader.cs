using System.IO;
using System.Linq;
using Windows.UI.Xaml.Controls;
using ImportManager;
using SRBD_UWP.PackagesLoader;

namespace SRBD_UWP.PagesManager
{
    public static class PagesLoader
    {
        public static Page[] LoadImportingPages()
        {
            AssemblyLoader assemblyLoader =  new AssemblyLoader("LoadingModulesConfig.xml");
            var pages = ImportsLoader.ImportObjects<Page>(assemblyLoader.getAssemblies()).ToArray();
            return pages;
        }
        
    }
}