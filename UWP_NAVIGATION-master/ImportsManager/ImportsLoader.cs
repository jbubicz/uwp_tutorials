using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ImportManager
{
    public static class ImportsLoader
    {
        public static IEnumerable<T> ImportObjects<T>(List<Assembly> assemliesList)
        {
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemliesList);

            var compositionHost = configuration.CreateContainer();

            var result = compositionHost.GetExports<T>();

            return result;
        }

        public static IEnumerable<T> ImportObjects<T>(object objectWithLooseImports, List<Assembly> assemliesList)
        {
            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemliesList);

            var compositionHost = configuration.CreateContainer();
            compositionHost.SatisfyImports(objectWithLooseImports);

            var result = compositionHost.GetExports<T>();

            return result;
        }
    }
}